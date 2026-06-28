using Application.Features.Libros.Commands.CreateLibro;

namespace UnitTests.Validators;

[TestFixture]
public sealed class CreateLibroValidatorTests
{
    private readonly CreateLibroValidator _validator = new();

    [Test]
    public void Validate_ReturnsError_When_TituloIsEmpty()
    {
        var command = new CreateLibroCommand("", "9780132350884", DateTime.UtcNow, null, "user", null, "en", 100, null, null, null, null);
        var result = _validator.Validate(command);
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == nameof(CreateLibroCommand.Titulo)), Is.True);
    }

    [Test]
    public void Validate_ReturnsError_When_PaginasIsZero()
    {
        var command = new CreateLibroCommand("Título", "9780132350884", DateTime.UtcNow, null, "user", null, "en", 0, null, null, null, null);
        var result = _validator.Validate(command);
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == nameof(CreateLibroCommand.Paginas)), Is.True);
    }

    [Test]
    public void Validate_Succeeds_When_CommandIsValid()
    {
        var command = new CreateLibroCommand(
            "Clean Architecture",
            "9780134494166",
            DateTime.UtcNow,
            "Descripción",
            "user@example.com",
            null,
            "en",
            350,
            "1ra",
            "Subtítulo",
            null,
            null);

        var result = _validator.Validate(command);
        Assert.That(result.IsValid, Is.True);
    }
}
