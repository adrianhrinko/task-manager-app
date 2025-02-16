namespace Shared.Domain;

public record Query(
    string Filter = "",
    string FilterBy = "",
    bool OrderDesc = false,
    string OrderBy = "",
    int PageNo = 1,
    int PageSize = 10);