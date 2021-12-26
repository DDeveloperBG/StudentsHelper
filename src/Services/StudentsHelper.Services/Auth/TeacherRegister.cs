using StudentsHelper.Data;
using StudentsHelper.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsHelper.Services.Auth
{
    public class TeacherRegister : ITeacherRegister
    {
        private readonly ApplicationDbContext dbContext;

        public TeacherRegister(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string RegisterTeacher(TeacherInputModel inputModel, string userId)
        {
            if (inputModel == null)
            {
                return "Настъпи грешка!";
            }

            if (inputModel.SubjectId == 0 || inputModel.SchoolId == 0)
            {
                return "Полетата не са попълнени!";
            }

            if (!this.dbContext.Subjects.Any(x => x.Id == inputModel.SubjectId))
            {
                return "Невалиден предмет!";
            }

            if (!this.dbContext.Schools.Any(x => x.Id == inputModel.SchoolId))
            {
                return "Невалидно училище!";
            }

            Teacher teacher = new Teacher
            {
                SubjectId = inputModel.SubjectId,
                SchoolId = inputModel.SchoolId,
                ApplicationUserId = userId,
                QualificationDocumentPath = inputModel.QualificationDocumentPath,
            };

            return null;
        }
    }
}
