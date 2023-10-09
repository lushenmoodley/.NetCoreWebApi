﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{

    [Route("api/VillaAPI")]
    [ApiController]//this tells the application this will be an api controller
    public class VillaAPIController:Controller
    {
        private readonly ILogging _logger;
        
        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }


        //creating an end point
        [HttpGet]
        public ActionResult <IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Getting all villas","");     
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
                _logger.Log("Get Villa Error with id"+id+"error","error");
              
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

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO==null || id!=villaDTO.Id)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy= villaDTO.Occupancy;

            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument <VillaDTO> patchDTO)
        {
            if(patchDTO==null || id==0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if(villa==null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villa,ModelState);

            return NoContent();

        }
    }
}
