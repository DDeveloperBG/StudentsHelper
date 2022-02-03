namespace StudentsHelper.Services.Data.Consulations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConsulationsService
    {
        IEnumerable<T> GetTeacherConsultations<T>(string teacherId);

        IEnumerable<T> GetStudentConsultations<T>(string studentId);

        Task AddConsultationAsync(DateTime startTime, DateTime endTime, decimal hourWage, string reason, int subjectId, string studentId, string teacherId);

        bool IsConsultationActive(string meetingId, string userId);
    }
}
