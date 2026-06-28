using Domain.Entities;

namespace UnitTests.Domain;

[TestFixture]
public sealed class LibroTests
{
    [Test]
    public void Constructor_AssignsSubTituloAndAuditFields()
    {
        var libro = new Libro(
            "Clean Code",
            "9780132350884",
            new DateTime(2008, 8, 1),
            "Descripción",
            "test@example.com",
            "en",
            464,
            "1ra",
            null,
            "A Handbook");

        Assert.That(libro.SubTitulo, Is.EqualTo("A Handbook"));
        Assert.That(libro.CreadoPor, Is.EqualTo("test@example.com"));
        Assert.That(libro.CreadoEn, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(2)));
        Assert.That(libro.Estado, Is.True);
        Assert.That(libro.Version, Is.EqualTo(1));
    }

    [Test]
    public void Update_UpdatesSubTituloAndIncrementsVersion()
    {
        var libro = new Libro(
            "Título",
            "9780132350884",
            null,
            null,
            "creator",
            null,
            100,
            null,
            null);

        libro.Update(
            "Nuevo título",
            "9780132350884",
            null,
            "Nueva descripción",
            "modifier",
            false,
            null,
            200,
            "2da",
            null,
            "Nuevo subtítulo");

        Assert.That(libro.Titulo, Is.EqualTo("Nuevo título"));
        Assert.That(libro.SubTitulo, Is.EqualTo("Nuevo subtítulo"));
        Assert.That(libro.ModificadoPor, Is.EqualTo("modifier"));
        Assert.That(libro.Version, Is.EqualTo(2));
        Assert.That(libro.Estado, Is.False);
    }

    [Test]
    public void Constructor_ThrowsWhenTituloIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new Libro("", "9780132350884", null, null, "creator", null, 1, null, null));
    }
}
