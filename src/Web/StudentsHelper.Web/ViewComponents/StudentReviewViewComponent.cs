namespace StudentsHelper.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Paging;
    using StudentsHelper.Services.Data.Ratings;

    using StudentsHelper.Web.ViewModels.Reviews;

    public class StudentReviewViewComponent : ViewComponent
    {
        private const int PagingSize = 4;
        private readonly IReviewsService reviewsService;
        private readonly IReviewsPagingService pagingService;
        private readonly ICloudStorageService cloudStorageService;

        public StudentReviewViewComponent(
            IReviewsService reviewsService,
            IReviewsPagingService pagingService,
            ICloudStorageService cloudStorageService)
        {
            this.reviewsService = reviewsService;
            this.pagingService = pagingService;
            this.cloudStorageService = cloudStorageService;
        }

        public IViewComponentResult Invoke(string teacherId, int currentNumber)
        {
            int nextNumber;
            if (currentNumber <= 0)
            {
                nextNumber = 1;
            }
            else
            {
                nextNumber = currentNumber + PagingSize;
            }

            var teachersQuery = this.reviewsService.GetAllReviewsForTeacher<StudentReview>(teacherId);
            var result = this.pagingService.GetPaged(teachersQuery, nextNumber, PagingSize);
            foreach (var item in result.Results)
            {
                item.StudentApplicationUserPicturePath =
                    this.cloudStorageService.GetImageUri(item.StudentApplicationUserPicturePath, 50, 50);
            }

            return this.View(result);
        }
    }
}
