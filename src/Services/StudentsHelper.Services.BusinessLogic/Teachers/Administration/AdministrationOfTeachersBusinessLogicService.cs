namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Subjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public class AdministrationOfTeachersBusinessLogicService : IAdministrationOfTeachersBusinessLogicService
    {
        private readonly ITeachersService teachersService;
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationOfTeachersBusinessLogicService(
            ITeachersService teachersService,
            ISchoolSubjectsService schoolSubjectsService,
            ICloudStorageService cloudStorageService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager)
        {
            this.teachersService = teachersService;
            this.schoolSubjectsService = schoolSubjectsService;
            this.cloudStorageService = cloudStorageService;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public IEnumerable<NotDetailedTeacherViewModel> GetAllToApproveViewModel()
        {
            var teachers = this
                .teachersService
                .GetAllNotConfirmed<NotDetailedTeacherViewModel>()
                .Where(x =>
                {
                    GlobalVariables.TeachersConnectedAccountStatus.TryGetValue(x.ApplicationUserEmail, out bool status);
                    return status;
                });

            foreach (var teacher in teachers)
            {
                teacher.ApplicationUserPicturePath
                    = this.cloudStorageService.GetImageUri(teacher.ApplicationUserPicturePath);
            }

            return teachers;
        }

        public TeacherDetailsViewModel GetSetTeacherDataViewModel(string teacherId)
        {
            var teacherData = this.teachersService.GetOne<TeacherDetailsViewModel>(teacherId);
            teacherData.Subjects = this.schoolSubjectsService.GetAll<SchoolSubjectPickViewModel>();
            teacherData.ApplicationUserPicturePath = this.cloudStorageService.GetImageUri(teacherData.ApplicationUserPicturePath);
            teacherData.QualificationDocumentPath = this.cloudStorageService.GetImageUri(teacherData.QualificationDocumentPath);

            return teacherData;
        }

        public async Task SetTeacherDataAsync(TeacherExternalDataInputModel input, string email)
        {
            var message = GlobalConstants.EmailMessages.TeacherProfileIsConfirmedMessage;
            await this.SendEmailResponce(email, message);
            await this.teachersService.AcceptTeacherAsync(input.Id, input.SubjectsId);
        }

        public async Task Reject(string teacherId, string email)
        {
            var message = GlobalConstants.EmailMessages.TeacherProfileIsRejectedMessage;
            await this.SendEmailResponce(email, message);
            await this.teachersService.RejectTeacherAsync(teacherId);
        }

        public IEnumerable<TeacherForAllTeachersListViewModel> GetAllTeachersViewModel()
        {
            return this
                .teachersService
                .GetAllAsNoTracking()
                .To<TeacherForAllTeachersListViewModel>().ToList();
        }

        public TeacherDetailedViewModel GetDetailsViewModel(string teacherId)
        {
            return this
                .teachersService
                .GetOne<TeacherDetailedViewModel>(teacherId);
        }

        public TeacherDetailedViewModel GetEditViewModel(string teacherId)
        {
            var teacher = this.teachersService.GetOne<TeacherDetailedViewModel>(teacherId);
            teacher.AllSubjects = this.schoolSubjectsService.GetAll<SchoolSubjectPickViewModel>();

            return teacher;
        }

        public async Task<(bool HasSucceeded, string Message)> EditAsync(string teacherId, TeacherDetailedViewModel teacherData)
        {
            if (teacherId != teacherData.Id)
            {
                return (false, GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            var teacher = this.teachersService.GetOneWithSubjectsTracked(teacherId);

            if (teacher == null)
            {
                return (false, GlobalConstants.TeacherMessages.TeacherNotFoundMessage);
            }

            teacher.ApplicationUser = await this.userManager.FindByIdAsync(teacher.ApplicationUserId);

            if (teacher.ApplicationUser.Name != teacherData.ApplicationUserName)
            {
                teacher.ApplicationUser.Name = teacherData.ApplicationUserName;
            }

            if (teacher.ApplicationUser.Email != teacherData.ApplicationUserEmail)
            {
                teacher.ApplicationUser.Email = teacherData.ApplicationUserEmail;
            }

            if (teacher.ApplicationUser.PicturePath != teacherData.ApplicationUserPicturePath)
            {
                teacher.ApplicationUser.PicturePath = teacherData.ApplicationUserPicturePath;
            }

            if (teacher.QualificationDocumentPath != teacherData.QualificationDocumentPath)
            {
                teacher.QualificationDocumentPath = teacherData.QualificationDocumentPath;
            }

            if (teacher.IsValidated != teacherData.IsValidated ||
                teacher.IsRejected != teacherData.IsRejected)
            {
                if (teacher.IsValidated && !teacher.IsRejected && !teacherData.SelectedSubjectsIds.Any())
                {
                    return (false, ValidationConstants.GeneralError);
                }

                teacher.IsValidated = teacherData.IsValidated;
                teacher.IsRejected = teacherData.IsRejected;
            }

            if (teacher.IsValidated && !teacher.IsRejected && teacherData.SelectedSubjectsIds.Any())
            {
                var all = this.schoolSubjectsService.GetAllRaw();

                foreach (var subject in teacherData.SelectedSubjectsIds)
                {
                    if (!teacher.Subjects.Any(x => x.Id == subject))
                    {
                        teacher.Subjects.Add(all.Single(x => x.Id == subject));
                    }
                }

                foreach (var subject in teacher.Subjects)
                {
                    if (!teacherData.SelectedSubjectsIds.Any(x => x == subject.Id))
                    {
                        teacher.Subjects.Remove(subject);
                    }
                }
            }

            await this.teachersService.UpdateAsync(teacher);

            return (true, GlobalConstants.GeneralMessages.TaskSucceededMessage);
        }

        private Task SendEmailResponce(string to, string message)
        {
            return this.emailSender.SendEmailAsync(
                GlobalConstants.ContactEmail,
                GlobalConstants.SystemName,
                to,
                "Профила ви бе:",
                message);
        }
    }
}
