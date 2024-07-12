using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Finance_WebApi.Data;
using Finance_WebApi.Data.Interfaces;
using Finance_WebApi.Dtos.Comment;
using Finance_WebApi.Dtos.Stock;
using Finance_WebApi.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;
using Microsoft.EntityFrameworkCore;

namespace Finance_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IFMPService _fMPService;
        private readonly UserManager<AppUser> _userManager;
        public CommentsController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fMPService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fMPService = fMPService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync(queryObject);

            var commentDto = comments.Select(s => s.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{symbol}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, CommentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(requestDto);

            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                stock = await _fMPService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                    return BadRequest("stock does not exist");
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);

            var commentModel = requestDto.ToCommentFromCreate(stock.Id);
            commentModel.AppUserId = appUser.Id;

            await _commentRepo.CreateAsync(commentModel);

            return Ok(commentModel.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, CommentRequestDto updateDto)
        {
            var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());

            if (comment == null) return NotFound("Comment not found");

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var commentModel = await _commentRepo.DeleteAsync(id);
            if (commentModel == null) return NotFound("Comment does not exist");
            return Ok(commentModel);
        }


    }
}
