namespace StudentsHelper.Web.ViewModels.Consultations
{
    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class ConsultationCalendarEventViewModel : IMapFrom<Consultation>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Consultation, ConsultationCalendarEventViewModel>()
                .ForMember(
                     x => x.Description,
                     opt => opt.MapFrom(src => src.Reason))
                .ForMember(
                     x => x.StartTime,
                     opt => opt.MapFrom(src => src.StartTime.ToString("yyyy-MM-ddTHH:mm")))
                .ForMember(
                     x => x.EndTime,
                     opt => opt.MapFrom(src => src.EndTime.ToString("yyyy-MM-ddTHH:mm")));
        }
    }
}
