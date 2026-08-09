using Application.Features.Admin.Commands.PromoteUserToAdmin;
using FluentValidation.TestHelper;

namespace UnitTests.Validators;

[TestFixture]
public sealed class PromoteUserToAdminValidatorTests
{
    private readonly PromoteUserToAdminValidator _validator = new();

    [Test]
    public void Validate_ConEmailValido_NoTieneErrores()
    {
        var result = _validator.TestValidate(new PromoteUserToAdminCommand("admin@test.com"));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_ConEmailVacio_TieneError()
    {
        var result = _validator.TestValidate(new PromoteUserToAdminCommand(""));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public void Validate_ConEmailInvalido_TieneError()
    {
        var result = _validator.TestValidate(new PromoteUserToAdminCommand("no-es-email"));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
