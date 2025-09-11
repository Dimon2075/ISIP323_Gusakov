using System;


class Pr1
{
    static void Main()
    {
        const int minOp = 2;
        const int maxOp = 40;

        Console.WriteLine("Введите количество операций (от 2 до 40): ");
        int n;
        while (!int.TryParse(Console.ReadLine(), out n) || n < minOp || n > maxOp) ;
        {
            Console.WriteLine($"Ошибка! Введи от 2 до 40: ");
        }

        string[] names = new string[n];
        decimal[] amonts = new decimal[n];

        Console.WriteLine("Введите каждую операцию в формате: Название услуги или товара; Сумма в рублях");
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Операция {i + 1}: ");
            string input = Console.ReadLine();

            string[] parts = input.Split(";");
            if (parts.Length != 2)
            {
                Console.WriteLine("Неверный формат");
                i--;
                continue;
            }
            string namePart = parts[0].Trim();
            string amonPart = parts[1].Trim();

            decimal amount;
            if (!decimal.TryParse(amonPart, out amount) || amount < 0)
            {
                Console.WriteLine("Неверная сумма. Напишите снова.");
                i--;
                continue;
            }

            names[i] = namePart;
            amonts[i] = amount;
        }
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
            Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите пункт меню: ");

            string menu = Console.ReadLine();
            switch (menu)
            {
                case "1":
                    printD(names, amonts);
                    break;
                case "2":
                    Stat(amonts);
                    break;
                case "3":
                    Sort(names, amonts);
                    break;
                    Console.WriteLine("Данные отсартированы по цене");
                    printD (names, amonts);
                case "4":
                    Conver(names, amonts);
                    break;
                case "5":
                    poisk(names, amonts);
                    break;
                case "6":
                    Console.WriteLine("Выход из программы");
                    return;
                default:
                    Console.WriteLine("Неверный пункт");
                    break;
            }
        }
    }
    static void printD(string[] names, decimal[] amonts)
    {
        Console.WriteLine("\nСписок трат:");
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {names[i]} - {amonts[i]} руб.");
        }
    }

    static void Stat(decimal[] amonts)
    {
        decimal summa = 0;
        decimal max = amonts[0];
        decimal min = amonts[0];
        for (int i = 0; i < amonts.Length; i++)
        {
            summa += amonts[i];
            if (amonts[i] > max) max = amonts[i];
            if (amonts[i] < min) min = amonts[i];
        }
        decimal sred = summa / amonts.Length;

        Console.WriteLine($"\nСтатистика:");
        Console.WriteLine($"Сумма расходов: {summa} руб.");
        Console.WriteLine($"Среднее значение: {sred:F2} руб.");
        Console.WriteLine($"Максимальная сумма: {max} руб.");
        Console.WriteLine($"Минимальная сумма: {min} руб.");
    }

    static void Sort(string[] names, decimal[] amonts)
    {
        int n = amonts.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (amonts[j] > amonts[j + 1])
                {
                    decimal tempAm = amonts[j];
                    amonts[j] = amonts[j + 1];
                    amonts[j + 1] = tempAm;

                    string tempName = names[i];
                    names[i] = names[j];
                    names[j + 1] = tempName;

                    Console.WriteLine($"{tempAm} и {tempName}");
                }
            }
        }
    }

    static void Conver(string[] names, decimal[] amonts)
    {
        Console.WriteLine("Конвертация валюты:");
        Console.WriteLine("1. Введите курс вручную");
        Console.WriteLine("2. Выбрать из списка (USD, EUR, GBP)");
        Console.Write("Выберите опцию: ");
        string option = Console.ReadLine();

        decimal val = 0;
        string cur = "";
        if (option == "1")
        {
            Console.Write("Введите курс (сколько рублей в 1 единице выбранной валюты): ");
            while (!decimal.TryParse(Console.ReadLine(), out val) || val <= 0)
            {
                Console.WriteLine("Некорректный курс. Попробуйте еще раз:");
            }
            cur = "выбранной валюты";
        }
        else if (option == "2")
        {
            Console.WriteLine("Выберите валюту:");
            Console.WriteLine("1. USD (Курс: 75.0)");
            Console.WriteLine("2. EUR (Курс: 90.0)");
            Console.WriteLine("3. GBP (Курс: 100.0)");
            Console.Write("Ввод: ");
            string curMenu = Console.ReadLine();

            switch (curMenu)
            {
                case "1":
                    val = 75m;
                    cur = "USD";
                    break;
                case "2":
                    val = 90m;
                    cur = "EUR";
                    break;
                case "3":
                    val = 100m;
                    cur = "GBP";
                    break;
                default:
                    Console.WriteLine("Неверный выбор валюты.");
                    return;
            }
        }
        else
        {
            Console.WriteLine("Неверный выбор опции.");
            return;
        }

        Console.WriteLine($"\nТраты в {cur}:");
        for (int i = 0; i < amonts.Length; i++)
        {
            decimal converted = amonts[i] / val;
            Console.WriteLine($"{names[i]} - {converted:F2} {cur}");
        }
    }
    static void poisk(string[] names, decimal[] amonts)
    {
        Console.Write("Введите часть или полное название для поиска: ");
        string qr = Console.ReadLine().ToLower();

        bool fnd = false;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].ToLower().Contains(qr))
            {
                Console.WriteLine($"{names[i]} - {amonts[i]} руб.");
                fnd = true;
            }
        }

        if (!fnd)
        {
            Console.WriteLine("Совпадений не найдено.");
        }
    }
}