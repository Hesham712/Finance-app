using api.Models;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Helpers;
using Finance_WebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Finance_WebApi.Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken (AppUser user); 
    }
}
