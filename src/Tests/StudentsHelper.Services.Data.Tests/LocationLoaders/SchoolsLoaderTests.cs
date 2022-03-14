namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.LocationLoaders;

    using Xunit;

    public class SchoolsLoaderTests : BaseTest
    {
        private List<School> schools;
        private SchoolsLoader schoolsLoader;

        [Fact]
        public void GetLocations_ReturnsCollectionOfLocations()
        {
            List<School> addedSchools = new List<School>();
            int lastLocationId = 2;
            for (int i = 0; i < 20; i++)
            {
                var school = new School
                {
                    Id = i,
                    Name = $"School_{i}",
                    PopulatedAreaId = lastLocationId,
                };

                addedSchools.Add(school);
                this.schools.Add(school);
            }

            var result = this.schoolsLoader.GetLocations(lastLocationId);

            Assert.True(addedSchools.All(x => result.Any(y => y.Id == x.Id && y.Name == x.Name)));
        }

        public override void CleanWorkbench()
        {
            this.schools = new List<School>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.schools);

            this.schoolsLoader = new SchoolsLoader(repository.Object);
        }
    }
}
