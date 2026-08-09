namespace Application.Abstractions.Security;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Editor = "Editor";
    public const string Reader = "Reader";

    public static readonly IReadOnlyList<string> All = [Admin, Editor, Reader];

    public static readonly IReadOnlyList<string> AssignableCatalogRoles = [Editor, Reader];
}
