using Microsoft.EntityFrameworkCore;
using ParkApi.Models;
using ParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Data
{
    public class AplicationDbContent: DbContext
    {
        //contsructor
        public AplicationDbContent(DbContextOptions<AplicationDbContent>options) :base(options)
        {
            
        }
        //add national park to db
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
    }
}
