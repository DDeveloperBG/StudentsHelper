namespace StudentsHelper.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Services.Data.LocationLoaders;

    public class LocationsController : BaseController
    {
        private readonly Dictionary<string, ILocationLoader> locationLoaders;

        public LocationsController(
            RegionsLoader regionsLoader,
            TownshipsLoader townshipsLoader,
            PopulatedAreasLoader populatedAreasLoader,
            SchoolsLoader schoolsLoader)
        {
            this.locationLoaders = new Dictionary<string, ILocationLoader>
            {
                // For Registration page
                { "regions", regionsLoader },
                { "townships", townshipsLoader },
                { "populatedAreas", populatedAreasLoader },
                { "Input.TeacherModel.SchoolId", schoolsLoader },
                { "TeacherModel.SchoolId", schoolsLoader },

                // For AllTeachers page
                { "regionId", regionsLoader },
                { "townshipId", townshipsLoader },
                { "populatedAreaId", populatedAreasLoader },
                { "schoolId", schoolsLoader },

                // For TeacherSchool page
                { "Input.RegionId", regionsLoader },
                { "Input.TownshipId", townshipsLoader },
                { "Input.PopulatedAreaId", populatedAreasLoader },
                { "Input.SchoolId", schoolsLoader },
            };
        }

        public IActionResult Get(string selectName, int? lastSelectedId)
        {
            var locationLoader = this.locationLoaders.FirstOrDefault(x => x.Key.Contains(selectName)).Value;

            if (locationLoader != null && lastSelectedId != 0)
            {
                var result = locationLoader.GetLocations(lastSelectedId);
                return this.Json(result);
            }

            return null;
        }
    }
}
