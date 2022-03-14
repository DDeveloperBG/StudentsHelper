namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Students;

    using Xunit;

    public class StudentsServiceTests : BaseTest
    {
        private List<Student> students;
        private StudentsService studentsService;

        [Fact]
        public void GetId_ReturnsStudentId()
        {
            const string searchedStudentId = "importantId";
            const string searchedStudentUserId = "-1-";
            var students = new List<Student>
            {
                new Student
                {
                    Id = searchedStudentId,
                    ApplicationUserId = searchedStudentUserId,
                },
                new Student
                {
                    ApplicationUserId = "123",
                },
                new Student
                {
                    ApplicationUserId = "321",
                },
                new Student
                {
                    ApplicationUserId = "abc!12",
                },
            };

            this.students.AddRange(students);

            string result = this.studentsService.GetId(searchedStudentUserId);

            Assert.Equal(searchedStudentId, result);
        }

        [Fact]
        public void GetOne_Succeede()
        {
            const string searchedStudentId = "importantId";
            const string searchedStudentUserId = "-1-";
            var students = new List<Student>
            {
                new Student
                {
                    Id = searchedStudentId,
                    ApplicationUserId = searchedStudentUserId,
                },
                new Student
                {
                    ApplicationUserId = "123",
                },
                new Student
                {
                    ApplicationUserId = "321",
                },
                new Student
                {
                    ApplicationUserId = "abc!12",
                },
            };

            this.students.AddRange(students);

            var result = this.studentsService.GetOne<GetOneTestClass>(searchedStudentUserId);

            Assert.Equal(searchedStudentId, result.Id);
            Assert.Equal(searchedStudentUserId, result.ApplicationUserId);
        }

        [Fact]
        public void GetOneFromStudentId_Succeede()
        {
            const string searchedStudentId = "importantId";
            const string searchedStudentUserId = "-1-";
            var students = new List<Student>
            {
                new Student
                {
                    Id = searchedStudentId,
                    ApplicationUserId = searchedStudentUserId,
                },
                new Student
                {
                    ApplicationUserId = "123",
                },
                new Student
                {
                    ApplicationUserId = "321",
                },
                new Student
                {
                    ApplicationUserId = "abc!12",
                },
            };

            this.students.AddRange(students);

            var result = this.studentsService.GetOneFromStudentId<GetOneTestClass>(searchedStudentId);

            Assert.Equal(searchedStudentId, result.Id);
            Assert.Equal(searchedStudentUserId, result.ApplicationUserId);
        }

        [Fact]
        public void GetOneTracked_Succeede()
        {
            const string searchedStudentId = "importantId";
            const string searchedStudentUserId = "-1-";
            var students = new List<Student>
            {
                new Student
                {
                    Id = searchedStudentId,
                    ApplicationUserId = searchedStudentUserId,
                },
                new Student
                {
                    ApplicationUserId = "123",
                },
                new Student
                {
                    ApplicationUserId = "321",
                },
                new Student
                {
                    ApplicationUserId = "abc!12",
                },
            };

            this.students.AddRange(students);

            var result = this.studentsService.GetOneTracked(searchedStudentId);

            Assert.Equal(students.First(x => x.Id == searchedStudentId), result);
        }

        [Fact]
        public void GetAllAsNoTracking_Succeede()
        {
            const string deletedStudentId = "importantId";
            var students = new List<Student>
            {
                new Student
                {
                    Id = deletedStudentId,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = GlobalConstants.DeletedUserUsername,
                    },
                },
                new Student
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "456",
                    },
                },
                new Student
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "321",
                    },
                },
                new Student
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "123",
                    },
                },
            };

            this.students.AddRange(students);

            var result = this.studentsService.GetAllAsNoTracking().ToList();

            Assert.Equal(students.Count - 1, result.Count);
            Assert.True(!result.Any(x => x.Id == deletedStudentId));
            Assert.True(students.Skip(1).All(x => result.Any(y => y.Id == x.Id)));
        }

        [Fact]
        public void DeleteStudentAsync_Succeede()
        {
            var student = new Student
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = "456",
                },
            };

            this.students.Add(student);

            this.studentsService.DeleteStudentAsync(student.ApplicationUserId).GetAwaiter().GetResult();

            Assert.Equal(GlobalConstants.DeletedUserUsername, student.ApplicationUser.UserName);
        }

        public override void CleanWorkbench()
        {
            this.students = new List<Student>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.students);

            var deletedUser = new ApplicationUser
            {
                UserName = GlobalConstants.DeletedUserUsername,
            };

            var userManager = GetMockedClasses.MockUserManager<ApplicationUser>();
            userManager
                .Setup(u => u.FindByNameAsync(It.Is<string>(x => x == GlobalConstants.DeletedUserUsername)))
                .Returns(Task.FromResult(deletedUser));

            this.studentsService = new StudentsService(repository.Object, userManager.Object);
        }
    }
}
