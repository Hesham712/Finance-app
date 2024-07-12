using Finance_WebApi.Dtos.Comment;
using Finance_WebApi.Models;
using System.Runtime.CompilerServices;

namespace Finance_WebApi.Mapper
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment CommentModel)
        {
            return new CommentDto
            {
                Id = CommentModel.Id,
                Content = CommentModel.Content,
                CreatedOn = CommentModel.CreatedOn,
                StockId = CommentModel.StockId,
                CreatedBy = CommentModel.AppUser.UserName,
                Title = CommentModel.Title
            };
        }

        public static Comment ToCommentFromCreate(this CommentRequestDto CommentDto,int stockId)
        {
            return new Comment
            {
                Content = CommentDto.Content,
                StockId = stockId,
                Title = CommentDto.Title
            };
        }

        public static Comment ToCommentFromUpdate(this CommentRequestDto CommentDto)
        {
            return new Comment
            {
                Content = CommentDto.Content,
                Title = CommentDto.Title
            };
        }
    }
}
