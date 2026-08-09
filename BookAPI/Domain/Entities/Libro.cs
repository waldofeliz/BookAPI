namespace Domain.Entities;

public sealed class Libro
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Titulo { get; private set; } = default!;
    public string? SubTitulo { get; private set; }
    public string Isbn { get; private set; } = default!;
    public DateTime? PublicadoEn { get; private set; }
    public string? Descripcion { get; private set; }
    public string? Lenguaje { get; private set; }
    public int Paginas { get; private set; }
    public string? Edicion { get; private set; }
    public string? CoverImageUrl { get; private set; }
    public Guid? EditoraId { get; private set; }
    public Editora? Editora { get; private set; }
    public ICollection<LibroAutor> LibroAutores { get; private set; } = [];
    public bool Estado { get; private set; }
    public string CreadoPor { get; private set; } = default!;
    public DateTime CreadoEn { get; private set; }
    public string? ModificadoPor { get; private set; }
    public DateTime ModificadoEn { get; private set; }
    public int Version { get; private set; }

    private Libro() { }

    public Libro(string titulo, string isbn, DateTime? publicadoEn, string? descripcion, string creadoPor, string? lenguaje,
        int paginas, string? edicion, string? coverImageUrl, string? subTitulo = null, Guid? editoraId = null)
    {
        SetTitulo(titulo);
        SetIsbn(isbn);
        SetCoverImageUrl(coverImageUrl);
        SetSubTitulo(subTitulo);

        PublicadoEn = publicadoEn;
        Descripcion = descripcion;
        Lenguaje = lenguaje;
        Paginas = paginas;
        Edicion = edicion;
        EditoraId = editoraId;
        CreadoPor = creadoPor;
        CreadoEn = DateTime.UtcNow;
        ModificadoEn = DateTime.UtcNow;
        Estado = true;
        Version = 1;
    }

    public void Update(string titulo, string isbn, DateTime? publicadoEn, string? descripcion, string? modificadoPor, bool estado,
        string? lenguaje, int paginas, string? edicion, string? coverImageUrl, string? subTitulo = null, Guid? editoraId = null)
    {
        SetTitulo(titulo);
        SetIsbn(isbn);
        SetCoverImageUrl(coverImageUrl);
        SetSubTitulo(subTitulo);

        PublicadoEn = publicadoEn;
        Descripcion = descripcion;
        Lenguaje = lenguaje;
        Paginas = paginas;
        Edicion = edicion;
        EditoraId = editoraId;
        ModificadoEn = DateTime.UtcNow;
        ModificadoPor = modificadoPor;
        Estado = estado;
        Version++;
    }

    public void AssignEditora(Guid? editoraId) => EditoraId = editoraId;

    public void SyncAutores(IReadOnlyList<Guid> autorIds)
    {
        var distinctIds = autorIds.Distinct().ToList();

        var toRemove = LibroAutores.Where(la => !distinctIds.Contains(la.AutorId)).ToList();
        foreach (var item in toRemove)
            LibroAutores.Remove(item);

        for (var i = 0; i < distinctIds.Count; i++)
        {
            var autorId = distinctIds[i];
            var orden = i + 1;
            var existing = LibroAutores.FirstOrDefault(la => la.AutorId == autorId);
            if (existing is null)
                LibroAutores.Add(new LibroAutor(Id, autorId, orden));
            else
                existing.SetOrden(orden);
        }
    }

    private void SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new ArgumentException("El título es requerido..");
        if (titulo.Length > 250) throw new ArgumentException("El título no puede exceder 250 caracteres.");
        Titulo = titulo.Trim();
    }

    private void SetIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN es requerido.");
        isbn = isbn.Trim();

        if (isbn.Length < 10 || isbn.Length > 17) throw new ArgumentException("La longitud del ISBN es inválida.");
        Isbn = isbn;
    }

    private void SetCoverImageUrl(string? coverImageUrl)
    {
        if (!string.IsNullOrWhiteSpace(coverImageUrl) && !Uri.IsWellFormedUriString(coverImageUrl, UriKind.Absolute))
            throw new ArgumentException("La URL de la imagen de portada no es válida.");
        CoverImageUrl = coverImageUrl;
    }

    private void SetSubTitulo(string? subTitulo)
    {
        if (!string.IsNullOrWhiteSpace(subTitulo) && subTitulo.Length > 250)
            throw new ArgumentException("El subtítulo no puede exceder 250 caracteres.");
        SubTitulo = string.IsNullOrWhiteSpace(subTitulo) ? null : subTitulo.Trim();
    }
}
