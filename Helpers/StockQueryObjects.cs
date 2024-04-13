namespace api.Helpers
{
    public class StockQueryObjects
    {
        public string? Sybmol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
    }
}