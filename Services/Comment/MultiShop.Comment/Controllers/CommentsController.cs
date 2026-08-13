using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Context;
using MultiShop.Comment.DTOs;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _commentContext;
        public CommentsController(CommentContext context)
        {
            _commentContext = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCommentDto createCommentDto, CancellationToken cancellationToken)
        {
            var comment = new UserComment
            {
                ProductId = createCommentDto.ProductId,
                NameSurname = createCommentDto.NameSurname,
                ImageUrl = createCommentDto.ImageUrl,
                Email = createCommentDto.Email,
                CommentDetail = createCommentDto.CommentDetail,
                Rating = createCommentDto.Rating,
                CreatedDate = DateTime.UtcNow,
                Status = false
            };

            await _commentContext.UserComments.AddAsync(comment, cancellationToken);
            await _commentContext.SaveChangesAsync(cancellationToken);
            return Created($"/api/comments/{comment.UserCommentId}", new { comment.UserCommentId });
        }

        [HttpGet("by-product/{productId}")]
        public async Task<ActionResult<IReadOnlyList<ResultCommentDto>>> GetByProductId(string productId, CancellationToken cancellationToken)
        {
            var results = await _commentContext.UserComments.Where(x => x.ProductId == productId && x.Status == true)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .Select(x => new ResultCommentDto
                                                            {
                                                                UserCommentId = x.UserCommentId,
                                                                ProductId = x.ProductId,
                                                                NameSurname = x.NameSurname,
                                                                ImageUrl = x.ImageUrl,
                                                                CommentDetail = x.CommentDetail,
                                                                Rating = x.Rating,
                                                                CreatedDate = x.CreatedDate
                                                            })
                                                            .ToListAsync(cancellationToken);
            return Ok(results);
        }
    }
}
