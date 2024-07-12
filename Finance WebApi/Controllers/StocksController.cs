using Finance_WebApi.Data;
using Finance_WebApi.Data.Interfaces;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Helpers;
using Finance_WebApi.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;
using Microsoft.EntityFrameworkCore;

namespace Finance_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockService;

        public StocksController(ApplicationDbContext context, IStockRepository stockService)
        {
            _context = context;
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var allStockData = await _stockService.GetAllAsync(query);
            var stockDto = allStockData.Select(s => s.ToStockDto()).ToList();

            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var stockData = await _stockService.GetByIdAsync(id);

            if (stockData != null)
                return Ok(stockData.ToStockDto());

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockRequestDto stockDto)
        {
            if (!ModelState.IsValid) return BadRequest(stockDto);

            var stockModel = stockDto.ToStockFromCreatedDto();
            if (stockModel == null)
                return BadRequest();
            await _stockService.CreateAsync(stockModel);
            return Ok(stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] StockRequestDto updateDto, [FromRoute] int id)
        {
            var stock = await _stockService.UpdateAsync(id,updateDto);
            if (stock == null)
                return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var stockData = await _stockService.DeleteAsync(id);

            if (stockData != null)
            {
                return Ok(stockData.ToStockDto());
            }

            return NotFound();
        }
    }
}
