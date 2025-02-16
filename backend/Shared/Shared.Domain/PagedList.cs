namespace Shared.Domain;

public class PagedList<T>(List<T> items, int page, int pageSize, int totalCount)
{
    public List<T> Items { get; } = items;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    public int TotalCount { get; } = totalCount;

    public int TotalPages => TotalCount / PageSize + (TotalCount % PageSize > 0 ? 1 : 0);
    
    public bool HasNextPage => Page * PageSize < TotalCount;

    public bool HasPreviousPage => Page > 1;
    
    public PagedList<TResult> MapItems<TResult>(Func<T, TResult> mapFunction)
        => new PagedList<TResult>(Items.Select(mapFunction).ToList(), Page, PageSize, TotalCount);
}