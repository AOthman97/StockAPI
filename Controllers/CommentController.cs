using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepository,
        IStockRepository stockRepository,
        IMapper mapper) : ControllerBase
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();

            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }

            //var commentDto = comments.Select(c => c.ToCommentDto());
            var commentDto = _mapper.Map<List<CommentDto>>(comments);

            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentRequestDto AddDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.StockExists(stockId);

            if (stock == false)
            {
                return NotFound();
            }

            //var CommentModel = AddDto.ToCommentFromCreateDto(stockId);
            var CommentModel = _mapper.Map<Comment>(AddDto);
            await _commentRepository.CreateAsync(CommentModel);
            // After successful save, this will call the "GetById" method and pass-in the newly created Comment's Id and
            // also convert the model to a DTO
            return CreatedAtAction(nameof(GetById), new { id = CommentModel.Id }, _mapper.Map<CommentDto>(AddDto));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var CommentModel = await _commentRepository.DeleteAsync(id);

            if (CommentModel == null)
            {
                return NotFound();
            }

            else return NoContent();
        }
    }
}