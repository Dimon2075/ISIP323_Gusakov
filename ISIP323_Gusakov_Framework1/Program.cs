using ISIP323_Gusakov_Framework1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//1
namespace MarketPlace
{
    internal class Program
    {
        private static Users1 currentUser = null;
        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("== Регистрация нового пользователя ==");
            Console.Write("Введите Username: ");
            string username = Console.ReadLine(); 
            Console.Write("Введите Email: ");
            string email = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string pass1 = Console.ReadLine();
            Console.Write("Подтвердите пароль: ");
            string pass2 = Console.ReadLine();
            if (pass1 != pass2)
            {
                Console.WriteLine("Ошибка: Пароли не совпадают!");
                Console.ReadLine();
            }
            if (Core.Context.Users1.Any(u => u.Username == username || u.Email == email))
            {
                Console.WriteLine("Ошибка: Пользователь с таким Email или Username уже существует!");
                Console.ReadLine();
            }
            string passHash = SimpleHash(pass1);
            Users1 newUser = new Users1
            {
                Username = username,
                Email = email,
                PasswordHash = passHash
            };
            Core.Context.Users1.Add(newUser);
            try
            {
                Core.Context.SaveChanges();
                Carts1 newCart = new Carts1
                {
                    UserID = newUser.UserID 
                };
                Core.Context.Carts1.Add(newCart);
                Core.Context.SaveChanges();
                Console.WriteLine("Регистрация прошла успешно! Теперь вы можете войти.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в БД: {ex.Message}");
            }
            Console.ReadLine();
        }
        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("== Вход в аккаунт ==");
            Console.Write("Введите Email: ");
            string email = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            string passHash = SimpleHash(password);
            Users1 user = Core.Context.Users1.FirstOrDefault(u => u.Email == email && u.PasswordHash == passHash);
            if (user == null)
            {
                Console.WriteLine("Ошибка: Неверный Email или пароль!");
                Console.ReadLine();
            }
            else
            {
                currentUser = user;
                Console.WriteLine($"Добро пожаловать, {currentUser.Username}!");
                Console.ReadLine();
            }
        }
        private static void Logout()
        {
            currentUser = null;
            Console.WriteLine("Вы вышли из аккаунта.");
            Console.ReadLine();
        }
        private static void ViewProducts()
        {
            Console.Clear();
            Console.WriteLine("== Список доступных товаров ==");
            var products = Core.Context.Products
                .Where(p => p.StockQuantity > 0)
                .ToList();
            if (!products.Any())
            {
                Console.WriteLine("Товаров в наличии нет.");
                Console.ReadLine();
            }
            foreach (var p in products)
            {
                Console.WriteLine($"[ID: {p.ProductID}] {p.Name} - {p.Price} руб.");
                Console.WriteLine($"    (Остаток: {p.StockQuantity} шт.) Описание: {p.Description}");
                Console.WriteLine();
            }
            if (currentUser != null)
            {
                Console.WriteLine("-----------------------------------");
                Console.Write("Введите ID товара для добавления в корзину (или 0 для возврата): ");
                string choice = Console.ReadLine();
                if (int.TryParse(choice, out int productID) && productID != 0)
                {
                    AddToCart(productID);
                }
            }
            else
            {
                Console.WriteLine("Войдите в аккаунт, чтобы добавлять товары в корзину.");
                Console.ReadLine();
            }
        }
        private static void AddToCart(int productID)
        {
            var product = Core.Context.Products.Find(productID);
            if (product == null || product.StockQuantity <= 0)
            {
                Console.WriteLine("Такого товара нет или он закончился.");
                Console.ReadLine();
            }
            
            Console.Write($"Введите количество (доступно: {product.StockQuantity}): ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неверное количество.");
                Console.ReadLine();
            }
            if (quantity > product.StockQuantity)
            {
                Console.WriteLine($"Ошибка: Недостаточно товара на складе (Остаток: {product.StockQuantity})");
                Console.ReadLine();
            }
            var cart = Core.Context.Carts1.FirstOrDefault(c => c.UserID == currentUser.UserID);
            if (cart == null)
            {
                Console.WriteLine("Критическая ошибка: Корзина не найдена!");
                Console.ReadLine();
                return;
            }
            var cartItem = Core.Context.CartItems
                .FirstOrDefault(ci => ci.CartID == cart.CartID && ci.ProductID == productID);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItems
                {
                    CartID = cart.CartID,
                    ProductID = productID,
                    Quantity = quantity
                };
                Core.Context.CartItems.Add(cartItem);
            }
            try
            {
                Core.Context.SaveChanges();
                
               
                Console.WriteLine($"Товар '{product.Name}' (x{quantity}) добавлен в корзину.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
            Console.ReadLine();
        }
        private static void ViewCart()
        {
            Console.Clear();
            Console.WriteLine("== Моя корзина ==");
            var cart = Core.Context.Carts1.FirstOrDefault(c => c.UserID == currentUser.UserID);
            var items = Core.Context.CartItems.Where(ci => ci.CartID == cart.CartID).Include(ci => ci.Products).ToList();
            if (!items.Any())
            {
                Console.WriteLine("Ваша корзина пуста.");
                Console.ReadLine();
                return;
            }
            decimal totalPrice = 0;
            foreach (var item in items)
            {
                if (item.Products != null)
                {
                    decimal itemTotalPrice = item.Products.Price * item.Quantity;
                    Console.WriteLine($"Товар: {item.Products.Name}");
                    Console.WriteLine($"   Кол-во: {item.Quantity} x {item.Products.Price} руб. = {itemTotalPrice} руб.");
                    totalPrice += itemTotalPrice;
                }
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"Итоговая сумма: {totalPrice} руб.");
            Console.WriteLine();
            Console.Write("Хотите оформить заказ? (Да/Нет): ");
            string choice = Console.ReadLine();
            if (choice.Equals("Да", StringComparison.OrdinalIgnoreCase))//перечисление используется для указания способа сравнения строк без учета регистора
            {
                CreateOrder(items, totalPrice);
            }
        }
        private static void CreateOrder(System.Collections.Generic.List<CartItems> items, decimal totalPrice)
        {
            using (var transaction = Core.Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in items)
                    {
                        var productInDb = Core.Context.Products.Find(item.ProductID);
                        if (productInDb.StockQuantity < item.Quantity)
                        {
                            Console.WriteLine($"Ошибка: Товара '{productInDb.Name}' не осталось на складе (Остаток: {productInDb.StockQuantity}).");
                            Console.WriteLine("Заказ отменен.");
                            transaction.Rollback();
                            Console.ReadLine();
                            return;
                        }
                    }
                    Console.WriteLine("== Выбор пункта выдачи заказов (ПВЗ) ==");
                    var pickupPoints = Core.Context.PickupPoints.ToList();
                    if (!pickupPoints.Any())
                    {
                        Console.WriteLine("Ошибка: Нет доступных ПВЗ. Заказ невозможен.");
                        transaction.Rollback();
                        Console.ReadLine();
                        return;
                    }
                    foreach (var p in pickupPoints)
                    {
                        Console.WriteLine($"[ID: {p.PickupPointID}] {p.Address} (Часы работы: {p.OperatingHours})");
                    }
                    int pickupID = 0;
                    while (true)
                    {
                        Console.Write("Введите ID ПВЗ: ");
                        if (int.TryParse(Console.ReadLine(), out pickupID) && pickupPoints.Any(p => p.PickupPointID == pickupID))
                        {
                            break;
                        }
                        Console.WriteLine("Неверный ID. Попробуйте снова.");
                    }
                    Orders newOrder = new Orders
                    {
                        UserID = currentUser.UserID,
                        PickupPointID = pickupID,
                        OrderDate = DateTime.Now,
                        Status = "В обработке",
                        TotalPrice = totalPrice
                    };
                    Core.Context.Orders.Add(newOrder);
                    Core.Context.SaveChanges();
                    foreach (var item in items)
                    {
                        OrderItems orderItem = new OrderItems
                        {
                            OrderID = newOrder.OrderID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            PriceAtPurchase = item.Products.Price
                        };
                        Core.Context.OrderItems.Add(orderItem);
                        var productToUpdate = Core.Context.Products.Find(item.ProductID);
                        productToUpdate.StockQuantity -= item.Quantity;
                    }
                    Core.Context.CartItems.RemoveRange(items);
                    Core.Context.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine($"Заказ №{newOrder.OrderID} успешно создан!");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Критическая ошибка при оформлении заказа: {ex.Message}");
                    transaction.Rollback();
                    Console.ReadLine();
                }
            }
        }
        private static void ViewOrderHistory() 
        {
            Console.Clear();
            Console.WriteLine("== Моя история заказов ==");
            var orders = Core.Context.Orders
                .Where(o => o.UserID == currentUser.UserID)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            if (!orders.Any())
            {
                Console.WriteLine("У вас пока нет заказов.");
                Console.ReadLine();
                return;
            }
            foreach (var order in orders)
            {
                Console.WriteLine($"Заказ: №{order.OrderID} от {order.OrderDate.ToShortDateString()}");
                Console.WriteLine($"   Статус: {order.Status}, Сумма: {order.TotalPrice} руб.");
            }
            Console.WriteLine("-----------------------------------");
            Console.Write("Введите ID заказа для просмотра деталей (или 0 для возврата): ");
            if (int.TryParse(Console.ReadLine(), out int orderID) && orderID != 0)
            {
                ShowOrderDetails(orderID);
            }
        }
        private static void ShowOrderDetails(int orderID)
        {
            var order = Core.Context.Orders
                .Include(o => o.PickupPoints)
                .FirstOrDefault(o => o.OrderID == orderID && o.UserID == currentUser.UserID);
            if (order == null)
            {
                Console.WriteLine("Заказ не найден или он вам не принадлежит.");
                Console.ReadLine();
                return;
            }
            Console.Clear();
            Console.WriteLine($"== Детали заказа №{order.OrderID} ==");
            Console.WriteLine($"Дата: {order.OrderDate}, Статус: {order.Status}, Сумма: {order.TotalPrice} руб.");
            Console.WriteLine($"Пункт выдачи: {order.PickupPoints.Address} ({order.PickupPoints.OperatingHours})");
            Console.WriteLine();
            Console.WriteLine("Состав заказа:");
            var orderItems = Core.Context.OrderItems
                .Where(oi => oi.OrderID == orderID)
                .Include(oi => oi.Products)
                .ToList();
            foreach (var item in orderItems)
            {
                Console.WriteLine($" - {item.Products.Name} (x{item.Quantity} шт. по {item.PriceAtPurchase} руб.)");
            }
            Console.ReadLine();
        }
        private static void MainLoop()
        {
            while (true)
            {
                Console.Clear();
                if (currentUser == null)
                {
                    ShowGuestMenu();
                }
                else
                {
                    ShowUserMenu();
                }
            }
        }
        private static void ShowGuestMenu()
        {
            Console.WriteLine("Добро пожаловать в маркетплейс WONGG!");
            Console.WriteLine("1. Войти в аккаунт");
            Console.WriteLine("2. Зарегистрироваться");
            Console.WriteLine("3. Просмотреть товары");
            Console.WriteLine("0. Выйти из программы");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    ViewProducts();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    break;
            }
        }
        private static void ShowUserMenu()
        {
            Console.WriteLine($"Вы вошли как: {currentUser.Username}");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("1. Просмотреть товары (и добавить в корзину)");
            Console.WriteLine("2. Просмотреть мою корзину");
            Console.WriteLine("3. Просмотреть историю моих заказов");
            Console.WriteLine("9. Выйти из аккаунта");
            Console.WriteLine("0. Выйти из программы");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewProducts();
                    break;
                case "2":
                    ViewCart();
                    break;
                case "3":
                    ViewOrderHistory();
                    break;
                case "9":
                    Logout();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    break;
            }
        }
        private static string SimpleHash(string password)
        {
            char[] charArray = password.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        static void Main(string[] args)
        {
            MainLoop();
        }
    }
}