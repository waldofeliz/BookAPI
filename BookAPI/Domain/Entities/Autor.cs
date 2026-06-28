namespace Domain.Entities;

public sealed class Autor
{
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string Nombre { get; private set; } = default;
    public string Apellido { get; private set; } = default;
    public string? Biografia { get; private set; }
    public DateTime Cumpleanio { get; private set; } = default;
    public string? Nacionalidad { get; private set; }
    public bool Estado { get; private set; } = default;
    public string CreadoPor { get; private set; }
    public DateTime CreadoEn { get; private set; }
    public string? ModificadoPor { get; private set; }
    public DateTime ModificadoEn { get; private set; }
    public int Version { get; private set; }
    
    private Autor() {} //EF Core

    public Autor(string nombre, string apellido, string? biografia,  DateTime cumpleanio, string creadoPor, string? nacionalidad)
    {
        SetNombre(nombre);
        SetApellido(apellido);
        
        Biografia = biografia;
        Cumpleanio = cumpleanio;
        CreadoEn = new DateTime().Date;
        CreadoPor = creadoPor;
        Estado = true;
        Version = 1;
        Nacionalidad = nacionalidad;
    }
    
    public void Update(string nombre, string apellido, string? biografia,  DateTime cumpleanio, string? modificadoPor, bool estado, string? nacionalidad)
    {
        SetNombre(nombre);
        SetApellido(apellido);
        
        Biografia = biografia;
        Cumpleanio = cumpleanio;
        ModificadoEn = new DateTime().Date;
        ModificadoPor = modificadoPor;
        Estado = estado;
        Nacionalidad = nacionalidad;
        Version++;
    }
    
    private void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido..");
        if (nombre.Length > 50) throw new ArgumentException("El nombre debe tener entre 50 caracteres.");
        Nombre = nombre.Trim();
    }
    
    private void SetApellido(string apellido)
    {
        if (string.IsNullOrWhiteSpace(apellido)) throw new ArgumentException("El apellido es requerido..");
        if (apellido.Length > 50) throw new ArgumentException("El apellido debe tener entre 50 caracteres.");
        Apellido = apellido.Trim();
    }
    
    
}