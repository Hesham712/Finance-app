using api.Extensions;
using api.Interfaces;
using api.Models;
using Finance_WebApi.Data.Interfaces;
using Finance_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finance_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFMPService _fMPService;


        public PortfolioController(IStockRepository stockRepository, UserManager<AppUser> userManager, IPortfolioRepository portfolioRepository, IFMPService fMPService)
        {
            _stockRepository = stockRepository;
            _userManager = userManager;
            _portfolioRepository = portfolioRepository;
            _fMPService = fMPService;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                stock = await _fMPService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                    return BadRequest("stock does not exist");
                else
                {
                    await _stockRepository.CreateAsync(stock);
                }
            }
            if (stock == null)
                return BadRequest("Stock not found");
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if (userPortfolio.Any(m => m.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("can not add same stock to portfolio");

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };
            await _portfolioRepository.CreateAsync(portfolioModel);

            if (portfolioModel == null)
                return BadRequest("could not create");

            return Ok(portfolioModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(p=>p.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count == 1)
            {
                await _portfolioRepository.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not found in your portfolio");
            }
            return Ok("Deleted successfully");
        }

    }
}
