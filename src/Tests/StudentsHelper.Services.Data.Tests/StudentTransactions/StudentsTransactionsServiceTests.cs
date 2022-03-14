namespace StudentsHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Meetings;
    using StudentsHelper.Services.Data.StudentTransactions;
    using Xunit;

    public class StudentsTransactionsServiceTests : BaseTest
    {
        private List<StudentTransaction> studentTransactions;
        private List<Meeting> meetings;
        private StudentsTransactionsService service;

        [Fact]
        public void AddStudentTransactionAsync_Succeede()
        {
            const string studentId = "x13";
            const decimal amount = 2.5M;
            const string sessionId = "x15";

            Assert.Empty(this.studentTransactions);
            this.service.AddStudentTransactionAsync(studentId, amount, sessionId).GetAwaiter().GetResult();
            Assert.Single(this.studentTransactions);

            var transaction = this.studentTransactions.First();
            Assert.Equal(studentId, transaction.StudentId);
            Assert.Equal(amount, transaction.Amount);
            Assert.Equal(sessionId, transaction.SessionId);
        }

        [Fact]
        public void MarkPaymentAsCompletedAsync_Succeede()
        {
            var utcNow = DateTime.UtcNow;
            const string studentId = "x13";
            const decimal amount = 2.5M;
            const string sessionId = "x15";

            var transaction = new StudentTransaction
            {
                StudentId = studentId,
                Amount = amount,
                SessionId = sessionId,
            };

            this.studentTransactions.Add(transaction);

            Assert.False(transaction.IsCompleted);

            this.service.MarkPaymentAsCompletedAsync(sessionId, utcNow).GetAwaiter().GetResult();

            Assert.True(transaction.IsCompleted);
            Assert.Equal(utcNow, transaction.PaymentDate);
        }

        [Fact]
        public void GetStudentBalance_Succeede()
        {
            const decimal expectedBalance = 18.2M + 2.2M - 4.3M;
            const string studentId = "1";
            var transactions = new List<StudentTransaction>
            {
                new StudentTransaction
                {
                    StudentId = "11",
                    Amount = 15.2M,
                    SessionId = "1",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "11",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "not expected",
                    Amount = 11.2M,
                    SessionId = "2",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "12",
                    },
                },
                new StudentTransaction
                {
                    StudentId = studentId,
                    Amount = 18.2M,
                    SessionId = "3",
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    StudentId = studentId,
                    Amount = 2.2M,
                    SessionId = "4",
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    StudentId = "15",
                    Amount = 3.2M,
                    SessionId = "5",
                    IsCompleted = true,
                    Consultation = new Consultation
                    {
                        StudentId = "15",
                    },
                },
                new StudentTransaction
                {
                    Amount = -4.3M,
                    SessionId = "6",
                    IsCompleted = true,
                    Consultation = new Consultation
                    {
                        StudentId = studentId,
                    },
                },
            };

            this.studentTransactions.AddRange(transactions);

            var result = this.service.GetStudentBalance(studentId);

            Assert.Equal(expectedBalance, result);
        }

        [Fact]
        public void GetTeacherBalance_Succeede()
        {
            const decimal expectedBalance = 18.2M + 5.2M + 4.3M;
            const string teacherId = "1";
            var transactions = new List<StudentTransaction>
            {
                new StudentTransaction
                {
                    Amount = -2.2M,
                    IsPaidToTeacher = false,
                    Consultation = new Consultation
                    {
                        TeacherId = "11",
                    },
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    Amount = -11.2M,
                    IsPaidToTeacher = true,
                    Consultation = new Consultation
                    {
                        TeacherId = "12",
                    },
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    Amount = -18.2M,
                    IsPaidToTeacher = false,
                    Consultation = new Consultation
                    {
                        TeacherId = teacherId,
                    },
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    Amount = -5.2M,
                    IsPaidToTeacher = false,
                    Consultation = new Consultation
                    {
                        TeacherId = teacherId,
                    },
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    Amount = -4.3M,
                    IsPaidToTeacher = true,
                    Consultation = new Consultation
                    {
                        TeacherId = teacherId,
                    },
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    Amount = -4.3M,
                    IsPaidToTeacher = false,
                    Consultation = new Consultation
                    {
                        TeacherId = teacherId,
                    },
                    IsCompleted = true,
                },
            };

            this.studentTransactions.AddRange(transactions);

            var result = this.service.GetTeacherBalance(teacherId);

            Assert.Equal(expectedBalance, result);
        }

        [Fact]
        public void GetStudentTransactions_Succeede()
        {
            const string studentId = "searched student";
            var utcNow = DateTime.UtcNow;

            var expectedTransactionsData = new List<GetTransactionsTestObject>
            {
                new GetTransactionsTestObject
                {
                    Id = "st1",
                    Amount = 32.5M,
                    PaymentDate = utcNow.AddDays(2).AddMinutes(5),
                    TeacherName = null,
                    StudentName = "Student1",
                    IsPaidToTeacher = false,
                },
                new GetTransactionsTestObject
                {
                    Id = "st2",
                    Amount = 3.16M,
                    PaymentDate = utcNow.AddMinutes(155),
                    TeacherName = "Teacher1",
                    StudentName = "Student1",
                    IsPaidToTeacher = true,
                },
                new GetTransactionsTestObject
                {
                    Id = "st3",
                    Amount = 2.15M,
                    PaymentDate = utcNow.AddMinutes(13),
                    TeacherName = "Teacher2",
                    StudentName = "Student1",
                    IsPaidToTeacher = true,
                },
            };

            var transactions = new List<StudentTransaction>
            {
                new StudentTransaction
                {
                    StudentId = "11",
                    Amount = 15.2M,
                    SessionId = "1",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "11",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "not expected",
                    Amount = 11.2M,
                    SessionId = "2",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "12",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "15",
                    Amount = 3.2M,
                    SessionId = "5",
                    IsCompleted = true,
                    Consultation = new Consultation
                    {
                        StudentId = "15",
                    },
                },
            };

            foreach (var expTransaction in expectedTransactionsData)
            {
                var transaction = new StudentTransaction
                {
                    Id = expTransaction.Id,
                    Consultation = new Consultation
                    {
                        Teacher = new Teacher
                        {
                            ApplicationUser = new ApplicationUser
                            {
                                Name = expTransaction.TeacherName,
                            },
                        },
                        StudentId = studentId,
                        Student = new Student
                        {
                            Id = studentId,
                            ApplicationUser = new ApplicationUser
                            {
                                Name = expTransaction.StudentName,
                            },
                        },
                    },
                    PaymentDate = expTransaction.PaymentDate,
                    Amount = expTransaction.Amount,
                    IsPaidToTeacher = expTransaction.IsPaidToTeacher,
                    IsCompleted = true,
                };

                transactions.Add(transaction);
            }

            this.studentTransactions.AddRange(transactions);

            var result = this.service.GetStudentTransactions<GetTransactionsTestObject>(studentId);

            Assert.Equal(expectedTransactionsData.Count, result.Count());
            foreach (var item in result)
            {
                var expectedTransaction = expectedTransactionsData.Find(x => x.Id == item.Id);

                Assert.NotNull(expectedTransaction);
                Assert.Equal(expectedTransaction.Id, item.Id);
                Assert.Equal(expectedTransaction.Amount, item.Amount);
                Assert.Equal(expectedTransaction.PaymentDate, item.PaymentDate);
                Assert.Equal(expectedTransaction.TeacherName, item.TeacherName);
                Assert.Equal(expectedTransaction.StudentName, item.StudentName);
                Assert.Equal(expectedTransaction.IsPaidToTeacher, item.IsPaidToTeacher);
            }
        }

        [Fact]
        public void GetTeacherTransactions_Succeede()
        {
            const string teacherId = "searched teacher";
            var utcNow = DateTime.UtcNow;

            var expectedTransactionsData = new List<GetTransactionsTestObject>
            {
                new GetTransactionsTestObject
                {
                    Id = "st1",
                    Amount = 32.5M,
                    PaymentDate = utcNow.AddDays(2).AddMinutes(5),
                    TeacherName = "Gosho",
                    StudentName = "Student1",
                    IsPaidToTeacher = false,
                },
                new GetTransactionsTestObject
                {
                    Id = "st2",
                    Amount = 3.16M,
                    PaymentDate = utcNow.AddMinutes(155),
                    TeacherName = "Teacher1",
                    StudentName = "Student1",
                    IsPaidToTeacher = true,
                },
                new GetTransactionsTestObject
                {
                    Id = "st3",
                    Amount = 2.15M,
                    PaymentDate = utcNow.AddMinutes(13),
                    TeacherName = "Teacher2",
                    StudentName = "Student1",
                    IsPaidToTeacher = true,
                },
            };

            var transactions = new List<StudentTransaction>
            {
                new StudentTransaction
                {
                    StudentId = "11",
                    Amount = 15.2M,
                    SessionId = "1",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "11",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "not expected",
                    Amount = 11.2M,
                    SessionId = "2",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "12",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "15",
                    Amount = 3.2M,
                    SessionId = "5",
                    IsCompleted = true,
                    Consultation = new Consultation
                    {
                        StudentId = "15",
                    },
                },
            };

            foreach (var expTransaction in expectedTransactionsData)
            {
                var transaction = new StudentTransaction
                {
                    Id = expTransaction.Id,
                    Consultation = new Consultation
                    {
                        TeacherId = teacherId,
                        Teacher = new Teacher
                        {
                            ApplicationUser = new ApplicationUser
                            {
                                Name = expTransaction.TeacherName,
                            },
                        },
                        Student = new Student
                        {
                            ApplicationUser = new ApplicationUser
                            {
                                Name = expTransaction.StudentName,
                            },
                        },
                    },
                    PaymentDate = expTransaction.PaymentDate,
                    Amount = expTransaction.Amount,
                    IsPaidToTeacher = expTransaction.IsPaidToTeacher,
                    IsCompleted = true,
                };

                transactions.Add(transaction);
            }

            this.studentTransactions.AddRange(transactions);

            var result = this.service.GetTeacherTransactions<GetTransactionsTestObject>(teacherId);

            Assert.Equal(expectedTransactionsData.Count, result.Count());
            foreach (var item in result)
            {
                var expectedTransaction = expectedTransactionsData.Find(x => x.Id == item.Id);

                Assert.NotNull(expectedTransaction);
                Assert.Equal(expectedTransaction.Id, item.Id);
                Assert.Equal(expectedTransaction.Amount, item.Amount);
                Assert.Equal(expectedTransaction.PaymentDate, item.PaymentDate);
                Assert.Equal(expectedTransaction.TeacherName, item.TeacherName);
                Assert.Equal(expectedTransaction.StudentName, item.StudentName);
                Assert.Equal(expectedTransaction.IsPaidToTeacher, item.IsPaidToTeacher);
            }
        }

        [Fact]
        public void ChargeStudentAsync_Succeede()
        {
            // Test meetings
            this.meetings.Add(new Meeting());
            this.meetings.Add(new Meeting());
            this.meetings.Add(new Meeting());

            DateTime paymentDate = DateTime.UtcNow;
            var meeting = new Meeting
            {
                HasStarted = true,
                DurationInMinutes = 5,
                Consultation = new Consultation
                {
                    HourWage = 15.5M,
                },
            };
            this.meetings.Add(meeting);

            this.service.ChargeStudentAsync(meeting.Id, paymentDate).GetAwaiter().GetResult();

            Assert.Single(this.studentTransactions);
            var addedTransaction = this.studentTransactions.First();

            var expectedPrice = Math.Round(meeting.Consultation.HourWage / 60 * meeting.DurationInMinutes, 2) * -1;
            Assert.Equal(expectedPrice, addedTransaction.Amount);
            Assert.Equal(paymentDate, addedTransaction.PaymentDate);
            Assert.True(addedTransaction.IsCompleted);
            Assert.Equal(meeting.Consultation.Id, addedTransaction.ConsultationId);
        }

        [Fact]
        public void SetAsPaidTransactionsAsync_Succeede()
        {
            var transactions = new List<StudentTransaction>
            {
                new StudentTransaction
                {
                    StudentId = "11",
                    Amount = 15.2M,
                    SessionId = "1",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "11",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "not expected",
                    Amount = 11.2M,
                    SessionId = "2",
                    IsCompleted = false,
                    Consultation = new Consultation
                    {
                        StudentId = "12",
                    },
                },
                new StudentTransaction
                {
                    StudentId = "2",
                    Amount = 18.2M,
                    SessionId = "3",
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    StudentId = "2",
                    Amount = 2.2M,
                    SessionId = "4",
                    IsCompleted = true,
                },
                new StudentTransaction
                {
                    StudentId = "15",
                    Amount = 3.2M,
                    SessionId = "5",
                    IsCompleted = true,
                    Consultation = new Consultation
                    {
                        StudentId = "15",
                    },
                },
                new StudentTransaction
                {
                    Amount = -4.3M,
                    SessionId = "6",
                    IsCompleted = true,
                    Consultation = new Consultation
                    {
                        StudentId = "2",
                    },
                },
            };
            var usedTransactions = transactions.Take(3).ToList();

            this.studentTransactions.AddRange(transactions);

            this.service.SetAsPaidTransactionsAsync(usedTransactions).GetAwaiter().GetResult();

            foreach (var transaction in transactions)
            {
                if (usedTransactions.Contains(transaction))
                {
                    Assert.True(transaction.IsPaidToTeacher);
                }
                else
                {
                    Assert.False(transaction.IsPaidToTeacher);
                }
            }
        }

        public override void CleanWorkbench()
        {
            this.studentTransactions = new List<StudentTransaction>();

            var studentTransactionsRepository = GetMockedClasses.MockIRepository(this.studentTransactions);

            this.meetings = new List<Meeting>();

            var meetingRepository = GetMockedClasses.MockIRepository(this.meetings);

            var meetingsService = new MeetingsService(meetingRepository.Object);

            this.service
                = new StudentsTransactionsService(studentTransactionsRepository.Object, meetingsService);
        }
    }
}
