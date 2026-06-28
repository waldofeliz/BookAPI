namespace Domain.Entities;

public sealed class LibroAutor
{
    public Guid LibroId { get; private set; }
    public Guid AutorId { get; private set; }
    public int Orden { get; private set; }
    public Libro Libro { get; private set; } = default!;
    public Autor Autor { get; private set; } = default!;

    private LibroAutor() { }

    public LibroAutor(Guid libroId, Guid autorId, int orden)
    {
        if (orden < 1) throw new ArgumentException("El orden debe ser mayor a cero.");
        LibroId = libroId;
        AutorId = autorId;
        Orden = orden;
    }

    internal void SetOrden(int orden) => Orden = orden;
}
