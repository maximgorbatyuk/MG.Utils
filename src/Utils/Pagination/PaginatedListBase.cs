namespace Utils.Pagination
{
    public abstract record PaginatedListBase
    {
        public int CurrentPage { get; init; }

        public int PageSize { get; init; }

        public int TotalItems { get; init; }
    }
}
