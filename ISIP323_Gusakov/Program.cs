using System;
using System.Collections.Generic;
using System.Linq;
public enum Genr
{
    Fiction,
    NonFiction,
    Mystery,
    ScienceFiction,
    Fantasy,
    Biograhy
}
public class Book
{
    private static int PervID = 1;
    public int ID { get; private set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Genr Genr { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public Book(string title, string author, Genr genr, int year, decimal price)
    {
        ID = PervID;
        Title = title;
        Author = author;
        Genr = genr;
        Year = year;
        Price = price;
    }
}
public class Libr
{
    private List<Book> books = new List<Book>(); 
    public void AddBook(Book book)
    {
        if (book != null)
        {
            books.Add(book);
        }
    }
    public void RemoveBook(int Id)
    {
        var Rem = books.FirstOrDefault(b => b.ID == Id);
        if (Rem != null)
        {
            books.Remove(Rem);
        }
    }
    public IEnumerable<Book> PoiskNazvania(string title)
    {
        return books.Where(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
    public IEnumerable<Book> PoickAutora(string author)
    {
        return books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }
    public IEnumerable<Book> PoiskGenr(Genr genr)
    {
        return books.Where(b => b.Genr == genr);
    }
    public IEnumerable<Book> SortNazv()
    {
        return books.OrderBy(b => b.Title);
    }
    public IEnumerable<Book> SortYear()
    {
        return books.OrderBy(b => b.Year);
    }
    public Book Dorogaya()
    {
        if (!books.Any())
            return null;
        return books.OrderByDescending(b => b.Price).First();
    }
    public Book Dehevaa()
    {
        if (!books.Any())
            return null;
        return books.OrderBy(b => b.Price).First();
    }
    public Dictionary<string, int> Static()
    {
        return books
           .GroupBy(b => b.Author)
           .ToDictionary(g => g.Key, g => g.Count());
    }
    public void initial()
    {
        AddBook(new Book("Гордость и предубеждение", "Джейн Остин", Genr.Fiction, 1813, 12.50m));
        AddBook(new Book("Мастер и Маргарита", "Михаил Булгаков", Genr.Fiction, 1967, 15.00m));
        AddBook(new Book("Курс по C#", "Автор1", Genr.NonFiction, 2020, 40.00m));
        AddBook(new Book("Мистическая тайна", "Автор2", Genr.Mystery, 2015, 8.99m));
        AddBook(new Book("Наука и фантастика", "Автор3", Genr.ScienceFiction, 2010, 20.00m));
    }
    public void ShowAllBooks()
    {
        if (!books.Any())
        {
            Console.WriteLine("Книги отсутствуют.");
            return;
        }
        foreach (var b in books)
        {
            Console.WriteLine($"ID: {b.ID}, Название: {b.Title}, Автор: {b.Author}, Жанр: {b.Genr}, Год: {b.Year}, Цена: {b.Price}");
        }
    }
}
class Program
{
    static Libr library = new Libr();  
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Библиотека ---");
            Console.WriteLine("1. Показать все книги");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу по ID");
            Console.WriteLine("4. Найти книги по названию");
            Console.WriteLine("5. Найти книги по автору");
            Console.WriteLine("6. Найти книги по жанру");
            Console.WriteLine("7. Отсортировать книги по названию");
            Console.WriteLine("8. Отсортировать книги по году");
            Console.WriteLine("9. Самая дорогая книга");
            Console.WriteLine("10. Самая дешевая книга");
            Console.WriteLine("11. Статистика по авторам");
            Console.WriteLine("12. Тестовые данные");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    library.ShowAllBooks();
                    break;
                case "2":
                    AddBook();
                    break;
                case "3":
                    RemoveBook();
                    break;
                case "4":
                    PoiskNazvania();
                    break;
                case "5":
                    PoickAutora();
                    break;
                case "6":
                    PoiskGenr();
                    break;
                case "7":
                    SortNazv();
                    break;
                case "8":
                    SortYear();
                    break;
                case "9":
                    Dorogaya();
                    break;
                case "10":
                    Dehevaa();
                    break;
                case "11":
                    Static();
                    break;
                case "12":
                    library.initial();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Некорректный ввод, попробуйте еще раз.");
                    break;
            }
        }
    }
    static void AddBook()
    {
        Console.Write("Введите название книги: ");
        string title = Console.ReadLine();
        Console.Write("Введите автора: ");
        string author = Console.ReadLine();
        Console.WriteLine("Выберите жанр:");
        var genres = Enum.GetValues(typeof(Genr));
        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
        }
        int genreChoice = int.Parse(Console.ReadLine());
        Genr genreSelected = (Genr)genres.GetValue(genreChoice - 1);

        Console.Write("Введите год выпуска: ");
        int year = int.Parse(Console.ReadLine());

        Console.Write("Введите цену: ");
        decimal price = decimal.Parse(Console.ReadLine());

        library.AddBook(new Book(title, author, genreSelected, year, price));
        Console.WriteLine("Книга добавлена успешно.");
    }
    static void RemoveBook()
    {
        Console.Write("Введите ID книги для удаления: ");
        int id = int.Parse(Console.ReadLine());
        library.RemoveBook(id);
        Console.WriteLine("Если книга с таким ID существует, она удалена.");
    }
    static void PoiskNazvania()
    {
        Console.Write("Введите название книги: ");
        string title = Console.ReadLine();
        var results = library.PoiskNazvania(title);
        foreach (var b in results)
        {
            Console.WriteLine($"ID: {b.ID}, Название: {b.Title}, Автор: {b.Author}, Жанр: {b.Genr}, Год: {b.Year}, Цена: {b.Price}");
        }
    }
    static void PoickAutora()
    {
        Console.Write("Введите автора: ");
        string author = Console.ReadLine();
        var results = library.PoickAutora(author);
        foreach (var b in results)
        {
            Console.WriteLine($"ID: {b.ID}, Название: {b.Title}, Автор: {b.Author}, Жанр: {b.Genr}, Год: {b.Year}, Цена: {b.Price}");
        }
    }
    static void PoiskGenr()
    {
        Console.WriteLine("Выберите жанр:");
        var genres = Enum.GetValues(typeof(Genr));
        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
        }
        int genreChoice = int.Parse(Console.ReadLine());
        Genr selectedGenre = (Genr)genres.GetValue(genreChoice - 1);
        var results = library.PoiskGenr(selectedGenre);
        foreach (var b in results)
        {
            Console.WriteLine($"ID: {b.ID}, Название: {b.Title}, Автор: {b.Author}, Жанр: {b.Genr}, Год: {b.Year}, Цена: {b.Price}");
        }
    }
    static void SortNazv()
    {
        var sorted = library.SortNazv();
        foreach (var b in sorted)
        {
            Console.WriteLine($"ID: {b.ID}, Название: {b.Title}, Автор: {b.Author}, Жанр: {b.Genr}, Год: {b.Year}, Цена: {b.Price}");
        }
    }
    static void SortYear()
    {
        var sorted = library.SortYear();
        foreach (var b in sorted)
        {
            Console.WriteLine($"ID: {b.ID}, Название: {b.Title}, Автор: {b.Author}, Жанр: {b.Genr}, Год: {b.Year}, Цена: {b.Price}");
        }
    }
    static void Dorogaya()
    {
        var book = library.Dorogaya();
        if (book != null)
        {
            Console.WriteLine($"Самая дорогая книга: {book.Title}, цена: {book.Price}");
        }
        else
        {
            Console.WriteLine("Книги отсутствуют.");
        }
    }
    static void Dehevaa()
    {
        var book = library.Dehevaa();
        if (book != null)
        {
            Console.WriteLine($"Самая дешевая книга: {book.Title}, цена: {book.Price}");
        }
        else
        {
            Console.WriteLine("Книги отсутствуют.");
        }
    }
    static void Static()
    {
        var stats = library.Static();
        foreach (var item in stats)
        {
            Console.WriteLine($"Автор: {item.Key}, Количество книг: {item.Value}");
        }
    }
   
}
