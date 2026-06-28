namespace Domain.Entities;

public sealed class Editora
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nombre { get; private set; } = default!;
    public string? Descripcion { get; private set; }
    public string? Direccion { get; private set; }
    public string? Pais { get; private set; }
    public string? Website { get; private set; }
    public string? Telefono { get; private set; }
    public bool Estado { get; private set; }
    public string CreadoPor { get; private set; } = default!;
    public DateTime CreadoEn { get; private set; }
    public string? ModificadoPor { get; private set; }
    public DateTime ModificadoEn { get; private set; }
    public int Version { get; private set; }

    private Editora() { }

    public Editora(string nombre, string? descripcion, string? direccion, string? pais, string? website, string? telefono, string creadoPor)
    {
        SetNombre(nombre);
        Descripcion = descripcion;
        Direccion = direccion;
        SetPais(pais);
        SetWebsite(website);
        SetTelefono(telefono);
        SetCreadoPor(creadoPor);
        Pais = pais;
        Website = website;
        Telefono = telefono;
        CreadoEn = DateTime.UtcNow;
        Estado = true;
        Version = 1;
    }

    public void Update(string nombre, string? descripcion, string? direccion, string? pais, string? website, string? telefono, string? modificadoPor, bool estado)
    {
        SetNombre(nombre);
        Descripcion = descripcion;
        Direccion = direccion;
        SetPais(pais);
        SetWebsite(website);
        SetTelefono(telefono);
        Pais = pais;
        Website = website;
        Telefono = telefono;
        ModificadoEn = DateTime.UtcNow;
        ModificadoPor = modificadoPor;
        Estado = estado;
        Version++;
    }

    private void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
        if (nombre.Length > 50) throw new ArgumentException("El nombre no puede exceder 50 caracteres.");
        Nombre = nombre.Trim();
    }

    private void SetCreadoPor(string creadoPor)
    {
        if (string.IsNullOrWhiteSpace(creadoPor)) throw new ArgumentException("CreadoPor es requerido.");
        if (creadoPor.Length > 100) throw new ArgumentException("CreadoPor no puede exceder 100 caracteres.");
        CreadoPor = creadoPor.Trim();
    }

    private static void SetPais(string? pais)
    {
        if (!string.IsNullOrWhiteSpace(pais) && pais.Length > 50)
            throw new ArgumentException("El país no puede exceder 50 caracteres.");
    }

    private static void SetTelefono(string? telefono)
    {
        if (!string.IsNullOrWhiteSpace(telefono) && telefono.Length > 20)
            throw new ArgumentException("El teléfono no puede exceder 20 caracteres.");
    }

    private static void SetWebsite(string? website)
    {
        if (!string.IsNullOrWhiteSpace(website))
        {
            if (website.Length > 500) throw new ArgumentException("El sitio web no puede exceder 500 caracteres.");
            if (!Uri.IsWellFormedUriString(website, UriKind.Absolute))
                throw new ArgumentException("La URL del sitio web no es válida.");
        }
    }
}