namespace StudentsHelper.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.Data.Paging;
    using StudentsHelper.Services.Data.Ratings;

    using StudentsHelper.Web.ViewModels.Reviews;

    public class StudentReviewViewComponent : ViewComponent
    {
        private const int PagingSize = 4;
        private readonly IReviewsService reviewsService;
        private readonly IPagingService pagingService;

        public StudentReviewViewComponent(
            IReviewsService reviewsService,
            IPagingService pagingService)
        {
            this.reviewsService = reviewsService;
            this.pagingService = pagingService;
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

            return this.View(result);
        }
    }
}
