namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.LocationLoaders;

    using Xunit;

    public class TownshipsLoaderTests : BaseTest
    {
        private List<Township> townships;
        private TownshipsLoader townshipsLoader;

        [Fact]
        public void GetLocations_ReturnsCollectionOfLocations()
        {
            List<Township> addedTownships = new List<Township>();
            int lastLocationId = 2;
            for (int i = 0; i < 20; i++)
            {
                var township = new Township
                {
                    Id = i,
                    Name = $"Township_{i}",
                    RegionId = lastLocationId,
                };

                addedTownships.Add(township);
                this.townships.Add(township);
            }

            var result = this.townshipsLoader.GetLocations(lastLocationId);

            Assert.True(addedTownships.All(x => result.Any(y => y.Id == x.Id && y.Name == x.Name)));
        }

        public override void CleanWorkbench()
        {
            this.townships = new List<Township>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.townships);

            this.townshipsLoader = new TownshipsLoader(repository.Object);
        }
    }
}
