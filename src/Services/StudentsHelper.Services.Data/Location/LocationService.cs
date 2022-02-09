namespace StudentsHelper.Services.Data.Location
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.ViewModels.Locations;

    public class LocationService : ILocationService
    {
        private readonly IRepository<Region> regionsRepository;
        private readonly ITeachersService teachersService;

        public LocationService(
            IRepository<Region> regionsRepository,
            ITeachersService teachersService)
        {
            this.regionsRepository = regionsRepository;
            this.teachersService = teachersService;
        }

        public LocationInputModel GetTeacherLocation(string userId)
        {
            return this.teachersService
                .GetAllAsNoTracking()
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => new LocationInputModel
                {
                    RegionId = x.School.PopulatedArea.Township.RegionId,
                    TownshipId = x.School.PopulatedArea.TownshipId,
                    PopulatedAreaId = x.School.PopulatedAreaId,
                    SchoolId = x.SchoolId,
                })
                .Single();
        }

        public Task ChangeTeacherLocationAsync(string userId, int schoolId)
        {
            var teacher = this.teachersService
                .GetAll()
                .Where(x => x.ApplicationUserId == userId)
                .Single();

            teacher.SchoolId = schoolId;
            return this.regionsRepository.SaveChangesAsync();
        }

        public IQueryable<Teacher> GetTeachersInLocation(int regionId, int townshipId, int populatedAreaId, int schoolId)
        {
            var teachersByRegion = this.regionsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == regionId);

            if (townshipId > 0)
            {
                var teachersByTownship = teachersByRegion
                    .SelectMany(x => x.Townships
                        .Where(x => x.Id == townshipId));

                if (populatedAreaId > 0)
                {
                    var teachersByPopulatedArea = teachersByTownship
                        .SelectMany(x => x.PopulatedAreas
                            .Where(x => x.Id == populatedAreaId));

                    if (schoolId > 0)
                    {
                        return teachersByPopulatedArea
                               .SelectMany(x => x.Schools
                                    .Where(y => y.Id == schoolId)
                                    .SelectMany(x => x.Teachers));
                    }

                    return teachersByPopulatedArea
                        .SelectMany(x => x.Schools
                            .SelectMany(y => y.Teachers));
                }

                return teachersByTownship
                        .SelectMany(x => x.PopulatedAreas
                            .SelectMany(y => y.Schools
                            .SelectMany(y => y.Teachers)));
            }

            return teachersByRegion
                .SelectMany(x => x.Townships
                    .SelectMany(y => y.PopulatedAreas
                        .SelectMany(z => z.Schools
                            .SelectMany(t => t.Teachers))));
        }
    }
}
