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
    //[ApiExplorerSettings(GroupName = "ParkOpenApiSpecNationalParks")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : Controller
    {
        private readonly INationalParkRepo _npRepo;
        private readonly IMapper _imapper;

        public NationalParksV2Controller(INationalParkRepo npRepo, IMapper imapper)
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
    }
}
