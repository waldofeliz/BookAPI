namespace Domain.Entities;

public sealed class Libro
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string Titulo { get; private set; } = default!;
    public string? SubTitulo { get; private set; }
    public string Isbn { get; private set; } = default!;
    public DateTime? PublicadoEn { get; private set; }
    public string? Descripcion { get; private set; }
    public string? Lenguaje { get; private set; }
    public int Paginas { get; private set; }
    public string? Edicion { get; private set; }
    public string? CoverImageUrl { get; private set; }
    public bool Estado { get; private set; } = default;
    public string CreadoPor { get; private set; }
    public DateTime CreadoEn { get; private set; }
    public string? ModificadoPor { get; private set; }
    public DateTime ModificadoEn { get; private set; }
    public int Version { get; private set; }
    
    private Libro() {} //EF Core

    public Libro(string titulo, string isbn, DateTime? publicadoEn, string? descripcion, string creadoPor, string? lenguaje, 
        int paginas, string? edicion, string? coverImageUrl)
    {
        SetTitulo(titulo);
        SetIsbn(isbn);
        SetCoverImageUrl(coverImageUrl);
        
        PublicadoEn = publicadoEn;
        Descripcion = descripcion;
        Lenguaje = lenguaje;
        Paginas = paginas;
        Edicion = edicion;
        CreadoPor = creadoPor;
        CreadoEn = new DateTime().Date;
        Estado = true;
        Version = 1;
    }
    
    public void Update(string titulo, string isbn, DateTime? publicadoEn, string? descripcion, string? modificadoPor, bool estado, 
        string? lenguaje, int paginas, string? edicion, string? coverImageUrl)
    {
        SetTitulo(titulo);
        SetIsbn(isbn);
        SetCoverImageUrl(coverImageUrl);

        PublicadoEn = publicadoEn;
        Descripcion = descripcion;
        Lenguaje = lenguaje;
        Paginas = paginas;
        Edicion = edicion;
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

    private void SetCoverImageUrl(string? coverImageUrl)
    {
        if (!string.IsNullOrWhiteSpace(coverImageUrl) && !Uri.IsWellFormedUriString(coverImageUrl, UriKind.Absolute))
            throw new ArgumentException("La URL de la imagen de portada no es válida.");
        CoverImageUrl = coverImageUrl;
    }

}