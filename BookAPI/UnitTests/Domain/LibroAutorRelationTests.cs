using Domain.Entities;

namespace UnitTests.Domain;

[TestFixture]
public sealed class LibroAutorRelationTests
{
    [Test]
    public void SyncAutores_AddsAndOrdersAuthors()
    {
        var autor1 = Guid.NewGuid();
        var autor2 = Guid.NewGuid();

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

        libro.SyncAutores([autor2, autor1]);

        Assert.That(libro.LibroAutores, Has.Count.EqualTo(2));
        Assert.That(libro.LibroAutores.First(la => la.AutorId == autor2).Orden, Is.EqualTo(1));
        Assert.That(libro.LibroAutores.First(la => la.AutorId == autor1).Orden, Is.EqualTo(2));
    }

    [Test]
    public void SyncAutores_ReplacesExistingAuthors()
    {
        var autor1 = Guid.NewGuid();
        var autor2 = Guid.NewGuid();
        var autor3 = Guid.NewGuid();

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

        libro.SyncAutores([autor1, autor2]);
        libro.SyncAutores([autor3]);

        Assert.That(libro.LibroAutores, Has.Count.EqualTo(1));
        Assert.That(libro.LibroAutores.Single().AutorId, Is.EqualTo(autor3));
    }

    [Test]
    public void Constructor_AssignsEditoraId()
    {
        var editoraId = Guid.NewGuid();

        var libro = new Libro(
            "Título",
            "9780132350884",
            null,
            null,
            "creator",
            null,
            100,
            null,
            null,
            editoraId: editoraId);

        Assert.That(libro.EditoraId, Is.EqualTo(editoraId));
    }
}
