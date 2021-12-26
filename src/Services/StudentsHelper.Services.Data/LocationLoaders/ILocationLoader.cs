namespace StudentsHelper.Services.Data.LocationLoaders
{
    using System.Collections.Generic;

    public interface ILocationLoader
    {
        public ICollection<Location> GetLocations(int? lastLocationId);
    }
}
