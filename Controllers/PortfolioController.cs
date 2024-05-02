using api.Dtos.Stock;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, 
        IPortfolioRepository portfolioRepository) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly IPortfolioRepository _portfolioRepository = portfolioRepository;

        [HttpGet("GetUserPortfolio")]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null)
                return StatusCode(404, "No User Found!");

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            return Ok(userPortfolio);



        }

        [HttpPost("CreateUserPortfolio")]
        public async Task<IActionResult> CreateUserPortfolio(string symbol)
        {
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (appUser == null)
                return StatusCode(404, "No User Found!");
            if (stock == null)
                return StatusCode(404, "No Stock Found!");


            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if (userPortfolio.Any(u => u.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Cannot add same stock with user to Portfolio!");

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepository.CreateAsync(portfolioModel);

            if(portfolioModel == null)
                return StatusCode(500, "Create Failed!");
            else
                return Created();
        }
    }
}