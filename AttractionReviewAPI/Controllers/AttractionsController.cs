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
        private readonly IMapper _mapper;

        public AttractionsController(IAttractionService attractionService, IMapper mapper)
        {
            _attractionService = attractionService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AttractionDTO>> GetAll()
        {
            var attractions = _attractionService.GetAllAttractions();
            var attractionDTOs = _mapper.Map<IEnumerable<AttractionDTO>>(attractions);
            return Ok(attractionDTOs);
        }

        [HttpGet("{id}")]
        public ActionResult<AttractionDTO> GetById(int id)
        {
            var attraction = _attractionService.GetByIdAttraction(id);
            if (attraction == null)
            {
                return NotFound($"Достопримечательность с ID {id} не найден");
            }
            var attractionDTO = _mapper.Map<AttractionDTO>(attraction);
            return Ok(attractionDTO);
        }

        [HttpPost]
        public ActionResult<AttractionDTO> Create([FromBody] CreateAttractionDTO createAttractionDTO)
        {
            try
            {
                var attraction = _attractionService.CreateAttraction(createAttractionDTO);
                var attractionDTO = _mapper.Map<AttractionDTO>(attraction);
                return CreatedAtAction(nameof(GetById), new { id = attractionDTO.Id }, attractionDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<AttractionDTO> Update(int id, [FromBody] CreateAttractionDTO updateAttractionDTO)
        {
            try
            {
                if (updateAttractionDTO == null)
                    return BadRequest("Данные для обновления не могут быть пустыми");

                var updatedAttraction = _attractionService.UpdateAttraction(id, updateAttractionDTO);
                if (updatedAttraction == null)
                    return NotFound($"Достопримечательность с ID {id} не найден");

                var updatedAttractionDto = _mapper.Map<ReviewDTO>(updatedAttraction);
                return Ok(updatedAttractionDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _attractionService.DeleteAttraction(id);
            if (!result)
            {
                return NotFound($"Достопримечательность с ID {id} не найден");
            }
            return NoContent();
        }
    }
}
