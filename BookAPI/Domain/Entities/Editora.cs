namespace Domain.Entities;

public sealed class Editora
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string Nombre { get; private set; } = default;
    public string? Descripcion { get; private set; }
    public string? Direccion { get; private set; }
    public string? Pais { get; private set; }
    public string? Website { get; private set; }
    public string? Telefono { get; private set; }
    public bool Estado { get; private set; } = default;
    public string CreadoPor { get; private set; }
    public DateTime CreadoEn { get; private set; }
    public string? ModificadoPor { get; private set; }
    public DateTime ModificadoEn { get; private set; }
    public int Version { get; private set; }

    private Editora() {} //EF Core

    public Editora(string nombre, string? descripcion, string? direccion, string? pais, string? website, string? telefono, string creadoPor)
    {
        SetNombre(nombre);
        Descripcion = descripcion;
        Direccion = direccion;
        Pais = pais;
        Website = website;
        Telefono = telefono;        
        CreadoEn = new DateTime().Date;
        CreadoPor = creadoPor;
        Estado = true;
        Version = 1;
    }

    public void Update(string nombre, string? descripcion, string? direccion, string? pais, string? website, string? telefono, string? modificadoPor, bool estado)
    {
        SetNombre(nombre);
        Direccion = direccion;
        Telefono = telefono;
        Pais = pais;
        Website = website;
        Descripcion = descripcion;
        ModificadoEn = new DateTime().Date;
        ModificadoPor = modificadoPor;
        Estado = estado;
        Version++;
    }

    private void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido..");
        if (nombre.Length > 50) throw new ArgumentException("El nombre debe tener entre 50 caracteres.");
        Nombre = nombre.Trim();
    }
}