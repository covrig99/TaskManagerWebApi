namespace TaskManagerWebApi.Models.NewFolder
{
    public class PagedResult<T>
    {
        public int TotalCount { get; }
        public int Offset { get; }
        public int Limit { get; }
        public List<T> Items { get; }

        public PagedResult(int totalCount, int offset, int limit, List<T> items)
        {
            TotalCount = totalCount;
            Offset = offset;
            Limit = limit;
            Items = items;
        }
    }
}
