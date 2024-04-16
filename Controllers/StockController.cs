using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    [Authorize]
    public class StockController(IStockRepository stockRepository, IMapper mapper) : ControllerBase
    {
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] StockQueryObjects query)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            // We need to use the .Select BECAUSE we're iterating through a list of items, If we were to
            // have a single stock we could've directly used the .ToStockDto without the .Select
            var stocks = await _stockRepository.GetAllAsync(query);

            if (stocks == null || stocks.Count == 0)
            {
                return NotFound();
            }

            //var config = new TypeAdapterConfig();

            //config.NewConfig<Stock, StockDto>()
            //    .Map(dest => dest.Comments, src => src.Comments.Adapt<List<CommentDto>>())
            //    .Map(dest => dest.lang, src => MapContext.Current.Parameters["lang"]);

            // Old with manual mapper with the "StockMappers" class
            //var stockDto = stocks.Select(s => s.ToStockDto());

            // First is dest, second is source
            // Currently throwing a null exception with the AddParameter option
            var stockDto = _mapper.Map<List<StockDto>>(stocks);
                //.BuildAdapter()
                //.AddParameters("lang", "ar")
                //.AdaptToType<List<StockDto>>();

            // Second with mapping inside this action method, Working properly
            //var destObject = stocks.BuildAdapter(config)
            //                        .AddParameters("lang", "fr")
            //                        .AdaptToType<List<StockDto>>();

            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            //return Ok(stock.ToStockDto());
            return Ok(_mapper.Map<StockDto>(stock));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto AddDto)
        {
            // Receive a StockDto as a param from body and revert it to a Stock model
            //var StockModel = AddDto.ToStockFromCreateDto();
            var StockModel = _mapper.Map<Stock>(AddDto);
            await _stockRepository.CreateAsync(StockModel);
            // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
            // also convert the model to a DTO
            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, _mapper.Map<StockDto>(AddDto));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateDto)
        {
            var StockModel = await _stockRepository.UpdateAsync(id, UpdateDto);

            if (StockModel != null)
            {
                // After successful save, this will call the "GetById" method and pass-in the newly created stock's Id and
                // also convert the model to a DTO
                //return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
                return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, _mapper.Map<StockDto>(UpdateDto));
            }
            else return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
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