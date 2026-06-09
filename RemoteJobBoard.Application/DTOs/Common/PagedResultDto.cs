namespace RemoteJobBoard.Application.DTOs.Common;

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage => PageNumber * PageSize < TotalCount;
    public bool HasPreviousPage => PageNumber > 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public int? NextPage => HasNextPage ? PageNumber + 1 : null;
    public int? PreviousPage => HasPreviousPage ? PageNumber - 1 : null;
}