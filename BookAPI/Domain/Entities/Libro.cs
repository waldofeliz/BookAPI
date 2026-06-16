namespace Domain.Entities;

public sealed class Libro
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string Titulo { get; private set; } = default!;
    public string Isbn { get; private set; } = default!;
    public DateTime PublicadoEn { get; private set; }
    public string? Descripcion { get; private set; }
    public bool Estado { get; private set; } = default;
    public string CreadoPor { get; private set; }
    public DateTime CreadoEn { get; private set; }
    public string? ModificadoPor { get; private set; }
    public DateTime ModificadoEn { get; private set; }
    public int Version { get; private set; }
    
    private Libro() {} //EF Core

    public Libro(string titulo, string isbn, DateTime publicadoEn, string? descripcion, string creadoPor)
    {
        SetTitulo(titulo);
        SetIsbn(isbn);
        
        PublicadoEn = publicadoEn;
        Descripcion = descripcion;
        CreadoPor = creadoPor;
        CreadoEn = new DateTime().Date;
        Estado = true;
        Version = 1;
    }
    
    public void Update(string titulo, string isbn, DateTime publicadoEn, string? descripcion, string? modificadoPor, bool estado)
    {
        SetTitulo(titulo);
        SetIsbn(isbn);

        PublicadoEn = publicadoEn;
        Descripcion = descripcion;
        ModificadoEn = new DateTime().Date;
        ModificadoPor = modificadoPor;
        Estado = estado;
        Version++;
    }
    
    private void SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new ArgumentException("El título es requerido..");
        if (titulo.Length > 200) throw new ArgumentException("El título debe tener entre 200 caracteres.");
        Titulo = titulo.Trim();
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