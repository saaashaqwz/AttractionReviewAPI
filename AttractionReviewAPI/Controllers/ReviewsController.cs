using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace AttractionReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ReviewDTO>> GetAll()
        {
            var reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ReviewDTO> GetById(int id)
        {
            var review = _reviewService.GetByIdReview(id);
            if (review == null)
                return NotFound(new { Success = false, ErrorMessage = $"Отзыв с ID {id} не найден" });
            return Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("attraction/{attractionId}")]
        public ActionResult<IEnumerable<ReviewDTO>> GetByAttraction(int attractionId)
        {
            var reviews = _reviewService.GetByAttraction(attractionId);
            if (reviews == null)
                return NotFound(new { Success = false, ErrorMessage = $"Достопримечательность с ID {attractionId} не найдена" });
            return Ok(reviews);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult<ReviewDTO> Create([FromBody] CreateReviewDTO createReviewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = _reviewService.CreateReview(createReviewDTO);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public ActionResult<ReviewDTO> Update(int id, [FromBody] UpdateReviewDTO updateReviewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _reviewService.UpdateReview(id, updateReviewDTO);
            if (updated == null)
                return NotFound(new { Success = false, ErrorMessage = $"Отзыв с ID {id} не найден" });
            return Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _reviewService.DeleteReview(id);
            if (!result)
                return NotFound(new { Success = false, ErrorMessage = $"Отзыв с ID {id} не найден" });
            return NoContent();
        }
    }
}
