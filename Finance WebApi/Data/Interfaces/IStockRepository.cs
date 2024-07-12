using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Helpers;
using Finance_WebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Finance_WebApi.Data.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, StockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExists(int id);

    }
}
