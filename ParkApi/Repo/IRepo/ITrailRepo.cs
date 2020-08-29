using ParkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Repo.IRepo
{
    public interface ITrailRepo
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int npId);
        Trail GetTrail(int nationalParkId);
        bool TrailExists(string name);
        bool TrailExists(int id);
        bool UpdateTrail(Trail nationalPark);
        bool CreateTrail(Trail nationalPark);
        bool DeleteTrail(Trail nationalPark);

        bool Save();
    }
}
