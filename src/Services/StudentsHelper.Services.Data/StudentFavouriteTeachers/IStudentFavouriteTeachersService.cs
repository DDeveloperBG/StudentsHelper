namespace StudentsHelper.Services.Data.StudentFavouriteTeachers
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IStudentFavouriteTeachersService
    {
        public Task AddOrRemoveAsync(string studentId, string teacherId);

        public IQueryable<T> GetStudentAllFavouriteTeachers<T>(string studentId);

        public bool IsTeacherFavouriteToStudent(string studentId, string teacherId);
    }
}
