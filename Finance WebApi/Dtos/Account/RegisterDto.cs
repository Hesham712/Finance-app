using System.ComponentModel.DataAnnotations;

namespace Finance_WebApi.Dtos.Account
{
    public class RegisterDto
    {
        [Required] public string? Email { get; set; }
        [Required] public string? Password { get; set; }
        [Required] public string? UserName { get; set; }
    }
}
