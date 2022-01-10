namespace StudentsHelper.Services.Data.LocationLoaders
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class RegionsLoader : ILocationLoader
    {
        private readonly IDeletableEntityRepository<Region> regionRepository;

        public RegionsLoader(IDeletableEntityRepository<Region> regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        public IEnumerable<Location> GetLocations(int? lastLocationId)
        {
            return this.regionRepository
                .All()
                .Select(x => new Location { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }
}
