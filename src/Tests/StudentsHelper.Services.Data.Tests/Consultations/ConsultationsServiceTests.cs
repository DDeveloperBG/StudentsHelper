namespace StudentsHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Consulations;

    using Xunit;

    public class ConsultationsServiceTests : BaseTest
    {
        private List<Consultation> consultations;
        private ConsulationsService consulationsService;

        [Fact]
        public void GetTeacherConsultations_ReturnsCollectionOfMappedObjects_1()
        {
            var utcNow = DateTime.UtcNow;
            const string teacherId = "1";
            var consultation1 = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddHours(1),
            };
            var consultation2 = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddHours(2),
                TeacherId = teacherId,
            };
            this.consultations.Add(consultation1);
            this.consultations.Add(consultation2);

            var result = this.consulationsService.GetTeacherConsultations<GetConsultationsTestObject>(teacherId, utcNow);

            Assert.Single(result);
            var obj = result.First();
            Assert.Equal(consultation2.Id, obj.Id);
            Assert.Equal(teacherId, obj.TeacherId);
            Assert.Equal(consultation2.StartTime, obj.StartTime);
            Assert.Equal(consultation2.EndTime, obj.EndTime);
        }

        [Fact]
        public void GetTeacherConsultations_ReturnsCollectionOfMappedObjects_2()
        {
            var utcNow = DateTime.UtcNow;
            const string teacherId = "1";
            var consultation1 = new Consultation
            {
                StartTime = utcNow.AddHours(-1),
                EndTime = utcNow,
                TeacherId = teacherId,
            };
            var consultation2 = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddHours(2),
                TeacherId = teacherId,
            };
            this.consultations.Add(consultation1);
            this.consultations.Add(consultation2);

            var result = this.consulationsService.GetTeacherConsultations<GetConsultationsTestObject>(teacherId, utcNow);

            Assert.Single(result);
            var obj = result.First();
            Assert.Equal(consultation2.Id, obj.Id);
            Assert.Equal(teacherId, obj.TeacherId);
            Assert.Equal(consultation2.StartTime, obj.StartTime);
            Assert.Equal(consultation2.EndTime, obj.EndTime);
        }

        [Fact]
        public void GetStudentConsultations_ReturnsCollectionOfMappedObjects_1()
        {
            var utcNow = DateTime.UtcNow;
            const string studentId = "1";
            var consultation1 = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddHours(1),
            };
            var consultation2 = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddHours(2),
                StudentId = studentId,
            };
            this.consultations.Add(consultation1);
            this.consultations.Add(consultation2);

            var result = this.consulationsService.GetStudentConsultations<GetConsultationsTestObject>(studentId, utcNow);

            Assert.Single(result);
            var obj = result.First();
            Assert.Equal(consultation2.Id, obj.Id);
            Assert.Equal(studentId, obj.StudentId);
            Assert.Equal(consultation2.StartTime, obj.StartTime);
            Assert.Equal(consultation2.EndTime, obj.EndTime);
        }

        [Fact]
        public void GetStudentConsultations_ReturnsCollectionOfMappedObjects_2()
        {
            var utcNow = DateTime.UtcNow;
            const string studentId = "1";
            var consultation1 = new Consultation
            {
                StartTime = utcNow.AddHours(-1),
                EndTime = utcNow,
                StudentId = studentId,
            };
            var consultation2 = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddHours(2),
                StudentId = studentId,
            };
            this.consultations.Add(consultation1);
            this.consultations.Add(consultation2);

            var result = this.consulationsService.GetStudentConsultations<GetConsultationsTestObject>(studentId, utcNow);

            Assert.Single(result);
            var obj = result.First();
            Assert.Equal(consultation2.Id, obj.Id);
            Assert.Equal(studentId, obj.StudentId);
            Assert.Equal(consultation2.StartTime, obj.StartTime);
            Assert.Equal(consultation2.EndTime, obj.EndTime);
        }

        [Fact]
        public void AddConsultationAsync_AddsConsultation()
        {
            DateTime startTime = DateTime.UtcNow;
            DateTime endTime = DateTime.UtcNow.AddMinutes(10);
            decimal hourWage = 12.5M;
            string reason = "Azz 1223";
            int subjectId = 1;
            string studentId = "1";
            string teacherId = "2";

            var consultation = this.consulationsService
                .AddConsultationAsync(startTime, endTime, hourWage, reason, subjectId, studentId, teacherId)
                .GetAwaiter()
                .GetResult();

            var consultationId = this.consultations.First().Id;
            Assert.Equal(consultationId, consultation.Id);

            Assert.Equal(consultation.StartTime, startTime);
            Assert.Equal(consultation.EndTime, endTime);
            Assert.Equal(consultation.HourWage, hourWage);
            Assert.Equal(consultation.Reason, reason);
            Assert.Equal(consultation.SchoolSubjectId, subjectId);
            Assert.Equal(consultation.StudentId, studentId);
            Assert.Equal(consultation.TeacherId, teacherId);
        }

        [Fact]
        public void IsConsultationActive_ReturnsTrue_SearchForStudent()
        {
            var utcNow = DateTime.UtcNow;
            const string meetingId = "#1#";
            const string studentUserId = "1";
            var consultation = new Consultation
            {
                MeetingId = meetingId,
                Student = new Student
                {
                    ApplicationUserId = studentUserId,
                },
                Teacher = new Teacher
                {
                    ApplicationUserId = "1-1",
                },
                StartTime = utcNow.AddHours(-1),
                EndTime = utcNow.AddHours(1),
            };
            this.consultations.Add(consultation);

            var result = this.consulationsService.IsConsultationActive(meetingId, studentUserId, utcNow);
            Assert.True(result);
        }

        [Fact]
        public void IsConsultationActive_ReturnsFalse_SearchForStudent()
        {
            var utcNow = DateTime.UtcNow;
            const string meetingId = "#1#";
            const string studentUserId = "1";
            var consultation = new Consultation
            {
                MeetingId = meetingId,
                Student = new Student
                {
                    ApplicationUserId = studentUserId,
                },
                Teacher = new Teacher
                {
                    ApplicationUserId = "1-1",
                },
                StartTime = utcNow.AddHours(-1),
                EndTime = utcNow.AddSeconds(-1),
            };
            this.consultations.Add(consultation);

            var result = this.consulationsService.IsConsultationActive(meetingId, studentUserId, utcNow);
            Assert.False(result);
        }

        [Fact]
        public void IsConsultationActive_ReturnsTrue_SearchForTeacher()
        {
            var utcNow = DateTime.UtcNow;
            const string meetingId = "#1#";
            const string teacherUserId = "1";
            var consultation = new Consultation
            {
                MeetingId = meetingId,
                Student = new Student
                {
                    ApplicationUserId = "1-1",
                },
                Teacher = new Teacher
                {
                    ApplicationUserId = teacherUserId,
                },
                StartTime = utcNow.AddHours(-1),
                EndTime = utcNow.AddHours(1),
            };
            this.consultations.Add(consultation);

            var result = this.consulationsService.IsConsultationActive(meetingId, teacherUserId, utcNow);
            Assert.True(result);
        }

        [Fact]
        public void IsConsultationActive_ReturnsFalse_SearchForTeacher()
        {
            var utcNow = DateTime.UtcNow;
            const string meetingId = "#1#";
            const string teacherUserId = "1";
            var consultation = new Consultation
            {
                MeetingId = meetingId,
                Student = new Student
                {
                    ApplicationUserId = "1-1",
                },
                Teacher = new Teacher
                {
                    ApplicationUserId = teacherUserId,
                },
                StartTime = utcNow.AddHours(-1),
                EndTime = utcNow.AddSeconds(-1),
            };
            this.consultations.Add(consultation);

            var result = this.consulationsService.IsConsultationActive(meetingId, teacherUserId, utcNow);
            Assert.False(result);
        }

        [Fact]
        public void UpdateConsultationStartTimeAsync_Succeeds()
        {
            var utcNow = DateTime.UtcNow;
            var consultation = new Consultation
            {
                Meeting = new Meeting
                {
                    HasStarted = false,
                },
                StartTime = utcNow,
                EndTime = utcNow.AddHours(1),
            };
            this.consultations.Add(consultation);

            var duration = consultation.Duration;
            var newStartTime = utcNow.AddHours(1);
            this.consulationsService.UpdateConsultationStartTimeAsync(consultation.Id, newStartTime);

            Assert.Equal(newStartTime, consultation.StartTime);
            Assert.Equal(duration, consultation.Duration);
        }

        [Fact]
        public void UpdateConsultationStartTimeAsync_Fails()
        {
            Assert.Throws<InvalidOperationException>(() => this.consulationsService.UpdateConsultationStartTimeAsync("2", DateTime.UtcNow).GetAwaiter().GetResult());

            var consultation = new Consultation
            {
                Meeting = new Meeting
                {
                    HasStarted = true,
                },
            };
            this.consultations.Add(consultation);

            Assert.Throws<ArgumentException>(() => this.consulationsService.UpdateConsultationStartTimeAsync(consultation.Id, DateTime.UtcNow).GetAwaiter().GetResult());
        }

        public override void CleanWorkbench()
        {
            this.consultations = new List<Consultation>();

            var repository = GetMockedClasses.MockIRepository(this.consultations);

            this.consulationsService = new ConsulationsService(repository.Object);
        }
    }
}
