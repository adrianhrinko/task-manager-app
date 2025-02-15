namespace Shared;

public record Query(
    string? Filter,
    Order? Order,
    string? OrderBy,
    int PageNo,
    int PageSize);