namespace StudentsHelper.Services.Data.LocationLoaders
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class TownshipsLoader : ILocationLoader
    {
        private readonly IDeletableEntityRepository<Township> townshipRepository;

        public TownshipsLoader(IDeletableEntityRepository<Township> townshipRepository)
        {
            this.townshipRepository = townshipRepository;
        }

        public IEnumerable<Location> GetLocations(int? lastLocationId)
        {
            return this.townshipRepository
                .All()
                .Where(x => x.Region.Id == lastLocationId)
                .Select(x => new Location { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }
}
