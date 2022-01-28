namespace StudentsHelper.Web.ViewModels.Balance
{
    using System;

    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TransactionViewModel : IMapFrom<StudentTransaction>, IHaveCustomMappings
    {
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string TeacherName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<StudentTransaction, TransactionViewModel>()
                .ForMember(
                    x => x.TeacherName,
                    opt => opt.MapFrom(src => src.Consultation.Teacher.ApplicationUser.Name));
        }
    }
}
