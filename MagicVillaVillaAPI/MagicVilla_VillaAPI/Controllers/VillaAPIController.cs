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
        public IEnumerable<VillaDTO> GetVillas()
        {
            return VillaStore.villaList;
        }

        [HttpGet("{id:int}")]
        public VillaDTO GetVillas(int id)//you have to pass an id in the http get or else it would crash
        {
            return VillaStore.villaList.FirstOrDefault(u=>u.Id==id);
        }

    }
}
