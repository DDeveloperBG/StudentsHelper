namespace StudentsHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;

    using Hangfire;
    using Hangfire.MemoryStorage;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Meetings;
    using Xunit;

    public class MeetingsServiceTests : BaseTest
    {
        private List<Meeting> meetings;
        private MeetingsService meetingsService;

        [Fact]
        public void UpdateParticipantActivityAndIncreaseDurationAsync_SucceedForStudent()
        {
            const string role = GlobalConstants.StudentRoleName;
            const int consultationDuration = 10;
            var utcNow = DateTime.UtcNow;

            var consultation = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddMinutes(consultationDuration),
            };

            var meeting = new Meeting
            {
                Consultation = consultation,
            };

            this.meetings.Add(meeting);

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    role,
                    meeting.Id,
                    utcNow,
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(0, meeting.DurationInMinutes);
            Assert.Equal(utcNow, meeting.StudentLastActivity);
            Assert.False(meeting.HasStarted);
        }

        [Fact]
        public void UpdateParticipantActivityAndIncreaseDurationAsync_SucceedForTeacher()
        {
            const string role = GlobalConstants.TeacherRoleName;
            const int consultationDuration = 10;
            var utcNow = DateTime.UtcNow;

            var consultation = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddMinutes(consultationDuration),
            };

            var meeting = new Meeting
            {
                Consultation = consultation,
            };

            this.meetings.Add(meeting);

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    role,
                    meeting.Id,
                    utcNow,
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(0, meeting.DurationInMinutes);
            Assert.Equal(utcNow, meeting.TeacherLastActivity);
            Assert.False(meeting.HasStarted);
        }

        [Fact]
        public void UpdateParticipantActivityAndIncreaseDurationAsync_SucceedForStudentAndTeacher()
        {
            const int consultationDuration = 10;
            var utcNow = DateTime.UtcNow;

            var consultation = new Consultation
            {
                StartTime = utcNow,
                EndTime = utcNow.AddMinutes(consultationDuration),
            };

            var meeting = new Meeting
            {
                Consultation = consultation,
            };

            this.meetings.Add(meeting);

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    GlobalConstants.StudentRoleName,
                    meeting.Id,
                    utcNow,
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(0, meeting.DurationInMinutes);
            Assert.Equal(utcNow, meeting.StudentLastActivity);
            Assert.False(meeting.HasStarted);

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    GlobalConstants.TeacherRoleName,
                    meeting.Id,
                    utcNow,
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(0, meeting.DurationInMinutes);
            Assert.Equal(utcNow, meeting.TeacherLastActivity);
            Assert.True(meeting.HasStarted);
            meeting.HasStarted = false;

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    GlobalConstants.StudentRoleName,
                    meeting.Id,
                    utcNow,
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(0, meeting.DurationInMinutes);
            Assert.Equal(utcNow, meeting.StudentLastActivity);
            Assert.True(meeting.HasStarted);

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    GlobalConstants.TeacherRoleName,
                    meeting.Id,
                    utcNow.AddSeconds(40),
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(0, meeting.DurationInMinutes);
            Assert.Equal(utcNow.AddSeconds(40), meeting.TeacherLastActivity);
            Assert.True(meeting.HasStarted);

            meeting.TeacherLastActivity = utcNow;

            this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    GlobalConstants.TeacherRoleName,
                    meeting.Id,
                    utcNow.AddMinutes(1),
                    () => this.EmptyMethod()).GetAwaiter().GetResult();

            Assert.Equal(1, meeting.DurationInMinutes);
            Assert.Equal(utcNow.AddMinutes(1), meeting.TeacherLastActivity);
            Assert.True(meeting.HasStarted);
        }

        [Fact]
        public void GetMeetingData_ReturnsMappedObject()
        {
            var meeting = new Meeting();

            this.meetings.Add(meeting);

            var result = this.meetingsService.GetMeetingData<GetMeetingDataTestClass>(meeting.Id);

            Assert.Equal(meeting.Id, result.Id);
        }

        public void EmptyMethod()
        {
        }

        public override void CleanWorkbench()
        {
            JobStorage.Current = new MemoryStorage();

            this.meetings = new List<Meeting>();

            var repository = GetMockedClasses.MockIRepository(this.meetings);

            this.meetingsService = new MeetingsService(repository.Object);
        }
    }
}
