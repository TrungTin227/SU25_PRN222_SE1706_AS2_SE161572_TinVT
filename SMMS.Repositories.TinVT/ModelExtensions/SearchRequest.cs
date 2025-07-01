namespace SMMS.Repositories.TinVT.ModelExtensions
{
    public class SearchRequest
    {
        public int? CurrentPage { get; set; } = 1;
        public int? PageSize { get; set; } = 2;
    }
}