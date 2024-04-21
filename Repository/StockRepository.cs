using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<List<Stock>> GetAllAsync(StockQueryObjects query)
        {
            //return await _context.Stocks.Include(c => c.Comments).ToListAsync();

            // By using the query object we can add additional logic or filtering for our sql statements based on the
            // query params that are received
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(c => c.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Sybmol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Sybmol));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            var SkipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(y => y.Id == id);

            if (stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockDto)
        {
            var StockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (StockModel == null)
            {
                return null;
            }

            StockModel.Purchase = updateStockDto.Purchase;
            StockModel.Symbol = updateStockDto.Symbol;
            StockModel.MarketCap = updateStockDto.MarketCap;
            StockModel.CompanyName = updateStockDto.CompanyName;
            StockModel.LastDiv = updateStockDto.LastDiv;
            StockModel.Industry = updateStockDto.Industry;

            await _context.SaveChangesAsync();
            return StockModel;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(i => i.Symbol == symbol);
        }
    }
}