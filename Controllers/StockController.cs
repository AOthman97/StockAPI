using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            // We need to use the .Select BECAUSE we're iterating through a list of items, If we were to
            // have a single stock we could've directly used the .ToStockDto without the .Select
            var stocks = _context.Stocks.ToList()
                        .Select(s => s.ToStockDto());

            if(stocks == null)
            {
                return NotFound();
            }

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) 
        {
            var stock = _context.Stocks.Find(id);

            if(stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto StockDto)
        {
            // Receive a StockDto as a param from body and revert it to a Stock model
            var StockModel = StockDto.ToStockFromCreateDto();
            _context.Stocks.Add(StockModel);
            _context.SaveChanges();
            // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
            // also convert the model to a DTO
            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
        }
    }
}