using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController(ApplicationDBContext context, IStockRepository stockRepository) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;
        private readonly IStockRepository _stockRepository = stockRepository;

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
            var stocks = await _stockRepository.GetAllAsync();

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
            var stock = await _stockRepository.GetByIdAsync(id);

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
            await _stockRepository.CreateAsync(StockModel);
            // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
            // also convert the model to a DTO
            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
        {
            var StockModel = await _stockRepository.UpdateAsync(id, UpdateDto);

            if(StockModel != null)
            {
                // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
                // also convert the model to a DTO
                return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
            }
            else return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var StockModel = await _stockRepository.DeleteAsync(id);

            if (StockModel == null)
            {
                return NotFound();
            }

            else return NoContent();
        }
    }
}