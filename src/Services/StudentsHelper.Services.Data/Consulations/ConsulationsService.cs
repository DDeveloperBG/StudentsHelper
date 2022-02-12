namespace StudentsHelper.Services.Data.Consulations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class ConsulationsService : IConsulationsService
    {
        private readonly IRepository<Consultation> consultationsRepository;

        public ConsulationsService(IRepository<Consultation> consultationsRepository)
        {
            this.consultationsRepository = consultationsRepository;
        }

        public IEnumerable<T> GetTeacherConsultations<T>(string teacherId, DateTime utcNow)
        {
            return this.consultationsRepository
                .AllAsNoTracking()
                .Where(x => x.TeacherId == teacherId && x.EndTime > utcNow)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetStudentConsultations<T>(string studentId, DateTime utcNow)
        {
            return this.consultationsRepository
                .AllAsNoTracking()
                .Where(x => x.StudentId == studentId && x.EndTime > utcNow)
                .To<T>()
                .ToList();
        }

        public async Task<Consultation> AddConsultationAsync(DateTime startTime, DateTime endTime, decimal hourWage, string reason, int subjectId, string studentId, string teacherId)
        {
            var consulation = new Consultation
            {
                StartTime = startTime,
                EndTime = endTime,
                HourWage = hourWage,
                Reason = reason,
                Meeting = new Meeting(),
                SchoolSubjectId = subjectId,
                StudentId = studentId,
                TeacherId = teacherId,
            };
            await this.consultationsRepository.AddAsync(consulation);
            await this.consultationsRepository.SaveChangesAsync();
            return consulation;
        }

        public bool IsConsultationActive(string meetingId, string userId, DateTime utcNow)
        {
            var count = this.consultationsRepository
                .AllAsNoTracking()
                .Where(x =>
                    x.MeetingId == meetingId
                    && (x.Teacher.ApplicationUserId == userId || x.Student.ApplicationUserId == userId)
                    && (utcNow >= x.StartTime && utcNow <= x.EndTime))
                .Count();

            return count == 1;
        }

        public Task UpdateConsultationStartTimeAsync(string consultationId, DateTime startTime)
        {
            var consultation = this.GetTrackedConsultation(consultationId);

            consultation.EndTime = startTime + consultation.Duration;
            consultation.StartTime = startTime;

            return this.consultationsRepository.SaveChangesAsync();
        }

        private Consultation GetTrackedConsultation(string consultationId)
        {
            return this.consultationsRepository
                .All()
                .Where(x => x.Id == consultationId)
                .Single();
        }
    }
}
