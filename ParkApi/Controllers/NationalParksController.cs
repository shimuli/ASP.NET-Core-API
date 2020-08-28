using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkApi.Models;
using ParkApi.Models.Dtos;
using ParkApi.Repo.IRepo;

namespace ParkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepo _npRepo;
        private readonly IMapper _imapper;

        public NationalParksController(INationalParkRepo npRepo, IMapper imapper)
        {
            _npRepo = npRepo;
            _imapper = imapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();
            
            foreach(var obj in objList)
            {
                objDto.Add(_imapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto);
        }
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _imapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }
            
            var nationalParkObj = _imapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
           // return Ok();
            return CreatedAtRoute("GetNationalPark", new
            {
                nationalParkId = nationalParkObj.Id
            }, nationalParkObj);
        }

        [HttpPatch("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId!=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalParkObj = _imapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when Updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();


        }

        [HttpDelete("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult DeleteNationalPark (int nationalParkId) {

            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var nationalParkObj = _npRepo.GetNationalPark(nationalParkId);

            if (!_npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when Deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();


        }
    }
}
