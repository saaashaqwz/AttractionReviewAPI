using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AttractionReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReviewDTO>> GetAll()
        {
            var reviews = _reviewService.GetAllReviews();
            var reviewsDTO = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return Ok(reviewsDTO);
        }

        [HttpGet("{id}")]
        public ActionResult<ReviewDTO> GetById(int id)
        {
            var review = _reviewService.GetByIdReview(id);
            if (review == null) return NotFound($"Отзыв с ID {id} не найден");
            
            var reviewDTO = _mapper.Map<ReviewDTO>(review);
            return Ok(reviewDTO);
        }

        [HttpGet("attraction/{attractionId}")]
        public ActionResult<IEnumerable<ReviewDTO>> GetByAttraction(int attractionId)
        {
            try
            {
                var reviews = _reviewService.GetByAttraction(attractionId);
                
                if (!reviews.Any())
                {
                    return NotFound($"Отзывы для достопримечательности с ID {attractionId} не найдены");
                }
                
                var reviewsDTO = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
                return Ok(reviewsDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<ReviewDTO> Create([FromBody] CreateReviewDTO createReviewDTO)
        {
            try
            {
                var review = _reviewService.CreateReview(createReviewDTO);
                var reviewDTO = _mapper.Map<ReviewDTO>(review);
                return CreatedAtAction(nameof(GetById), new { id = reviewDTO.Id }, reviewDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ReviewDTO> Update(int id, [FromBody] UpdateReviewDTO updateReviewDTO)
        {
            try
            {
                if (updateReviewDTO == null)
                    return BadRequest("Данные для обновления не могут быть пустыми");

                var updatedReview = _reviewService.UpdateReview(id, updateReviewDTO);
                if (updatedReview == null)
                    return NotFound($"Отзыв с ID {id} не найден");

                var updatedReviewDto = _mapper.Map<ReviewDTO>(updatedReview);
                return Ok(updatedReviewDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _reviewService.DeleteReview(id);
            if (!result)
            {
                return NotFound($"Отзыв с ID {id} не найден");
            }
            return NoContent();
        }
    }
}
