namespace AbidiCompanySenario.ViewModels.Common
{
    public class PaginationMetadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
