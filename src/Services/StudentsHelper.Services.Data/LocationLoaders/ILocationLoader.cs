namespace StudentsHelper.Services.Data.LocationLoaders
{
    using System.Collections.Generic;

    public interface ILocationLoader
    {
        public IEnumerable<Location> GetLocations(int? lastLocationId);
    }
}
