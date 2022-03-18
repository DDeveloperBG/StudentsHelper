namespace StudentsHelper.Services.BusinessLogic.Consultations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Common;
    using StudentsHelper.Services.Data.Consulations;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.ViewModels.Consultations;

    public class ConsultationsBusinessLogicService : IConsultationsBusinessLogicService
    {
        private readonly IConsulationsService consultationsService;
        private readonly IStudentsService studentsService;
        private readonly ITeachersService teachersService;
        private readonly IStudentsTransactionsService studentsTransactionsService;
        private readonly IDateTimeProvider dateTimeProvider;

        public ConsultationsBusinessLogicService(
            IConsulationsService consultationsService,
            IStudentsService studentsService,
            ITeachersService teachersService,
            IStudentsTransactionsService studentsTransactionsService,
            IDateTimeProvider dateTimeProvider)
        {
            this.consultationsService = consultationsService;
            this.studentsService = studentsService;
            this.teachersService = teachersService;
            this.studentsTransactionsService = studentsTransactionsService;
            this.dateTimeProvider = dateTimeProvider;
        }

        public BookConsultationInputModel GetBookConsultationViewModel(string teacherId)
        {
            return new BookConsultationInputModel
            {
                TeacherId = teacherId,
                StartTime = this.dateTimeProvider.GetUtcNow(),
            };
        }

        public async Task<string> BookConsultation(BookConsultationInputModel inputModel, string userId, int subjectId)
        {
            var minDuration = ValidationConstants.Consultation.MinDuration;
            var maxDuration = ValidationConstants.Consultation.MaxDuration;

            if (!(inputModel.Duration >= minDuration && inputModel.Duration <= maxDuration))
            {
                return GlobalConstants.ConsultationMessages.DurationLength;
            }

            var utcNow = this.dateTimeProvider.GetUtcNow();
            if ((utcNow - inputModel.StartTime).TotalMilliseconds > 0)
            {
                return GlobalConstants.ConsultationMessages.StartTime;
            }

            var hourWage = this.teachersService.GetHourWage(inputModel.TeacherId);

            if (hourWage == null)
            {
                return GlobalConstants.ConsultationMessages.NullHourWage;
            }

            var studentId = this.studentsService.GetId(userId);

            bool hasEnoughMoney = this.DoesStudentHasEnoughMoney(
                inputModel.TeacherId,
                studentId,
                (int)inputModel.Duration.TotalMinutes);

            if (!hasEnoughMoney)
            {
                return GlobalConstants.PaymentMessages.InsufficientBalanceMessage;
            }

            var endTime = inputModel.StartTime + inputModel.Duration;

            await this
                .consultationsService
                .AddConsultationAsync(
                    inputModel.StartTime,
                    endTime,
                    hourWage.Value,
                    inputModel.Reason,
                    subjectId,
                    studentId,
                    inputModel.TeacherId);

            return null;
        }

        public bool DoesStudentHasEnoughMoney(string teacherId, string studentUserId, int timeInMinutes)
        {
            var hourWage = this.teachersService.GetHourWage(teacherId);
            if (hourWage == null)
            {
                return false;
            }

            string studentId = this.studentsService.GetId(studentUserId);

            var studentBalance = this.studentsTransactionsService.GetStudentBalance(studentId);
            var neededBalance = hourWage / 60 * timeInMinutes;

            return studentBalance > 0 && studentBalance >= neededBalance;
        }

        public IOrderedEnumerable<StudentConsultationViewModel> GetStudentConsultationsViewModel(string userId)
        {
            var studentId = this.studentsService.GetId(userId);

            var viewModel = this
                .consultationsService
                .GetStudentConsultations<StudentConsultationViewModel>(
                    studentId,
                    this.dateTimeProvider.GetUtcNow())
                .OrderBy(x => x.ConsultationDetails.StartTime);

            return viewModel;
        }

        public IOrderedEnumerable<TeacherConsultationViewModel> GetTeacherConsultationsViewModel(string userId)
        {
            var teacherId = this.teachersService.GetId(userId);

            var viewModel = this
                .consultationsService
                .GetTeacherConsultations<TeacherConsultationViewModel>(
                    teacherId,
                    this.dateTimeProvider.GetUtcNow())
                .OrderBy(x => x.ConsultationDetails.StartTime);

            return viewModel;
        }

        public IEnumerable<ConsultationCalendarEventViewModel> GetCalendarConsultationsAsync(string userId, bool isTeacher, bool isStudent)
        {
            IEnumerable<ConsultationCalendarEventViewModel> consultations;
            var utcNow = this.dateTimeProvider.GetUtcNow();
            if (isTeacher)
            {
                var teacherId = this.teachersService.GetId(userId);
                consultations = this.consultationsService.GetTeacherConsultations<ConsultationCalendarEventViewModel>(teacherId, utcNow);
            }
            else if (isStudent)
            {
                var studentId = this.studentsService.GetId(userId);
                consultations = this.consultationsService.GetStudentConsultations<ConsultationCalendarEventViewModel>(studentId, utcNow);
            }
            else
            {
                return null;
            }

            return consultations;
        }

        public async Task<bool> ChangeConsultationStartTimeAsync(UpdateConsultationInputModel input)
        {
            var utcNow = this.dateTimeProvider.GetUtcNow();
            if (input.StartTime < utcNow)
            {
                return false;
            }

            try
            {
                await this
                    .consultationsService
                    .UpdateConsultationStartTimeAsync(
                        input.ConsultationId,
                        input.StartTime);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
