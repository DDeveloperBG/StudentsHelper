namespace StudentsHelper.Services.Data.StudentFavouriteTeachers
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models.Favourite;
    using StudentsHelper.Services.Mapping;

    public class StudentFavouriteTeachersService : IStudentFavouriteTeachersService
    {
        private readonly IRepository<StudentFavouriteTeacher> studentsFavouriteTeachersRepository;

        public StudentFavouriteTeachersService(
             IRepository<StudentFavouriteTeacher> studentsFavouriteTeachersRepository)
        {
            this.studentsFavouriteTeachersRepository = studentsFavouriteTeachersRepository;
        }

        public async Task AddOrRemoveAsync(string studentId, string teacherId)
        {
            var studentFavouriteTeacher = this.GetAll()
               .Where(x => x.StudentId == studentId && x.TeacherId == teacherId)
               .SingleOrDefault();

            if (studentFavouriteTeacher != null)
            {
                studentFavouriteTeacher.StillIsFavourite = !studentFavouriteTeacher.StillIsFavourite;
            }
            else
            {
                studentFavouriteTeacher = new StudentFavouriteTeacher
                {
                    StudentId = studentId,
                    TeacherId = teacherId,
                };

                await this.studentsFavouriteTeachersRepository.AddAsync(studentFavouriteTeacher);
            }

            await this.studentsFavouriteTeachersRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetStudentAllFavouriteTeachers<T>(string studentId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.StudentId == studentId)
                .Select(x => x.Teacher)
                .To<T>();
        }

        public IQueryable<StudentFavouriteTeacher> GetAllAsNoTracking()
        {
            return this.studentsFavouriteTeachersRepository
                .AllAsNoTracking()
                .Where(x => x.StillIsFavourite)
                .Where(x => x.Teacher.ApplicationUser.UserName != GlobalConstants.DeletedUserUsername);
        }

        public bool IsTeacherFavouriteToStudent(string studentId, string teacherId)
        {
            return this.GetAll()
                .Where(x => x.StudentId == studentId && x.TeacherId == teacherId)
                .Count() > 0;
        }

        public IQueryable<StudentFavouriteTeacher> GetAll()
        {
            return this.studentsFavouriteTeachersRepository
                .All()
                .Where(x => x.StillIsFavourite)
                .Where(x => x.Teacher.ApplicationUser.UserName != GlobalConstants.DeletedUserUsername);
        }
    }
}
