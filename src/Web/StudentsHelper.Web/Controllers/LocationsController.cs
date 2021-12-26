namespace StudentsHelper.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.Data.LocationLoaders;

    public class LocationsController : Controller
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
                { "regions", regionsLoader },
                { "townships", townshipsLoader },
                { "populatedAreas", populatedAreasLoader },
                { "Input.TeacherModel.SchoolId", schoolsLoader },
            };
        }

        public IActionResult Get(string selectName, int? lastSelectedId)
        {
            if (this.locationLoaders.ContainsKey(selectName) && lastSelectedId != 0)
            {
                var result = this.locationLoaders[selectName].GetLocations(lastSelectedId);
                return this.Json(result);
            }

            return null;
        }
    }
}
