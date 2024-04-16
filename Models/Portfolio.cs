using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    // This class denotes the many to many relationship between the Stock and AppUser, Where one AppUser can have multiple
    // stocks and vice versa
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}