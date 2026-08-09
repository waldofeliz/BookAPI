namespace Api.Contracts.Admin;

public sealed class PromoteUserRequest
{
    public required string Email { get; init; }
}
