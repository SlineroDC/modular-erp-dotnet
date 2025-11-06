namespace ERP.Core.Models;

public class ResponsePage<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
