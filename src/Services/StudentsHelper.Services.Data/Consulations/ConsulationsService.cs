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
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;

        public ConsulationsService(IDeletableEntityRepository<Consultation> consultationsRepository)
        {
            this.consultationsRepository = consultationsRepository;
        }

        public IEnumerable<T> GetTeacherConsultations<T>(string teacherId)
        {
            return this.consultationsRepository
                .AllAsNoTracking()
                .Where(x => x.TeacherId == teacherId && x.EndTime > DateTime.UtcNow)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetStudentConsultations<T>(string studentId)
        {
            return this.consultationsRepository
                .AllAsNoTracking()
                .Where(x => x.StudentId == studentId && x.EndTime > DateTime.UtcNow)
                .To<T>()
                .ToList();
        }

        public async Task AddConsultationAsync(DateTime startTime, DateTime endTime, decimal hourWage, string reason, int subjectId, string studentId, string teacherId)
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
        }

        public bool IsConsultationActive(string meetingId, string userId)
        {
            var now = DateTime.UtcNow;

            var count = this.consultationsRepository
                .All()
                .Where(x =>
                    x.MeetingId == meetingId
                    && (x.Teacher.ApplicationUserId == userId || x.Student.ApplicationUserId == userId)
                    && (now >= x.StartTime && now <= x.EndTime))
                .Count();

            return count == 1;
        }
    }
}
