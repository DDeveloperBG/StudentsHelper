namespace StudentsHelper.Web.ViewModels.Meetings
{
    using System;

    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class IsStudentBalanceEnoughForMoreTimeNeededDataModel : IHaveCustomMappings
    {
        public string StudentId { get; set; }

        public decimal NextPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meeting, IsStudentBalanceEnoughForMoreTimeNeededDataModel>()
                 .ForMember(
                     x => x.StudentId,
                     opt => opt.MapFrom(src => src.Consultation.StudentId))
                 .ForMember(
                     x => x.NextPrice,
                     opt => opt.MapFrom(src => Math.Round(src.Consultation.HourWage / 60 * (src.DurationInMinutes + 1), 2)));
        }
    }
}
