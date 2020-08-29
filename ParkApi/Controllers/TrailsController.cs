using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkApi.Data.Dtos;
using ParkApi.Models;
using ParkApi.Models.Dtos;
using ParkApi.Repo.IRepo;

namespace ParkApi.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepo _trailRepo;
        private readonly IMapper _imapper;

        public TrailsController(ITrailRepo trailRepo, IMapper imapper)
        {
            _trailRepo = trailRepo;
            _imapper = imapper;
        }

        /// <summary>
        /// Get List of all registered National Parks in Kenya
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            
            foreach(var obj in objList)
            {
                objDto.Add(_imapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get One Registered National PArk in Kenya
        /// </summary>
        /// <param name="trailId">The Id of the national Park</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _imapper.Map<TrailDto>(obj);
           /* var objDto = new TrailDto()
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
        /// <param name="trailDto">Information required to do the update</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            if(trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }
            
            var trailObj = _imapper.Map<Trail>(trailDto);

            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
           // return Ok();
            return CreatedAtRoute("GetTrail", new
            {
                trailId = trailObj.Id
            }, trailObj);
        }
        /// <summary>
        /// Update a national Park
        /// </summary>
        /// <param name="trailId">National Park Id</param>
        /// <param name="trailDto">National Park data to update</param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailDto trailDto)
        {
            if (trailDto == null || trailId!=trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _imapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when Updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();


        }
        /// <summary>
        /// Delete a National Park
        /// </summary>
        /// <param name="trailId">National Park Id</param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail (int trailId) {

            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _trailRepo.GetTrail(trailId);

            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when Deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();


        }
    }
}
