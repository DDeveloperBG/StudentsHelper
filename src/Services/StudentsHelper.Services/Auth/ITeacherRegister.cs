namespace StudentsHelper.Services.Auth
{
    using System.Collections.Generic;

    public interface ITeacherRegister
    {
        public string RegisterTeacher(TeacherInputModel inputModel, string userId);
    }
}
