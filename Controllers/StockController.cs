using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController(ApplicationDBContext context) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;

        // Advanced feature, New null checking without actually checking for nulls each time in code
        //public class NullStock : Stock
        //{
        //    public NullStock() 
        //    {
        //        return;
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // We need to use the .Select BECAUSE we're iterating through a list of items, If we were to
            // have a single stock we could've directly used the .ToStockDto without the .Select
            var stocks = await _context.Stocks.ToListAsync();

            if (stocks == null || stocks.Count == 0)
            {
                return NotFound();
            }

            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            var stock = await _context.Stocks.FindAsync(id);

            if(stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto AddDto)
        {
            // Receive a StockDto as a param from body and revert it to a Stock model
            var StockModel = AddDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(StockModel);
            await _context.SaveChangesAsync();
            // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
            // also convert the model to a DTO
            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
        {
            var StockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(StockModel == null)
            {
                return NotFound();
            }

            StockModel.Purchase = UpdateDto.Purchase;
            StockModel.Symbol = UpdateDto.Symbol;
            StockModel.MarketCap = UpdateDto.MarketCap;
            StockModel.CompanyName = UpdateDto.CompanyName;
            StockModel.LastDiv = UpdateDto.LastDiv;
            StockModel.Industry = UpdateDto.Industry;

            //_context.Stocks.Update(StockModel);
            await _context.SaveChangesAsync();
            // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
            // also convert the model to a DTO
            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(y => y.Id == id);

            if (stockModel == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}