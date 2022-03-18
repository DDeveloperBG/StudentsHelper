namespace StudentsHelper.Services.BusinessLogic.Consultations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Consultations;

    public interface IConsultationsBusinessLogicService
    {
        BookConsultationInputModel GetBookConsultationViewModel(string teacherId);

        Task<string> BookConsultation(BookConsultationInputModel inputModel, string userId, int subjectId);

        bool DoesStudentHasEnoughMoney(string teacherId, string studentUserId, int timeInMinutes);

        IOrderedEnumerable<StudentConsultationViewModel> GetStudentConsultationsViewModel(string userId);

        IOrderedEnumerable<TeacherConsultationViewModel> GetTeacherConsultationsViewModel(string userId);

        IEnumerable<ConsultationCalendarEventViewModel> GetCalendarConsultationsAsync(string userId, bool isTeacher, bool isStudent);

        Task<bool> ChangeConsultationStartTimeAsync(UpdateConsultationInputModel input);
    }
}
