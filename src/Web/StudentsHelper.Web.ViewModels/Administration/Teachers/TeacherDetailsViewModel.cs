﻿namespace StudentsHelper.Web.ViewModels.Administration.Teachers
{
    using System.Collections.Generic;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Subjects;
    using StudentsHelper.Services.Mapping;

    public class TeacherDetailsViewModel : IMapFrom<Teacher>
    {
        public string Id { get; set; }

        public string QualificationDocumentPath { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string ApplicationUserPicturePath { get; set; }

        public IEnumerable<SchoolSubjectPickViewModel> Subjects { get; set; }
    }
}
