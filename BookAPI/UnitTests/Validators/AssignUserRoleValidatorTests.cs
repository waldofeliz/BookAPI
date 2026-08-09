using Application.Abstractions.Security;
using Application.Features.Admin.Commands.AssignUserRole;
using FluentValidation.TestHelper;

namespace UnitTests.Validators;

[TestFixture]
public sealed class AssignUserRoleValidatorTests
{
    private readonly AssignUserRoleValidator _validator = new();

    [TestCase(AppRoles.Editor)]
    [TestCase(AppRoles.Reader)]
    public void Validate_ConRolDeCatalogoValido_NoTieneErrores(string role)
    {
        var result = _validator.TestValidate(new AssignUserRoleCommand("user@test.com", role));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_ConRolAdmin_TieneError()
    {
        var result = _validator.TestValidate(new AssignUserRoleCommand("user@test.com", AppRoles.Admin));
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Test]
    public void Validate_ConEmailVacio_TieneError()
    {
        var result = _validator.TestValidate(new AssignUserRoleCommand("", AppRoles.Reader));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
