namespace StudentsHelper.Services.Data.LocationLoaders
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class PopulatedAreasLoader : ILocationLoader
    {
        private readonly IDeletableEntityRepository<PopulatedArea> populatedAreaRepository;

        public PopulatedAreasLoader(IDeletableEntityRepository<PopulatedArea> populatedAreaRepository)
        {
            this.populatedAreaRepository = populatedAreaRepository;
        }

        public ICollection<Location> GetLocations(int? lastLocationId)
        {
            return this.populatedAreaRepository
                .All()
                .Where(x => x.Township.Id == lastLocationId)
                .Select(x => new Location { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }
}
