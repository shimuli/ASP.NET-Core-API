using ParkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Repo.IRepo
{
    public interface INationalParkRepo
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool UpdateNationalPark(NationalPark nationalPark);
        bool CreateNationalPark(NationalPark nationalPark);
        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();
    }
}
