namespace StudentsHelper.Services.Data.StudentTransactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentsTransactionsService : IStudentsTransactionsService
    {
        private IRepository<StudentTransaction> studentsTransactionsRepository;

        public StudentsTransactionsService(
            IRepository<StudentTransaction> studentsTransactionsRepository)
        {
            this.studentsTransactionsRepository = studentsTransactionsRepository;
        }

        public async Task AddStudentTransaction(string studentId, int amount, string sessionId)
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

        public decimal GetStudentBalance(string studentId)
        {
            return this.GetAllCompleted()
                .Where(x => x.StudentId == studentId)
                .Sum(x => x.Amount);
        }

        public decimal GetTeacherBalance(string teacherId)
        {
            var result = this.GetAllCompleted()
                .Where(x => x.Consultation.TeacherId == teacherId)
                .Sum(x => x.Amount);

            return Math.Abs(result);
        }

        public IEnumerable<T> GetStudentTransactions<T>(string studentId)
        {
            return this.GetAllCompleted()
                .Where(x => x.StudentId == studentId)
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

        private IQueryable<StudentTransaction> GetAllCompleted()
        {
            return this.studentsTransactionsRepository
                .All()
                .Where(x => x.IsCompleted);
        }
    }
}
