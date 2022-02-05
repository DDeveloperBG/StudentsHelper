namespace StudentsHelper.Services.Data.StudentTransactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Meetings;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Web.ViewModels.Consultations;

    public class StudentsTransactionsService : IStudentsTransactionsService
    {
        private readonly IRepository<StudentTransaction> studentsTransactionsRepository;
        private readonly IMeetingsService meetingsService;

        public StudentsTransactionsService(
            IRepository<StudentTransaction> studentsTransactionsRepository,
            IMeetingsService meetingsService)
        {
            this.studentsTransactionsRepository = studentsTransactionsRepository;
            this.meetingsService = meetingsService;
        }

        public async Task AddStudentTransaction(string studentId, decimal amount, string sessionId)
        {
            await this.studentsTransactionsRepository.AddAsync(new StudentTransaction
            {
                StudentId = studentId,
                Amount = amount,
                SessionId = sessionId,
            });

            await this.studentsTransactionsRepository.SaveChangesAsync();
        }

        public Task MarkPaymentAsCompletedAsync(string sessionId)
        {
            var transaction = this.studentsTransactionsRepository
                .All()
                .Where(x => x.SessionId == sessionId)
                .SingleOrDefault();

            transaction.IsCompleted = true;
            transaction.PaymentDate = DateTime.UtcNow;

            return this.studentsTransactionsRepository.SaveChangesAsync();
        }

        public decimal GetStudentBalanceWithUserId(string userId)
        {
            return this.GetAllCompleted()
                .Where(x => x.Student.ApplicationUserId == userId)
                .Sum(x => x.Amount);
        }

        public decimal GetStudentBalance(string studentId)
        {
            return this.GetAllCompleted()
                .Where(x => x.StudentId == studentId || x.Consultation.StudentId == studentId)
                .Sum(x => x.Amount);
        }

        public decimal GetTeacherBalance(string teacherId)
        {
            var result = this.GetAllCompleted()
                .Where(x => x.Consultation.TeacherId == teacherId && !x.IsPaidToTeacher)
                .Sum(x => x.Amount);

            return Math.Abs(result);
        }

        public IEnumerable<T> GetStudentTransactions<T>(string studentId)
        {
            return this.GetAllCompleted()
                .Where(x => x.StudentId == studentId || x.Consultation.StudentId == studentId)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetTeacherTransactions<T>(string teacherId)
        {
            return this.GetAllCompleted()
                .Where(x => x.Consultation.TeacherId == teacherId)
                .To<T>()
                .ToList();
        }

        public Task ChargeStudentAsync(string meetingId, DateTime paymentDate)
        {
            var meetingData = this.meetingsService.GetMeetingData<ChargeStudentNeededDataModel>(meetingId);

            if (meetingData.Price <= 0)
            {
                return Task.CompletedTask;
            }

            var transaction = new StudentTransaction
            {
                Amount = meetingData.Price * -1, // So that it would be known as payment.
                PaymentDate = paymentDate,
                IsCompleted = true,
                ConsultationId = meetingData.ConsultationId,
            };

            this.studentsTransactionsRepository.AddAsync(transaction);
            return this.studentsTransactionsRepository.SaveChangesAsync();
        }

        public Task SetAsPaidTransactionsAsync(List<StudentTransaction> allPaidTransactions)
        {
            foreach (var transaction in allPaidTransactions)
            {
                transaction.IsPaidToTeacher = true;
            }

            return this.studentsTransactionsRepository.SaveChangesAsync();
        }

        private IQueryable<StudentTransaction> GetAllCompleted()
        {
            return this.studentsTransactionsRepository
                .All()
                .Where(x => x.IsCompleted);
        }
    }
}
