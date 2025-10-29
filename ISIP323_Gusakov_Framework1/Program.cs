using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ISIP323_Gusakov_Framework1
{
    public static class GameAuto
    {
        private static Random rnd = new Random();
        public static void ObnPostavok()
        {
            var deliveries = Core.Context.PendingDeliveries.ToList();
            if (!deliveries.Any()) return;
            Console.WriteLine("\nОбновление поставок");
            foreach (var delivery in deliveries)
            {
                delivery.ClientsToWait--;
                if (delivery.ClientsToWait <= 0)
                {
                    var warehouseItem = Core.Context.WarehouseItems.FirstOrDefault(w => w.PartTypeID == delivery.PartTypeID);
                    var partName = Core.Context.PartTypes.First(p => p.PartTypeID == delivery.PartTypeID).Name;
                    if (warehouseItem != null)
                    {
                        warehouseItem.Quantity += delivery.Quantity;
                    }
                    else
                    {
                        Core.Context.WarehouseItems.Add(new WarehouseItems
                        {

                            PartTypeID = delivery.PartTypeID,
                            Quantity = delivery.Quantity
                        });
                    }
                    Console.WriteLine($"Прибыла поставка: {partName} (x{delivery.Quantity})!");
                    Core.Context.PendingDeliveries.Remove(delivery);
                }
            }
            Core.Context.SaveChanges();
        }
        public static void ProcessClientTurn()
        {
            var game = Core.Context.GameStatus.First();
            Console.WriteLine($"\nНовый клиент! Ваш баланс: {game.Balance:N2} руб");
            int maxPartId = Core.Context.PartTypes.Max(p => p.PartTypeID);
            int randomPartId = rnd.Next(1, maxPartId + 1);
            var neededPart = Core.Context.PartTypes.First(p => p.PartTypeID == randomPartId);
            decimal repairCost = neededPart.ShopPrice + neededPart.LaborCost;
            decimal penaltyRefuse = neededPart.ShopPrice * 0.5m;
            decimal penaltyFail = repairCost * 1.5m;
            Console.WriteLine($"Клиент приехал с поломкой: '{neededPart.Name}'");
            Console.WriteLine($"Стоимость ремонта для клиента: {repairCost:N2} руб (деталь {neededPart.ShopPrice:N2} руб + работа {neededPart.LaborCost:N2} руб)");
            var partInStock = Core.Context.WarehouseItems.FirstOrDefault(w => w.PartTypeID == neededPart.PartTypeID && w.Quantity > 0);
            if (partInStock != null)
            {
                Console.WriteLine($"У вас на складе: {partInStock.Quantity} шт.");
            }
            else
            {
                Console.WriteLine("У вас на складе НЕТ такой детали!");
            }
            Console.WriteLine("\nВаши действия:");
            Console.WriteLine("1. Принять заказ");
            Console.WriteLine("2. Отказаться от заказа (штраф {0:N2} руб)", penaltyRefuse);
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                if (partInStock != null)
                {
                    partInStock.Quantity--;
                    game.Balance += repairCost;
                    Core.Context.SaveChanges();
                    Console.WriteLine($"\nУспешный ремонт! +{repairCost:N2} руб.");
                    Console.WriteLine($"Деталей '{neededPart.Name}' осталось: {partInStock.Quantity} шт.");
                    Console.ResetColor();
                }
                else
                {
                    game.Balance -= penaltyFail;
                    Core.Context.SaveChanges();
                    Console.WriteLine($"\nКРИТИЧЕСКИЙ ПРОВАЛ! Вы взяли заказ без детали!");
                    Console.WriteLine($"Вы заплатили неустойку клиенту: -{penaltyFail:N2} руб");
                }
            }
            else
            {
                game.Balance -= penaltyRefuse;
                Core.Context.SaveChanges();
                Console.WriteLine($"\nВы отказались от заказа. Штраф: -{penaltyRefuse:N2} руб");
            }
        }
        public static void ShowShop()
        {
            Console.WriteLine("\nХотите зайти в магазин запчастей? (y/n)");
            if (Console.ReadLine().ToLower() != "y")
            {
                return;
            }
            var game = Core.Context.GameStatus.First();
            Console.WriteLine("\nМАГАЗИН ЗАПЧАСТЕЙ");
            Console.WriteLine($"Ваш баланс: {game.Balance:N2} руб");
            var allParts = Core.Context.PartTypes.ToList();
            foreach (var part in allParts)
            {
                Console.WriteLine($"{part.PartTypeID}. {part.Name} - {part.ShopPrice:N2} руб / шт.");
            }
            Console.WriteLine("0. Выйти из магазина");
            while (true)
            {
                Console.Write("Введите ID детали для покупки (или 0): ");
                if (!int.TryParse(Console.ReadLine(), out int partId) || partId == 0)
                {
                    break;
                }
                var partToBuy = allParts.FirstOrDefault(p => p.PartTypeID == partId);
                if (partToBuy == null)
                {
                    Console.WriteLine("Такой детали нет!");
                    continue;
                }
                Console.Write($"Сколько '{partToBuy.Name}' хотите купить? ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Неверное количество.");
                    continue;
                }
                decimal totalCost = partToBuy.ShopPrice * quantity;
                if (game.Balance < totalCost)
                {
                    Console.WriteLine($"Недостаточно денег! Нужно {totalCost:N2} руб, у вас {game.Balance:N2} руб");
                    continue;
                }
                game.Balance -= totalCost;
                Core.Context.PendingDeliveries.Add(new PendingDeliveries
                {
                    PartTypeID = partToBuy.PartTypeID,
                    Quantity = quantity,
                    ClientsToWait = 2
                });
                Core.Context.SaveChanges();
                Console.WriteLine($"\nУспешно куплено: {partToBuy.Name} (x{quantity}) за {totalCost:N2} руб");
                Console.WriteLine("Поставка прибудет через 2-х клиентов.");
                Console.WriteLine($"Остаток баланса: {game.Balance:N2} руб");
            }
            Console.WriteLine("Выход из магазина");
            Console.ResetColor();
        }
        public static bool CheckGameOver()
        {
            var game = Core.Context.GameStatus.First();
            var totalParts = Core.Context.WarehouseItems.Sum(w => (int?)w.Quantity) ?? 0;
            if (totalParts == 0)
            {
                decimal cheapestPartPrice = Core.Context.PartTypes.Min(p => p.ShopPrice);
                if (game.Balance < cheapestPartPrice)
                {
                    Console.WriteLine("\nИГРА ОКОНЧЕНА");
                    Console.WriteLine("У вас не осталось запчастей на складе и не хватает денег,");
                    Console.WriteLine($"чтобы купить даже самую дешевую деталь ({cheapestPartPrice:N2} руб).");
                    Console.WriteLine($"Ваш финальный баланс: {game.Balance:N2} руб");
                    return true;
                }
            }
            return false;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Ваш Автосервис!");  
                while (true)
                {
                    GameAuto.ObnPostavok();
                    GameAuto.ProcessClientTurn();
                    if (GameAuto.CheckGameOver())
                    {
                        break;
                    }
                    GameAuto.ShowShop();
                }
            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}