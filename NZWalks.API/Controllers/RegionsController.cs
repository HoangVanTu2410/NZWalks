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

        public RegionsController(IRegionRepository regionRepository) 
        {
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            var regionsDTO = regionsDomain.Select(item => new RegionDTO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                RegionImageUrl = item.RegionImageUrl,
            });

            return Ok(regionsDTO);
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

            var regionDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            var region = new Region
            {
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImageUrl = addRegionDTO.RegionImageUrl
            };

            region = await regionRepository.CreateAsync(region);

            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
            var region = new Region
            {
                Code = updateRegionDTO.Code,
                Name = updateRegionDTO.Name,
                RegionImageUrl = updateRegionDTO.RegionImageUrl
            };

            region = await regionRepository.UpdateAsync(id, region);

            if (region == null) {
                return NotFound();
            }

            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDTO);
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

            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDTO);
        }
    }
}
