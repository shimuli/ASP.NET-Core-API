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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepo _npRepo;
        private readonly IMapper _imapper;

        public NationalParksController(INationalParkRepo npRepo, IMapper imapper)
        {
            _npRepo = npRepo;
            _imapper = imapper;
        }

        /// <summary>
        /// Get List of all registered National Parks in Kenya
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(List<NationalParkDto>))]
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

        /// <summary>
        /// Get One Registered National PArk in Kenya
        /// </summary>
        /// <param name="nationalParkId">The Id of the national Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _imapper.Map<NationalParkDto>(obj);
           /* var objDto = new NationalParkDto()
            {
                Created = obj.Created,
                Id = obj.Id,
                Name = obj.Name,
                State = obj.State,
            };
           */
            return Ok(objDto);
        }
        /// <summary>
        /// Post a national Park
        /// </summary>
        /// <param name="nationalParkDto">Information required to do the update</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Update a national Park
        /// </summary>
        /// <param name="nationalParkId">National Park Id</param>
        /// <param name="nationalParkDto">National Park data to update</param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Delete a National Park
        /// </summary>
        /// <param name="nationalParkId">National Park Id</param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
