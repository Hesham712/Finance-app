using api.Models;
using Finance_WebApi.Dtos.Comment;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Models;

namespace Finance_WebApi.Data.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser,string symbol);


    }
}
