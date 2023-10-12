using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{

    [Route("api/VillaAPI")]
    [ApiController]//this tells the application this will be an api controller
    public class VillaAPIController:Controller
    {
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        
        public VillaAPIController(IVillaRepository dbVilla,IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
        }


        //creating an end point
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
           return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<VillaDTO>> GetVilla(int id)//you have to pass an id in the http get or else it would crash
        {
            if(id==0)
            {
            
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id);

            if(villa==null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createDTO)
        {
            /*if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            */

            if(await _dbVilla.GetAsync(x=>x.Name.ToLower()==createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error","Villa Already Exist!");
                return BadRequest(ModelState);
            }


            if(createDTO == null)
            {
                return BadRequest(createDTO);
            }

            Villa model=_mapper.Map<Villa>(createDTO);

            await   _dbVilla.CreateAsync(model);   
            
            return CreatedAtRoute("GetVilla",new { id=model.Id}, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id);

            if (villa==null)
            {
                return NotFound();
            }


            await _dbVilla.RemoveAsync(villa);
          
            return NoContent();

        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO)
        {
            if(updateDTO==null || id!=updateDTO.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            //villa.Name = villaDTO.Name;
            //villa.Sqft = villaDTO.Sqft;
            //villa.Occupancy= villaDTO.Occupancy;

            //Villa model = new()
            //{
            //    Amenity = updateDTO.Amenity,
            //    Details = updateDTO.Details,
            //    Id = updateDTO.Id,
            //    ImageUrl = updateDTO.ImageUrl,
            //    Name = updateDTO.Name,
            //    Occupancy = updateDTO.Occupancy,
            //    Rate = updateDTO.Rate,
            //    Sqft = updateDTO.Sqft,
            //};

            Villa model=_mapper.Map<Villa>(updateDTO);  

            await _dbVilla.UpdateAsync(model);
        
            return NoContent();

        }

        //[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult UpdatePartialVilla(int id,JsonPatchDocument <VillaDTO> villaDTO)
        //{
        //    if(villaDTO==null || id==0)
        //    {
        //        return BadRequest();
        //    }

        //    var villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);

            
        //    Villa villaDTO2 = new()
        //    {
        //        Amenity = villa.Amenity,
        //        Details = villa.Details,
        //        Id = villa.Id,
        //        ImageUrl = villa.ImageUrl,
        //        Name = villa.Name,
        //        Occupancy = villa.Occupancy,
        //        Rate = villa.Rate,
        //        Sqft = villa.Sqft,
        //    };

        //    Villa model = new Villa()
        //    {
        //        Amenity = villaDTO2.Amenity,
        //        Details = villaDTO2.Details,
        //        Id = villaDTO2.Id,
        //        ImageUrl = villaDTO2.ImageUrl,
        //        Name = villaDTO2.Name,
        //        Occupancy = villaDTO2.Occupancy,
        //        Rate = villaDTO2.Rate,
        //        Sqft = villaDTO2.Sqft,
        //    };


        //    if (villa==null)
        //    {
        //        return BadRequest();
        //    }
        //    /*
        //    _db.Villas.Update(model);
        //    _db.SaveChanges();*/

        //   if(!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return NoContent();

        //}
    }
}
