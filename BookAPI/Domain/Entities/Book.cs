namespace Domain.Entities;

public class Book
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string Title { get; private set; } = default!;
    public string Isbn { get; private set; } = default!;
    public DateTime PublishedOn { get; private set; }
    public string? Description { get; private set; }
    
    private Book() {} //EF Core

    public Book(string title, string isbn, DateTime publishedOn, string? description)
    {
        SetTitle(title);
        SetIsbn(isbn);
        
        PublishedOn = publishedOn;
        Description = description;
    }
    
    public void Update(string title, string isbn, DateTime publishedOn, string? description)
    {
        SetTitle(title);
        SetIsbn(isbn);

        PublishedOn = publishedOn;
        Description = description;
    }
    
    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        if (title.Length > 200) throw new ArgumentException("El title debe tener entre 200 caracteres.");
        Title = title.Trim();
    }

    private void SetIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN es requerido.");
        isbn = isbn.Trim();

        // Sencillo: permite ISBN-10/13 con guiones. Ajusta si quieres más estricto.
        if (isbn.Length < 10 || isbn.Length > 17) throw new ArgumentException("La longitud del ISBN es inválida.");
        Isbn = isbn;
    }

}