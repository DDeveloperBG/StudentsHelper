namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Services.Data.Paging;
    using Xunit;

    public class PagingTests : BaseTest
    {
        private ReviewsPagingService pagingService;

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 7)]
        [InlineData(5, 19)]
        [InlineData(11, 19)]
        public void GetLocations_ReturnsCollectionOfLocations(int startIndex, int endIndex)
        {
            List<string> collection = Enumerable.Range(1, 20)
                .Select(x => x.ToString())
                .ToList();
            int count = endIndex - startIndex + 1;

            var result = this.pagingService
                .GetPaged(collection.AsQueryable(), startIndex + 1, count);

            Assert.Equal(startIndex + 1, result.CurrentNumber);
            Assert.Equal(count, result.PageSize);
            Assert.Equal(collection.Count, result.AllCount);

            for (int i = 0; i < count; i++)
            {
                Assert.Equal(collection[startIndex + i], result.Results[i]);
            }
        }

        public override void CleanWorkbench()
        {
            this.pagingService = new ReviewsPagingService();
        }
    }
}
