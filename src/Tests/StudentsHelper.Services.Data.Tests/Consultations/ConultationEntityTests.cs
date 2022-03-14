namespace StudentsHelper.Services.Data.Tests
{
    using System;

    using StudentsHelper.Data.Models;

    using Xunit;

    public class ConultationEntityTests : BaseTest
    {
        [Fact]
        public void ConsultationEntityTest1()
        {
            var utcNow = DateTime.UtcNow;
            const string reason = "abcFBC";
            DateTime startTime = utcNow;
            DateTime endTime = utcNow.AddHours(1);
            decimal hourWage = 15;

            var consultation = new Consultation
            {
                StartTime = startTime,
                EndTime = endTime,
                HourWage = hourWage,
                Reason = reason,
            };

            Assert.Equal(startTime, consultation.StartTime);
            Assert.Equal(endTime, consultation.EndTime);
            Assert.Equal(hourWage, hourWage);
            Assert.Equal(reason, consultation.Reason);
            Assert.Equal(endTime - startTime, consultation.Duration);
            Assert.Equal(Math.Round(hourWage / 60 * (int)(endTime - startTime).TotalMinutes, 2), consultation.FullPrice);
        }

        [Fact]
        public void ConsultationEntityTest2()
        {
            var utcNow = DateTime.UtcNow;
            const string reason = "abc13FBC";
            DateTime startTime = utcNow;
            DateTime endTime = utcNow.AddMinutes(152);
            decimal hourWage = 21.4M;

            var consultation = new Consultation
            {
                StartTime = startTime,
                EndTime = endTime,
                HourWage = hourWage,
                Reason = reason,
            };

            Assert.Equal(startTime, consultation.StartTime);
            Assert.Equal(endTime, consultation.EndTime);
            Assert.Equal(hourWage, hourWage);
            Assert.Equal(reason, consultation.Reason);
            Assert.Equal(endTime - startTime, consultation.Duration);
            Assert.Equal(Math.Round(hourWage / 60 * (int)(endTime - startTime).TotalMinutes, 2), consultation.FullPrice);
        }
    }
}
