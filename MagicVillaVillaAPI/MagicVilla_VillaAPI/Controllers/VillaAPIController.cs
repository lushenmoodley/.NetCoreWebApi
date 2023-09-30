using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{

    [Route("api/VillaAPI")]
    [ApiController]//this tells the application this will be an api controller
    public class VillaAPIController:Controller
    {

        //creating an end point
        [HttpGet]
        public ActionResult <IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDTO> GetVilla(int id)//you have to pass an id in the http get or else it would crash
        {
            if(id==0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

            if(villa==null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            /*if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            */

            if(VillaStore.villaList.FirstOrDefault(x=>x.Name.ToLower()==villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error","Villa Already Exist!");
                return BadRequest(ModelState);
            }


            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            if(villaDTO.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStore.villaList.OrderByDescending(U => U.Id).FirstOrDefault().Id;


            VillaStore.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla",new { id=villaDTO.Id}, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

            if (villa==null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);

            return NoContent();

        }

        [HttpPut]
        public IActionResult UpdateVilla()
        {

        }


    }
}
