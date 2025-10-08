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

    }
    public void RemoveBook(int ID)
    {

    }
    public IEnumerable<Book> PoiskNazvania(string title)
    {

    }
    public IEnumerable<Book> PoickAutora(string author)
    {

    }
    public IEnumerable<Book> PoiskGenr(Genr genr)
    {

    }
    public IEnumerable<Book> SortNazv()
    {

    }
    public IEnumerable<Book> SortYear()
    {

    }
    public Book Dorogaya()
    {

    }
    public Book Dehevaa()
    {

    }
    public Dictionary<string, int> Static()
    {

    }
    public void initial()
    {

    }
}