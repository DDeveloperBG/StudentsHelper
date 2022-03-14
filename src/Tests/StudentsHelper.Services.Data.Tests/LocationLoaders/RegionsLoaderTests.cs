namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.LocationLoaders;

    using Xunit;

    public class RegionsLoaderTests : BaseTest
    {
        private List<Region> regions;
        private RegionsLoader regionsLoader;

        [Fact]
        public void GetLocations_ReturnsCollectionOfLocations()
        {
            List<Region> addedRegions = new List<Region>();
            for (int i = 0; i < 20; i++)
            {
                var region = new Region
                {
                    Id = i,
                    Name = $"Region_{i}",
                };

                addedRegions.Add(region);
                this.regions.Add(region);
            }

            var result = this.regionsLoader.GetLocations(null);

            Assert.True(addedRegions.All(x => result.Any(y => y.Id == x.Id && y.Name == x.Name)));
        }

        public override void CleanWorkbench()
        {
            this.regions = new List<Region>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.regions);

            this.regionsLoader = new RegionsLoader(repository.Object);
        }
    }
}
