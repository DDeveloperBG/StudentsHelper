namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using Xunit;

    public class SchoolSubjectsServiceTests : BaseTest
    {
        private List<SchoolSubject> schoolSubjects;
        private SchoolSubjectsService schoolSubjectsService;

        [Fact]
        public void GetAll_ReturnsCollection()
        {
            var addedSchoolSubjects = new List<SchoolSubject>();
            for (int i = 0; i < 5; i++)
            {
                var schoolSubject = new SchoolSubject
                {
                    Id = i,
                    Name = $"name_{i}",
                    IconPath = $"iconPath_{i}",
                };

                addedSchoolSubjects.Add(schoolSubject);
                this.schoolSubjects.Add(schoolSubject);
            }

            var result = this.schoolSubjectsService.GetAll<GetAllTestModel>();
            Assert.True(addedSchoolSubjects.All(x => result.Any(y => y.Id == x.Id && y.Name == x.Name && y.IconPath == x.IconPath)));
        }

        [Fact]
        public void GetAllRaw_ReturnsCollection()
        {
            var addedSchoolSubjects = new List<SchoolSubject>();
            for (int i = 0; i < 5; i++)
            {
                var schoolSubject = new SchoolSubject
                {
                    Id = i,
                    Name = $"name_{i}",
                    IconPath = $"iconPath_{i}",
                };

                addedSchoolSubjects.Add(schoolSubject);
                this.schoolSubjects.Add(schoolSubject);
            }

            var result = this.schoolSubjectsService.GetAllRaw();
            Assert.True(addedSchoolSubjects.All(x => result.Any(y => y.Id == x.Id && y.Name == x.Name && y.IconPath == x.IconPath)));
        }

        [Fact]
        public void GetTeacherSubjectsIds_ReturnsTeacherSubjectsIds()
        {
            var addedSchoolSubjects = new List<SchoolSubject>();
            var teacher = new Teacher
            {
                ApplicationUserId = "1",
            };
            for (int i = 0; i < 5; i++)
            {
                var schoolSubject = new SchoolSubject
                {
                    Id = i,
                    Name = $"name_{i}",
                    IconPath = $"iconPath_{i}",
                    Teachers = new List<Teacher> { teacher },
                };

                addedSchoolSubjects.Add(schoolSubject);
                this.schoolSubjects.Add(schoolSubject);
            }

            var result = this.schoolSubjectsService.GetTeacherSubjectsIds(teacher.Id);
            Assert.True(addedSchoolSubjects
                .All(x => result
                    .Any(y => y == x.Id)));
        }

        public override void CleanWorkbench()
        {
            this.schoolSubjects = new List<SchoolSubject>();

            var repository = GetMockedClasses.MockIRepository(this.schoolSubjects);

            this.schoolSubjectsService = new SchoolSubjectsService(repository.Object);
        }
    }
}
