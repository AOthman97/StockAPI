using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title must be 2 chars at least!")
            , MaxLength(150, ErrorMessage = "Title must be 150 chars at most!")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(2, ErrorMessage = "Content must be 2 chars at least!")
            , MaxLength(150, ErrorMessage = "Content must be 150 chars at most!")]
        public string Content { get; set; } = string.Empty;
        public required int StockId { get; set; }
    }
}