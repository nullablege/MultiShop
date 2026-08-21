using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Authorization;
using MultiShop.Comment.Context;
using MultiShop.Comment.DTOs;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = CommentAuthorizationConstants.Policy)]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _commentContext;
        public CommentsController(CommentContext context)
        {
            _commentContext = context;
        }

        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [HttpGet("admin")]
        public async Task<ActionResult<IReadOnlyList<AdminCommentListDto>>> GetAdminComments(CancellationToken cancellationToken)
        {

            var comments = await _commentContext.UserComments.OrderByDescending(x => x.CreatedDate)
                                                             .Select(x => new AdminCommentListDto
                                                                {
                                                                    UserCommentId = x.UserCommentId,
                                                                    ProductId = x.ProductId,
                                                                    NameSurname = x.NameSurname,
                                                                    CommentDetail = x.CommentDetail,
                                                                    Rating = x.Rating,
                                                                    CreatedDate = x.CreatedDate,
                                                                    Status = x.Status
                                                                }).ToListAsync(cancellationToken);

            return Ok(comments);

        }

        [HttpGet("admin/statistics")]
        [Authorize(Policy = CommentAuthorizationConstants.ManagementPolicy)]
        public async Task<ActionResult<CommentStatisticsDto>> GetStatisticsAsync(
            CancellationToken cancellationToken)
        {
            var totalCount = await _commentContext.UserComments.CountAsync(cancellationToken);
            var approvedCount = await _commentContext.UserComments
                .CountAsync(comment => comment.Status, cancellationToken);
            var pendingCount = await _commentContext.UserComments
                .CountAsync(comment => !comment.Status, cancellationToken);

            return Ok(new CommentStatisticsDto
            {
                TotalCount = totalCount,
                ApprovedCount = approvedCount,
                PendingCount = pendingCount
            });
        }

        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<AdminCommentListDto>> GetAdminComment(
            int id,
            CancellationToken cancellationToken)
        {
            var comment = await _commentContext.UserComments
                .AsNoTracking()
                .Where(x => x.UserCommentId == id)
                .Select(x => new AdminCommentListDto
                {
                    UserCommentId = x.UserCommentId,
                    ProductId = x.ProductId,
                    NameSurname = x.NameSurname,
                    CommentDetail = x.CommentDetail,
                    Rating = x.Rating,
                    CreatedDate = x.CreatedDate,
                    Status = x.Status
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPut("admin/{id:int}/status")]
        public async Task<IActionResult> UpdateCommentStatus(
            int id,
            UpdateCommentStatusDto updateCommentStatusDto,
            CancellationToken cancellationToken)
        {
            var comment = await _commentContext.UserComments
                .SingleOrDefaultAsync(
                    x => x.UserCommentId == id,
                    cancellationToken);

            if (comment == null)
                return NotFound();

            comment.Status = updateCommentStatusDto.Status;
            await _commentContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("admin/{id:int}")]
        public async Task<IActionResult> DeleteAdminComment(
            int id,
            CancellationToken cancellationToken)
        {
            var comment = await _commentContext.UserComments
                .SingleOrDefaultAsync(
                    x => x.UserCommentId == id,
                    cancellationToken);

            if (comment == null)
                return NotFound();

            _commentContext.UserComments.Remove(comment);
            await _commentContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
}
}
