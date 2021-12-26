namespace StudentsHelper.Services.Data.LocationLoaders
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class SchoolsLoader : ILocationLoader
    {
        private readonly IDeletableEntityRepository<School> schoolRepository;

        public SchoolsLoader(IDeletableEntityRepository<School> schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        public ICollection<Location> GetLocations(int? lastLocationId)
        {
            return this.schoolRepository
                .All()
                .Where(x => x.PopulatedArea.Id == lastLocationId)
                .Select(x => new Location { Id = x.Id, Name = x.Name })
                .ToList();
        }
    }
}
