namespace Api.Contracts.Admin;

public sealed class AssignUserRoleRequest
{
    public required string Email { get; init; }

    public required string Role { get; init; }
}
