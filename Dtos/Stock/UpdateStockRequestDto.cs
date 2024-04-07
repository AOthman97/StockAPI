using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol must be 1 chars at least!")
            , MaxLength(3, ErrorMessage = "Symbol must be 3 chars at most!")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(4, ErrorMessage = "Symbol must be 4 chars at least!")
            , MaxLength(50, ErrorMessage = "Symbol must be 50 chars at most!")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 1000000)]
        public decimal LastDiv { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Industry must be 2 chars at least!")
            , MaxLength(50, ErrorMessage = "Industry must be 50 chars at most!")]
        public string Industry { get; set; } = string.Empty;
        [Range(1, 90000000)]
        public long MarketCap { get; set; }
    }
}