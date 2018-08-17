using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context) => _context = context;
        
        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest) =>
            GetCity(cityId, false).PointsOfInterest.Add(pointOfInterest);

        public bool CityExists(int cityId) => _context.Cities.Any(c => c.Id == cityId);
        
        public IEnumerable<City> GetCities() => _context.Cities.OrderBy(c => c.Name).ToList();
        

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
            => _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefault();        

        public IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId) =>
            _context.PointsOfInterest.Where(c => c.CityId == cityId).ToList();
        
        public void DeletePointOfInterest(PointOfInterest pointOfInterest) => 
            _context.PointsOfInterest.Remove(pointOfInterest);

        public bool Save() => (_context.SaveChanges() >= 0);
    }
}
