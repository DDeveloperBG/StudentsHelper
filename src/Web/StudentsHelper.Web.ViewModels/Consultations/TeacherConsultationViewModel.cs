namespace StudentsHelper.Web.ViewModels.Consultations
{
    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeacherConsultationViewModel : IHaveCustomMappings
    {
        public ConsultationViewModel ConsultationDetails { get; set; }

        public string Reason { get; set; }

        public string StudentEmail { get; set; }

        public string StudentName { get; set; }

        public string StudentPicturePath { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Consultation, TeacherConsultationViewModel>()
                .ForMember(
                    x => x.ConsultationDetails,
                    opt => opt.MapFrom(src => src))
                .ForMember(
                    x => x.StudentEmail,
                    opt => opt.MapFrom(src => src.Student.ApplicationUser.Email))
                .ForMember(
                    x => x.StudentName,
                    opt => opt.MapFrom(src => src.Student.ApplicationUser.Name))
                .ForMember(
                    x => x.StudentPicturePath,
                    opt => opt.MapFrom(src => src.Student.ApplicationUser.PicturePath));
        }
    }
}
