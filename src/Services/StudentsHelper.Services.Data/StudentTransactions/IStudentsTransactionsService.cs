namespace StudentsHelper.Services.Data.StudentTransactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface IStudentsTransactionsService
    {
        Task AddStudentTransaction(string studentId, decimal amount, string sessionId);

        Task MarkPaymentAsCompletedAsync(string sessionId);

        decimal GetStudentBalance(string studentId);

        decimal GetTeacherBalance(string teacherId);

        IEnumerable<T> GetStudentTransactions<T>(string studentId);

        IEnumerable<T> GetTeacherTransactions<T>(string teacherId);

        Task ChargeStudentAsync(string meetingId, DateTime paymentDate);

        Task SetAsPaidTransactionsAsync(List<StudentTransaction> allPaidTransactions);
    }
}
