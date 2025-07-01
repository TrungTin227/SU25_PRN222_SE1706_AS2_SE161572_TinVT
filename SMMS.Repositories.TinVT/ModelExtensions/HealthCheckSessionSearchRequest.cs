namespace SMMS.Repositories.TinVT.ModelExtensions
{
    public class HealthCheckSessionSearchRequest : SearchRequest
    {
        public string? SessionCode { get; set; }
        public string? Title { get; set; }
        public string? StudentId { get; set; }
    }
}