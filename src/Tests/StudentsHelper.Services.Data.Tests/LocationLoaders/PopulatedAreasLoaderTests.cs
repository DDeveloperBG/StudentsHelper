namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.LocationLoaders;

    using Xunit;

    public class PopulatedAreasLoaderTests : BaseTest
    {
        private List<PopulatedArea> populatedAreas;
        private PopulatedAreasLoader populatedAreasLoader;

        [Fact]
        public void GetLocations_ReturnsCollectionOfLocations()
        {
            int lastLocationId = 2;
            List<PopulatedArea> addedPopulatedAreas = new List<PopulatedArea>();
            for (int i = 0; i < 20; i++)
            {
                var populatedArea = new PopulatedArea
                {
                    Id = i,
                    Name = $"PopulatedArea_{i}",
                    TownshipId = lastLocationId,
                };

                addedPopulatedAreas.Add(populatedArea);
                this.populatedAreas.Add(populatedArea);
            }

            var result = this.populatedAreasLoader.GetLocations(lastLocationId);

            Assert.True(addedPopulatedAreas.All(x => result.Any(y => y.Id == x.Id && y.Name == x.Name)));
        }

        public override void CleanWorkbench()
        {
            this.populatedAreas = new List<PopulatedArea>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.populatedAreas);

            this.populatedAreasLoader = new PopulatedAreasLoader(repository.Object);
        }
    }
}
