using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        // General DTO for get and alike operations
        //public static CommentDto ToCommentDto(this Comment CommentModel)
        //{
        //    return new CommentDto
        //    {
        //        Id = CommentModel.Id,
        //        Title = CommentModel.Title,
        //        Content = CommentModel.Content,
        //        CreatedOn = CommentModel.CreatedOn,
        //        StockId = CommentModel.StockId
        //    };
        //}

        //// Revert the DTO to the Comment for the create Comment without the ID, Because EF won't allow to submit a DTO instead
        //// of a class for saving
        //public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto CommentRequestDto, int stockId)
        //{
        //    return new Comment
        //    {
        //        Title = CommentRequestDto.Title,
        //        Content = CommentRequestDto.Content,
        //        StockId = stockId
        //    };
        //}
    }
}