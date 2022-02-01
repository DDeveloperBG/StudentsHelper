namespace StudentsHelper.Web.ViewModels.Consultations
{
    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentConsultationViewModel : IHaveCustomMappings
    {
        public ConsultationViewModel ConsultationDetails { get; set; }

        public string TeacherEmail { get; set; }

        public string TeacherName { get; set; }

        public string TeacherPicturePath { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Consultation, StudentConsultationViewModel>()
                .ForMember(
                    x => x.ConsultationDetails,
                    opt => opt.MapFrom(src => src))
                 .ForMember(
                    x => x.TeacherEmail,
                    opt => opt.MapFrom(src => src.Teacher.ApplicationUser.Email))
                .ForMember(
                    x => x.TeacherName,
                    opt => opt.MapFrom(src => src.Teacher.ApplicationUser.Name))
                .ForMember(
                    x => x.TeacherPicturePath,
                    opt => opt.MapFrom(src => src.Teacher.ApplicationUser.PicturePath));
        }
    }
}
