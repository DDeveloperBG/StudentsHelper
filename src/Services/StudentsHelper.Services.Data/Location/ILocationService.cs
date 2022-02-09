namespace StudentsHelper.Services.Data.Location
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Web.ViewModels.Locations;

    public interface ILocationService
    {
        IQueryable<Teacher> GetTeachersInLocation(int regionId, int townshipId, int populatedAreaId, int schoolId);

        LocationInputModel GetTeacherLocation(string id);

        Task ChangeTeacherLocationAsync(string userId, int schoolId);
    }
}
