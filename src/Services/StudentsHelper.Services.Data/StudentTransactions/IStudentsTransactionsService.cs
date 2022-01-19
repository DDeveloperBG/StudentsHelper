﻿namespace StudentsHelper.Services.Data.StudentTransactions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStudentsTransactionsService
    {
        Task AddStudentTransaction(string studentId, int amount, string sessionId);

        Task MarkPaymentAsCompletedAsync(string sessionId);

        decimal GetStudentBalance(string studentId);

        decimal GetTeacherBalance(string teacherId);

        IEnumerable<T> GetStudentTransactions<T>(string studentId);

        IEnumerable<T> GetTeacherTransactions<T>(string teacherId);
    }
}
