using MagicVilla_Api.Data;
using MagicVilla_Api.Models;
using MagicVilla_Api.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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




        [HttpGet("{id}" , Name = "GetVillaById")]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto) {
            if (villaDto == null) 
            {
                return BadRequest(villaDto);
            }

            if (villaDto.Id <= 0) 
            {
                Console.WriteLine(villaDto.Id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // if an object with the same id exists.
            if (VillaStore.villaList.FirstOrDefault(x => x.Id == villaDto.Id) != null) 
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault()!.Id + 1;
            VillaStore.villaList.Add(villaDto);

            return CreatedAtRoute("GetVillaById" , new {id = villaDto.Id} , villaDto);
                
        }




        [HttpDelete("{id}" , Name = "DeleteVillaById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteVillaById(int id) 
        {
            if (id <= 0)
            {
                return BadRequest("ID CANNOT BE NEGATIVE OR ZERO");
            }
            if (VillaStore.villaList.FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            VillaStore.villaList.Remove(villa!);
            return NoContent();
        }




        [HttpPut("{id}" , Name = "UpdateVillaById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto) 
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be negative");
            }
            if (id != villaDto.Id || villaDto == null)
            {
                return BadRequest("ids dont match");
            }
            if (VillaStore.villaList.FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound();
            }
            var oldVilla = VillaStore.villaList.First(x => x.Id == id);
            int indexOfOldVilla = VillaStore.villaList.IndexOf(oldVilla);
            VillaStore.villaList[indexOfOldVilla] = villaDto;

            return NoContent();
        }




        [HttpPatch("{id}", Name = "UpdatePartialVillaById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (id <= 0)
            {
                return BadRequest("Id cannot be negative");
            }
            if (patchDto == null)
            {
                return BadRequest();
            }
            if (VillaStore.villaList.FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound();
            }
            var oldVilla = VillaStore.villaList.First(x => x.Id == id);
            patchDto.ApplyTo(oldVilla , ModelState);

            return !(ModelState.IsValid) ? BadRequest(ModelState) : NoContent();

        }

    }
}
