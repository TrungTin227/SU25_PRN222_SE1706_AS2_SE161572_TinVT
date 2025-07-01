using SMMS.Repositories.TinVT.ModelExtensions;

namespace SMMS.Repositories.TinVT.Extensions
{
    public static class ListExtensions
    {
        public static PaginationResult<List<T>> ToPagination<T>(this List<T> source, int currentPage = 1, int pageSize = 10) where T : class
        {
            // Validate input
            if (currentPage <= 0) currentPage = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalItems = source.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Đảm bảo currentPage không vượt quá totalPages
            if (currentPage > totalPages && totalPages > 0)
                currentPage = totalPages;

            var pagedItems = source
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<List<T>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = pagedItems
            };
        }

        public static PaginationResult<List<T>> ToPagination<T>(this List<T> source, SearchRequest searchRequest) where T : class
        {
            return source.ToPagination(
                searchRequest.CurrentPage.GetValueOrDefault(1),
                searchRequest.PageSize.GetValueOrDefault(10)
            );
        }
    }
}