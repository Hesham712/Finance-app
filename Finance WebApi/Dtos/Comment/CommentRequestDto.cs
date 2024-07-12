using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_WebApi.Dtos.Comment
{
    public class CommentRequestDto
    {
        [Required]
        [MinLength(5,ErrorMessage ="Title must be 5 Character")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 Character")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 Character")]
        [MaxLength(280, ErrorMessage = "Content cannot be over 280 Character")]
        public string Content { get; set; } = string.Empty;
    }
}
