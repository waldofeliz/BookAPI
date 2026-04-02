namespace Shared.Results;

public sealed record PageMeta(int Page, int PageSize, int TotalCount);

public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required PageMeta Meta { get; init; }
}