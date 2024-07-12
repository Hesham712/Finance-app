using Finance_WebApi.Data.Interfaces;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Helpers;
using Finance_WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance_WebApi.Data.Services
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            _context.SaveChanges();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockData = await _context.Stocks.FirstOrDefaultAsync(m => m.Id == id);
            if(stockData == null) return null;

            _context.Stocks.Remove(stockData);
            _context.SaveChanges();
            return stockData;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c=>c.Comments).ThenInclude(a=>a.AppUser).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(c=>c.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(c => c.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? stocks.OrderByDescending(c=>c.Symbol) : stocks.OrderBy(c=>c.Symbol);
                }
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c=>c.Comments).FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(c => c.Symbol.ToLower() == symbol.ToLower());
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(c=>c.Id==id);
        }

        public async Task<Stock?> UpdateAsync(int id, StockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(m => m.Id == id);
            if (existingStock == null)
                return null;

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;
            _context.SaveChanges();
            return existingStock;
        }
    }
}
