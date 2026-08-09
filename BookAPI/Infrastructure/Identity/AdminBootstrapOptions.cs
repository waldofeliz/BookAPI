namespace Infrastructure.Identity;

public sealed class AdminBootstrapOptions
{
    public const string SectionName = "Admin";

    public string[] BootstrapEmails { get; init; } = [];
}
