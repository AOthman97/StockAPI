using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository(ApplicationDBContext context) : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Portfolio> CreateAsync(Portfolio portfolioModel)
        {
            await _context.Portfolios.AddAsync(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(p => p.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    CompanyName = stock.Stock.CompanyName,
                    Industry = stock.Stock.Industry,
                    LastDiv = stock.Stock.LastDiv,
                    MarketCap = stock.Stock.MarketCap,
                    Purchase = stock.Stock.Purchase,
                    Symbol = stock.Stock.Symbol
                }).ToListAsync();
        }
    }
}