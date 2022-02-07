namespace StudentsHelper.Services.Data.Consulations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface IConsulationsService
    {
        IEnumerable<T> GetTeacherConsultations<T>(string teacherId, DateTime utcNow);

        IEnumerable<T> GetStudentConsultations<T>(string studentId, DateTime utcNow);

        Task<Consultation> AddConsultationAsync(DateTime startTime, DateTime endTime, decimal hourWage, string reason, int subjectId, string studentId, string teacherId);

        bool IsConsultationActive(string meetingId, string userId, DateTime utcNow);
    }
}
