using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) 
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            return Ok(mapper.Map<List<Region>>(regionsDomain));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var region = mapper.Map<Region>(addRegionDTO);

            region = await regionRepository.CreateAsync(region);

            var regionDTO = mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var region = mapper.Map<Region>(updateRegionDTO);

            region = await regionRepository.UpdateAsync(id, region);

            if (region == null) {
                return NotFound();
            }
            
            return Ok(mapper.Map<RegionDTO>(region));
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDTO>(region));
        }
    }
}
