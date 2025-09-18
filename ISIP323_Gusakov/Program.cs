using System;
using System.Collections.Generic;
using System.Linq;

enum Category //категории
{
    Electronics = 1,
    Food = 2,
    Clothing = 3
}

class Prod
{
    public string Code { get; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Category Category { get; set; }

    public Prod(string name, decimal price, int quantity, Category category)// Конструктор с генерацией уникального кода
    {
        Code = Guid.NewGuid().ToString().Substring(0, 6);
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
    }

    public override string ToString() // Переопределение ToString для удобного вывода информации о товаре
    {
        return $"Код: {Code}, Название: {Name}, Цена: {Price}, Кол-во: {Quantity}, Категория: {Category}";
    }
}

class Store
{
    protected List<Prod> products = new List<Prod>();

    public void AddProduct(Prod product) => products.Add(product);

    public bool DeleteProduct(string code)
    {
        var product = products.Find(p => p.Code == code);
        if (product == null) return false;
        products.Remove(product);
        return true;
    }

    public Prod FindByCode(string code) => products.Find(p => p.Code == code);

    public List<Prod> FindByName(string name) =>
        products.Where(p => p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

    public List<Prod> FindByCategory(Category category) =>
        products.Where(p => p.Category == category).ToList();

    public bool RestockProduct(string code, int amount)
    {
        var product = FindByCode(code);
        if (product == null) return false;
        product.Quantity += amount;
        return true;
    }

    public bool SellProduct(string code, int amount)
    {
        var product = FindByCode(code);
        if (product == null || product.Quantity < amount) return false;
        product.Quantity -= amount;
        return true;
    }

    public void ShowProductDetails(Prod product)
    {
        if (product != null)
            Console.WriteLine(product);
        else
            Console.WriteLine("Товар не найден.");
    }
}

class Program
{
    static Store store = new Store();

    static void Main()
    {
        store.AddProduct(new Prod("Смартфон Samsung", 29999.99m, 10, Category.Electronics));
        store.AddProduct(new Prod("Хлеб ржаной", 35.50m, 50, Category.Food));
        store.AddProduct(new Prod("Футболка Adidas", 1499.99m, 25, Category.Clothing));
        store.AddProduct(new Prod("Ноутбук Lenovo", 59999.00m, 5, Category.Electronics));
        store.AddProduct(new Prod("Молоко 1л", 70.00m, 30, Category.Food));
        store.AddProduct(new Prod("Джинсы Wrangler", 3499.00m, 15, Category.Clothing));
        while (true)
        {
            ShowMenu();
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();

            switch (choice.Trim())
            {
                case "1": AddProductMenu(); break;
                case "2": DeleteProductMenu(); break;
                case "3": RestockProductMenu(); break;
                case "4": SellProductMenu(); break;
                case "5": SearchProductMenu(); break;
                case "0": Console.WriteLine("Выход."); return;
                default: Console.WriteLine("Недопустимая команда."); break;
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n--- Главное меню ---");
        Console.WriteLine("1 - Добавить товар");
        Console.WriteLine("2 - Удалить товар");
        Console.WriteLine("3 - Заказать поставку");
        Console.WriteLine("4 - Продать товар");
        Console.WriteLine("5 - Поиск товаров");
        Console.WriteLine("0 - Выход");
    }

    static void AddProductMenu()
    {
        Console.Write("Название: ");
        string name = Console.ReadLine();
        Console.Write("Цена: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
        {
            Console.WriteLine("Некорректная цена");
            return;
        }
        Console.Write("Количество: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
        {
            Console.WriteLine("Некорректное количество");
            return;
        }
        Console.WriteLine("Категории:");
        foreach (var c in Enum.GetValues(typeof(Category)))
            Console.WriteLine($"{(int)c} - {c}");
        Console.Write("Выбор категории: ");
        if (!int.TryParse(Console.ReadLine(), out int catChoice) || !Enum.IsDefined(typeof(Category), catChoice))
        {
            Console.WriteLine("Некорректный выбор категории");
            return;
        }
        Category category = (Category)catChoice;

        var product = new Prod(name, price, quantity, category);
        store.AddProduct(product);
        Console.WriteLine("Товар добавлен:\n" + product);
    }

    static void DeleteProductMenu()
    {
        Console.Write("Введите код товара: ");
        string code = Console.ReadLine();
        if (store.DeleteProduct(code))
            Console.WriteLine("Товар удален");
        else
            Console.WriteLine("Товар не найден");
    }

    static void RestockProductMenu()
    {
        Console.Write("Введите код товара для пополнения: ");
        string code = Console.ReadLine();
        Console.Write("Количество для добавления: ");
        if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
        {
            Console.WriteLine("Некорректное количество");
            return;
        }
        if (store.RestockProduct(code, amount))
            Console.WriteLine("Поставка выполнена");
        else
            Console.WriteLine("Товар не найден");
    }

    static void SellProductMenu()
    {
        Console.Write("Введите код товара для продажи: ");
        string code = Console.ReadLine();
        Console.Write("Количество для продажи: ");
        if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
        {
            Console.WriteLine("Некорректное количество");
            return;
        }
        if (store.SellProduct(code, amount))
            Console.WriteLine("Продажа выполнена");
        else
            Console.WriteLine("Недостаточно товара или товар не найден");
    }

    static void SearchProductMenu()
    {
        Console.WriteLine("Поиск по:\n1 - по коду\n2 - по названию\n3 - по категории");
        Console.Write("Выбор: ");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.Write("Введите код: ");
                var code = Console.ReadLine();
                var productByCode = store.FindByCode(code);
                store.ShowProductDetails(productByCode);
                break;
            case "2":
                Console.Write("Введите название: ");
                var name = Console.ReadLine();
                var listByName = store.FindByName(name);
                if (listByName.Any())
                    foreach (var p in listByName)
                        store.ShowProductDetails(p);
                else
                    Console.WriteLine("Товары не найдены");
                break;
            case "3":
                Console.WriteLine("Категории:");
                foreach (var c in Enum.GetValues(typeof(Category)))
                    Console.WriteLine($"{(int)c} - {c}");
                Console.Write("Выбор категории: ");
                if (int.TryParse(Console.ReadLine(), out int catChoice) && Enum.IsDefined(typeof(Category), catChoice))
                {
                    var category = (Category)catChoice;
                    var listByCat = store.FindByCategory(category);
                    if (listByCat.Any())
                        foreach (var p in listByCat)
                            store.ShowProductDetails(p);
                    else
                        Console.WriteLine("Товары не найдены.");
                }
                else
                {
                    Console.WriteLine("Некорректный выбор категории.");
                }
                break;
            default:
                Console.WriteLine("Некорректный выбор");
                break;
        }
    }
}
