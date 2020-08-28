using AutoMapper;
using ParkApi.Models;
using ParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Mappers
{
    public class ParkMapper : Profile
    {
        public ParkMapper()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
