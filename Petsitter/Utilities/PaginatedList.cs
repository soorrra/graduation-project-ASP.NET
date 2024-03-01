namespace Petsitter
{

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        /// <summary>
        /// Creates pagination.
        /// </summary>
        /// <param name="items">List<T></param>
        /// <param name="count">int</param>
        /// <param name="pageIndex">int</param>
        /// <param name="pageSize">int</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        /// <summary>
        /// Checks if there is a previous page of items.
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        /// <summary>
        /// Checks if there is a next page of items.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /// <summary>
        /// Creates pagination.
        /// </summary>
        /// <param name="source">IQueryable<T></param>
        /// <param name="pageIndex">int</param>
        /// <param name="pageSize">int</param>
        /// <returns></returns>
        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex,
                        int pageSize)
        {
            var count = source.Count();
            var items =
                source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }

}
