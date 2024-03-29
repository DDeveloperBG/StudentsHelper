﻿namespace StudentsHelper.Services.Data.Ratings
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Ratings.Models;

    public interface IReviewsService
    {
        IQueryable<TeacherWithRating> GetTeachersRating(IQueryable<Teacher> teachers);

        TeacherDetails GetTeacherRating(string teacherId);

        Task AddReviewAsync(string teacherId, string studentId, byte rating, string comment);

        bool HasStudentReviewedTeacher(string studentId, string teacherId);

        IQueryable<T> GetAllReviewsForTeacher<T>(string teacherId);

        Task<bool> DeleteReviewAsync(string userId, int reviewId, bool isAdmin);
    }
}
