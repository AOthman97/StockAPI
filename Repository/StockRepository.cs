using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        private readonly ApplicationDBContext _context = context;

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

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
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
    }
}