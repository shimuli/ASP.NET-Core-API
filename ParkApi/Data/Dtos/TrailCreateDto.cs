using ParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ParkApi.Models.Trail;

namespace ParkApi.Data.Dtos
{
    public class TrailCreateDto
    {

        [Required]
        public string Name { get; set; }

        public string Distance { get; set; }


        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

    }
}
