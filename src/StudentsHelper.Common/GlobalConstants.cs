namespace StudentsHelper.Common
{
    using System.Text.Encodings.Web;

    public static class GlobalConstants
    {
        public const string SystemName = "StudentsHelper";

        public const string ContactEmail = "dakataebg@students.softuni.bg";

        public const string FacebookPageURL = "https://www.facebook.com/Students-Helper-109858771554319";

        public const string AdministratorRoleName = "administrator";

        public const string TeacherRoleName = "teacher";

        public const string StudentRoleName = "student";

        public const string SiteDescription = $"{SystemName} има за цел да помогне на учениците с техните домашни, уроци и т.н. чрез мигновени консултации по конкретна тема с учител, специалист в областта.";

        public const string DeletedUserUsername = "deleted_user";

        public const string NoProfilePicturePath = "/img/noProgilePicture.png";

        public const string ConfirmEmailTitle = "Потвърдете акаунта си";

        public const string ProfilePicturesFolder = "ProfilePictures";

        public const decimal WebsiteMonthPercentageTax = 0.05M; // 5%

        public const string ReturnUrlSessionValueKey = "returnUrl";

        public const string SubjectIdSessionValueKey = "subjectId";

        public static class GeneralMessages
        {
            public const string UserNotFoundMessage = "Не може да се зареди текущия потребител.";

            public const string TaskSucceededMessage = "Задачата бе изпълнена успешно.";

            public const string TaskFailedMessage = "Възникна грешка!";
        }

        public static class PaymentStatusMessages
        {
            public const string Success = "success";

            public const string Canceled = "canceled";
        }

        public static class TeacherMessages
        {
            public const string TeacherNotFoundMessage = "Учителя не бе намерен!";

            public const string NotSetHourWage = "Не сте задали почасово заплащане на услугите си, за да го направите отидете в профила си и кликнете на баланс.";
        }

        public static class SchoolMessages
        {
            public const string FailSchoolUpdateMessage = "Error: Неочаквана грешка при опит за промяна на училище.";

            public const string SuccessSchoolUpdateMessage = "Вашето училище бе актуализирано";
        }

        public static class StudentMessages
        {
            public const string StudentNotFoundMessage = "Ученика не бе намерен!";
        }

        public static class EmailMessages
        {
            public const string TeacherProfileIsConfirmedMessage = "<h1 style=\"color: #6BB843\">Профила ви бе потвърден. Вече може да водите уроци.</h1>";

            public const string TeacherProfileIsRejectedMessage = "<h1 style=\"color: #E12735\">Профила ви бе отхвърлен. Не може да водите уроци.</h1>" + "<h2>Пробвайте да се регистрирате пак и качете по-добра снимка на документа ви за квалификация.</h2>";

            public const string VerificationEmailIsSentMessage = "Изпратен е имейл за потвърждение. Моля, проверете електронната си поща.";

            public const string EmailConfirmationTitle = "Имейл за потвърждение";

            public static string GetEmailConfirmationMessage(string confirmUrl) => $"<h1>Моля потвърдете акаунта си, като <a href='{HtmlEncoder.Default.Encode(confirmUrl)}'>кликнете тук</a>.";
        }

        public static class ContactFormMessages
        {
            public const string MessageNotSentMessage = "Не успяхме да изпратим съобщението ви";

            public const string MessageSentSuccessfullyMessage = "Съобщението ви бе изпратено успешно!";
        }

        public static class ReviewMessages
        {
            public const string HasReviewedAlreadyMessage = "Вече сте оценили един път.";

            public const string SuccessfulReviewMessage = "Успешно дадохте оценка.";
        }

        public static class PaymentMessages
        {
            public const string SuccessfulPaymentMessage = "Плащането бе успешно!";

            public const string FailedPaymentMessage = "Плащането бе отказано!";

            public const string InsufficientBalanceMessage = "Плащането бе отказано!";
        }

        public static class ConsultationMessages
        {
            public const string DurationLength = "Продължителността трябва да е между 10 минути и 5 часа.";

            public const string StartTime = "Началото на консултацията трябва да е по - късно от текущия момент.";

            public const string NullHourWage = "Настъпи грешка! Заплащането на учителя е null.";

            public const string SuccessfulConsultationReservation = "Успешно резервирахте консултация.";
        }
    }
}
