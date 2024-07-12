using api.Models;
using Finance_WebApi.Data.Interfaces;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance_WebApi.Data.Services
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;

        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(m => m.AppUserId == appUser.Id && m.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel == null) return null;
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(p=>p.AppUserId == user.Id)
                .Select(Stock =>new Stock
                {
                    Id = Stock.StockId,
                    Symbol = Stock.Stock.Symbol,
                    CompanyName = Stock.Stock.CompanyName,
                    Purchase = Stock.Stock.Purchase,
                    LastDiv = Stock.Stock.LastDiv,
                    Industry = Stock.Stock.Industry,
                    MarketCap = Stock.Stock.MarketCap
                }).ToListAsync();
        }
    }
}
