using api.Helpers;
using Finance_WebApi.Dtos.Comment;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Models;

namespace Finance_WebApi.Data.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int id, Comment commentModel);
        Task<Comment?> DeleteAsync(int id);
    }
}
