namespace StudentsHelper.Web.ViewModels.Payments
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeachersAndMonthTransactions : IHaveCustomMappings
    {
        public string TeacherConnectedAccountId { get; set; }

        public IEnumerable<StudentTransaction> CurrentMonthTransactions { get; set; }

        public decimal MonthSalary => this.CurrentMonthTransactions.Sum(y => y.Amount);

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Teacher, TeachersAndMonthTransactions>()
                 .ForMember(
                    x => x.TeacherConnectedAccountId,
                    opt => opt.MapFrom(src => src.ExpressConnectedAccountId))
                 .ForMember(
                    x => x.CurrentMonthTransactions,
                    opt => opt.MapFrom(src => src.Consultations
                        .Select(x => x.StudentTransaction)
                        .Where(x => !x.IsPaidToTeacher && x.Amount != 0)));
        }
    }
}
