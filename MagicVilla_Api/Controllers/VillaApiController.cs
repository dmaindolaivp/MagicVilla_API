using MagicVilla_Api.Data;
using MagicVilla_Api.Models;
using MagicVilla_Api.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Api.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]


    public class VillaApiController : ControllerBase
    {

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }




        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVillaById(int id)
        { 
            if (id <= 0) {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            return villa == null ? NotFound() : Ok(villa);
        }




        [HttpPost]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto) {
            if (villaDto == null) 
            {
                return BadRequest(villaDto);
            }

            if (villaDto.Id <= 0) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // if an object with the same id exists.
            if (VillaStore.villaList.FirstOrDefault(x => x.Id == villaDto.Id) != null) 
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);


            return Ok(villaDto);
                
        }
    }
}
