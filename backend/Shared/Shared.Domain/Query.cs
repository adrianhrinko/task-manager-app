namespace Shared.Domain;

public record Query(
    string? Filter = null,
    Order? Order = null,
    string? OrderBy = null,
    int PageNo = 1,
    int PageSize = 10);