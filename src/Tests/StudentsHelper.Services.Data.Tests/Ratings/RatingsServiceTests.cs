namespace StudentsHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.Teachers;

    using Xunit;

    public class RatingsServiceTests : BaseTest
    {
        private List<Review> reviews;
        private List<SchoolSubject> schoolSubjects;
        private List<Teacher> teachers;
        private ReviewsService service;

        [Theory]
        [InlineData("1x3fs24faz", "152s2s444z", 1, "Hello From Me1!!!")]
        [InlineData("2424", "4242", 3, "Hello From Me2!!!")]
        [InlineData("422141", "kafskkfask", 24, "Hello From Me3!!!")]
        [InlineData("da", "12", 16, "Hello From Me4!!!")]
        public void AddReviewAsync_Succeede(string teacherId, string studentId, byte rating, string comment)
        {
            this.service.AddReviewAsync(teacherId, studentId, rating, comment).GetAwaiter().GetResult();

            Assert.Single(this.reviews);

            var addedReview = this.reviews.First();

            Assert.Equal(addedReview.TeacherId, teacherId);
            Assert.Equal(addedReview.StudentId, studentId);
            Assert.Equal(addedReview.Rating, rating);
            Assert.Equal(addedReview.Comment, comment);
        }

        [Fact]
        public void HasStudentReviewedTeacher_Succeede()
        {
            const string reviewedStudentId = "1x3fs24faz";
            const string notReviewedStudentId = "2424";
            const string existingTeacherId = "kafskkfask";
            const string notExistingTeacherId = "152s2s444z";
            var reviews = new List<Review>
            {
                new Review
                {
                    StudentId = reviewedStudentId,
                    TeacherId = existingTeacherId,
                },
            };

            this.reviews.AddRange(reviews);

            Assert.True(this.service.HasStudentReviewedTeacher(reviewedStudentId, existingTeacherId));
            Assert.False(this.service.HasStudentReviewedTeacher(notReviewedStudentId, notExistingTeacherId));
            Assert.False(this.service.HasStudentReviewedTeacher(reviewedStudentId, notExistingTeacherId));
        }

        [Fact]
        public void GetTeachersRating_Succeede()
        {
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    HourWage = 2.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "Georgi123",
                        Email = "georgi@mail.en",
                        PicturePath = "picturePath/123",
                    },
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            Rating = 5,
                        },
                    },
                },
                new Teacher
                {
                    HourWage = 15.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "IVcani123",
                        Email = "Ivo@mail.en",
                        PicturePath = "picturePath12/5As24",
                    },
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            Rating = 1,
                        },
                        new Review
                        {
                            Rating = 2,
                        },
                        new Review
                        {
                            Rating = 2,
                        },
                        new Review
                        {
                            Rating = 5,
                        },
                        new Review
                        {
                            Rating = 5,
                        },
                    },
                },
                new Teacher
                {
                    HourWage = 8.54M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "Geo123",
                        Email = "geo@abv.en",
                        PicturePath = "picturePath/44As123",
                    },
                    Reviews = new List<Review>
                    {
                    },
                },
            };

            var result = this.service.GetTeachersRating(teachers.AsQueryable());

            foreach (var resultTeacher in result)
            {
                var expectedTeacher = teachers.Find(x => x.Id == resultTeacher.Id);
                var expectedAverageRating = expectedTeacher.Reviews.Count != 0 ? Math.Round(expectedTeacher.Reviews.Average(x => x.Rating), 2) : 0;

                Assert.NotNull(expectedTeacher);
                Assert.Equal(expectedTeacher.HourWage, resultTeacher.HourWage);
                Assert.Equal(expectedTeacher.ApplicationUser.Email, resultTeacher.ApplicationUserEmail);
                Assert.Equal(expectedTeacher.ApplicationUser.Name, resultTeacher.ApplicationUserName);
                Assert.Equal(expectedTeacher.ApplicationUser.PicturePath, resultTeacher.ApplicationUserPicturePath);
                Assert.Equal(expectedAverageRating, resultTeacher.AverageRating);
            }
        }

        [Fact]
        public void GetTeacherRating_Succeede()
        {
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    HourWage = 2.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "Georgi123",
                        Email = "georgi@mail.en",
                        PicturePath = "picturePath/123",
                    },
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            Rating = 5,
                        },
                    },
                },
                new Teacher
                {
                    HourWage = 15.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "IVcani123",
                        Email = "Ivo@mail.en",
                        PicturePath = "picturePath12/5As24",
                    },
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            Rating = 1,
                        },
                        new Review
                        {
                            Rating = 2,
                        },
                        new Review
                        {
                            Rating = 2,
                        },
                        new Review
                        {
                            Rating = 5,
                        },
                        new Review
                        {
                            Rating = 5,
                        },
                    },
                },
                new Teacher
                {
                    HourWage = 8.54M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "Geo123",
                        Email = "geo@abv.en",
                        PicturePath = "picturePath/44As123",
                    },
                    Reviews = new List<Review>
                    {
                    },
                },
            };

            this.teachers.AddRange(teachers);

            foreach (var expectedTeacher in teachers)
            {
                var result = this.service.GetTeacherRating(expectedTeacher.Id);
                var expectedAverageRating = expectedTeacher.Reviews.Count != 0 ? Math.Round(expectedTeacher.Reviews.Average(x => x.Rating), 2) : 0;

                Assert.NotNull(result);
                Assert.Equal(expectedTeacher.Id, result.Id);
                Assert.Equal(expectedTeacher.HourWage, result.HourWage);
                Assert.Equal(expectedTeacher.ApplicationUser.Email, result.ApplicationUserEmail);
                Assert.Equal(expectedTeacher.ApplicationUser.Name, result.ApplicationUserName);
                Assert.Equal(expectedTeacher.ApplicationUser.PicturePath, result.ApplicationUserPicturePath);

                Assert.Equal(5, result.RatingRangesCount.Count);
                for (int i = 0; i < 5; i++)
                {
                    var expectedCount = expectedTeacher.Reviews.Count(x => x.Rating == i + 1);
                    Assert.Equal(expectedCount, result.RatingRangesCount[i]);
                }

                Assert.Equal(expectedAverageRating, result.AverageRating);
            }
        }

        [Fact]
        public void GetAllReviewsForTeacher_Succeede()
        {
            var student1 = new Student
            {
                ApplicationUser = new ApplicationUser
                {
                    Name = "Gosho123",
                    Email = "gosgo123@gmail.com",
                    PicturePath = "picture/gosho123",
                },
            };
            var student2 = new Student
            {
                ApplicationUser = new ApplicationUser
                {
                    Name = "Ivcan321",
                    Email = "Ivan123@gmail.com",
                    PicturePath = "pictur23e/Ivan123",
                },
            };

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    HourWage = 2.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "Georgi123",
                        Email = "georgi@mail.en",
                        PicturePath = "picturePath/123",
                    },
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            Id = 1,
                            Rating = 5,
                            Comment = "Super Bravo!!!",
                            Student = student1,
                        },
                    },
                },
                new Teacher
                {
                    HourWage = 15.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "IVcani123",
                        Email = "Ivo@mail.en",
                        PicturePath = "picturePath12/5As24",
                    },
                    Reviews = new List<Review>
                    {
                        new Review
                        {
                            Rating = 1,
                            Id = 2,
                            Comment = "Super Bravo!!!",
                            Student = student1,
                        },
                        new Review
                        {
                            Rating = 2,
                            Id = 3,
                            Comment = "Super Bravo!!!",
                            Student = student2,
                        },
                        new Review
                        {
                            Rating = 2,
                            Id = 4,
                            Comment = "Super Bravo!!!",
                            Student = student1,
                        },
                        new Review
                        {
                            Rating = 5,
                            Id = 5,
                            Comment = "Super Bravo!!!",
                            Student = student2,
                        },
                        new Review
                        {
                            Rating = 5,
                            Id = 6,
                            Comment = "Super Bravo!!!",
                            Student = student1,
                        },
                    },
                },
                new Teacher
                {
                    HourWage = 8.54M,
                    ApplicationUser = new ApplicationUser
                    {
                        Name = "Geo123",
                        Email = "geo@abv.en",
                        PicturePath = "picturePath/44As123",
                    },
                    Reviews = new List<Review>
                    {
                    },
                },
            };
            var reviews = teachers.SelectMany(x =>
            {
                foreach (var review in x.Reviews)
                {
                    review.TeacherId = x.Id;
                }

                return x.Reviews;
            });

            this.reviews.AddRange(reviews);

            foreach (var teacher in teachers)
            {
                var resultReviews = this.service
                    .GetAllReviewsForTeacher<GetAllReviewsForTeacherTestObject>(teacher.Id)
                    .ToList();

                foreach (var review in teacher.Reviews)
                {
                    var resultReview = resultReviews.First(x => x.Id == review.Id);

                    Assert.Equal(review.Id, resultReview.Id);
                    Assert.Equal(review.Student.ApplicationUser.Email, resultReview.StudentApplicationUserEmail);
                    Assert.Equal(review.Student.ApplicationUser.Name, resultReview.StudentApplicationUserName);
                    Assert.Equal(review.Student.ApplicationUser.PicturePath, resultReview.StudentApplicationUserPicturePath);
                    Assert.Equal(review.Rating, resultReview.Rating);
                    Assert.Equal(review.Comment, resultReview.Comment);
                }
            }
        }

        [Fact]
        public void DeleteReviewAsync_Succeede()
        {
            var wantedStudent = new Student
            {
                ApplicationUserId = "wantedId",
            };

            var notWantedStudent = new Student
            {
                ApplicationUserId = "notWantedId",
            };

            const int wantedReviewId1 = 1;
            const int wantedReviewId2 = 4;
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = wantedReviewId1,
                    Student = wantedStudent,
                },
                new Review
                {
                    Id = 2,
                    Student = notWantedStudent,
                },
                new Review
                {
                    Id = 3,
                    Student = wantedStudent,
                },
                new Review
                {
                    Id = wantedReviewId2,
                    Student = new Student(),
                },
            };

            this.reviews.AddRange(reviews);

            const int notExistingReviewId = 404;
            Assert.False(this.service.DeleteReviewAsync(wantedStudent.ApplicationUserId, notExistingReviewId, false).GetAwaiter().GetResult());

            Assert.True(this.service.DeleteReviewAsync(wantedStudent.ApplicationUserId, wantedReviewId1, false).GetAwaiter().GetResult());
            Assert.DoesNotContain(this.reviews, x => x.Id == wantedReviewId1);

            Assert.True(this.service.DeleteReviewAsync(null, wantedReviewId2, true).GetAwaiter().GetResult());
            Assert.DoesNotContain(this.reviews, x => x.Id == wantedReviewId2);
        }

        public override void CleanWorkbench()
        {
            this.reviews = new List<Review>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.reviews);

            this.teachers = new List<Teacher>();

            var teachersRepository = GetMockedClasses.MockIDeletableEntityRepository(this.teachers);

            this.schoolSubjects = new List<SchoolSubject>();

            var schoolSubjectsRepository = GetMockedClasses.MockIDeletableEntityRepository(this.schoolSubjects);

            var deletedUser = new ApplicationUser
            {
                UserName = GlobalConstants.DeletedUserUsername,
            };

            var userManager = GetMockedClasses.MockUserManager<ApplicationUser>();
            userManager
                .Setup(u => u.FindByNameAsync(It.Is<string>(x => x == GlobalConstants.DeletedUserUsername)))
                .Returns(Task.FromResult(deletedUser));

            var teachersService = new TeachersService(
                teachersRepository.Object,
                schoolSubjectsRepository.Object,
                userManager.Object);

            this.service = new ReviewsService(repository.Object, teachersService);
        }
    }
}
