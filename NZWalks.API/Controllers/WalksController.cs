using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        public WalksController(IWalkRepository walkRepository)
        {
            this.walkRepository = walkRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomain = await walkRepository.GetAllAsync();

            var walksDTO = walksDomain.Select(item => new WalkDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                LengthInKm = item.LengthInKm,
                WalkImageUrl = item.WalkImageUrl,
                Difficulty = item.Difficulty,
                Region = item.Region
            });

            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = new WalkDTO
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Description = walkDomain.Description,
                LengthInKm = walkDomain.LengthInKm,
                WalkImageUrl = walkDomain.WalkImageUrl,
                Difficulty = walkDomain.Difficulty,
                Region = walkDomain.Region
            };

            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkDTO addWalkDTO)
        {
            var walkDomain = new Walk
            {
                Name = addWalkDTO.Name,
                Description = addWalkDTO.Description,
                LengthInKm = addWalkDTO.LengthInKm,
                WalkImageUrl = addWalkDTO.WalkImageUrl,
                DifficultyId = addWalkDTO.DifficultyId,
                RegionId = addWalkDTO.RegionId
            };

            walkDomain = await walkRepository.CreateAsync(walkDomain);

            var walkDTO = new WalkDTO
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Description = walkDomain.Description,
                LengthInKm = walkDomain.LengthInKm,
                WalkImageUrl = walkDomain.WalkImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var walkDomain = new Walk
            {
                Name = updateWalkDTO.Name,
                Description = updateWalkDTO.Description,
                LengthInKm = updateWalkDTO.LengthInKm,
                WalkImageUrl = updateWalkDTO.WalkImageUrl,
                DifficultyId = updateWalkDTO.DifficultyId,
                RegionId = updateWalkDTO.RegionId
            };

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = new WalkDTO
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Description = walkDomain.Description,
                LengthInKm = walkDomain.LengthInKm,
                WalkImageUrl = walkDomain.WalkImageUrl,
                Difficulty = walkDomain.Difficulty,
                Region = walkDomain.Region
            };

            return Ok(walkDTO);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = new WalkDTO
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Description = walkDomain.Description,
                LengthInKm = walkDomain.LengthInKm,
                WalkImageUrl = walkDomain.WalkImageUrl,
                Difficulty = walkDomain.Difficulty,
                Region = walkDomain.Region
            };

            return Ok(walkDTO);
        }
    }
}
