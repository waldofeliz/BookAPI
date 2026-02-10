namespace Domain.Entities;

public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? Descripcion { get; set; } = String.Empty;
    public DateTime FechaPublicacion { get; set; } = DateTime.MinValue;
}