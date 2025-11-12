using AttractionReviewAPI.DTO;
using AttractionReviewAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AttractionReviewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private readonly IAttractionService _attractionService;

        public AttractionsController(IAttractionService attractionService)
        {
            _attractionService = attractionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AttractionDTO>> GetAll()
        {
            var attractions = _attractionService.GetAllAttractions();
            return Ok(attractions);
        }

        [HttpGet("{id}")]
        public ActionResult<AttractionDTO> GetById(int id)
        {
            var attraction = _attractionService.GetByIdAttraction(id);
            if (attraction == null)
                return NotFound(new { Success = false, ErrorMessage = $"Достопримечательность с ID {id} не найдена" });
            
            return Ok(attraction);
        }

        [HttpPost]
        public ActionResult<AttractionDTO> Create([FromBody] CreateAttractionDTO createAttractionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var attraction = _attractionService.CreateAttraction(createAttractionDTO);
            return CreatedAtAction(nameof(GetById), new { id = attraction.Id }, attraction);
        }

        [HttpPut("{id}")]
        public ActionResult<AttractionDTO> Update(int id, [FromBody] CreateAttractionDTO updateAttractionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _attractionService.UpdateAttraction(id, updateAttractionDTO);
            if (updated == null)
                return NotFound(new { Success = false, ErrorMessage = $"Достопримечательность с ID {id} не найдена" });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _attractionService.DeleteAttraction(id);
            if (!result)
                return NotFound(new { Success = false, ErrorMessage = $"Достопримечательность с ID {id} не найдена" });
            return NoContent();
        }
    }
}
