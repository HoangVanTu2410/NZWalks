using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
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
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomain = await walkRepository.GetAllAsync();

            return Ok(mapper.Map<List<WalkDTO>>(walksDomain));
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

            return Ok(mapper.Map<WalkDTO>(walkDomain));
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDTO addWalkDTO)
        {
            var walkDomain = mapper.Map<Walk>(addWalkDTO);

            walkDomain = await walkRepository.CreateAsync(walkDomain);

            var walkDTO = mapper.Map<WalkDTO>(walkDomain);

            return CreatedAtAction(nameof(GetById), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var walkDomain = mapper.Map<Walk>(updateWalkDTO);

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walkDomain));
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

            return Ok(mapper.Map<WalkDTO>(walkDomain));
        }
    }
}
