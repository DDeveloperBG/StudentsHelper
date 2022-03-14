namespace StudentsHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Teachers;
    using Xunit;

    public class TeachersServiceTests : BaseTest
    {
        private List<SchoolSubject> schoolSubjects;
        private List<Teacher> teachers;
        private TeachersService service;

        [Fact]
        public void GetAllAsNoTracking_Succeede()
        {
            const string deletedTeacherId = "importantId";
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = deletedTeacherId,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = GlobalConstants.DeletedUserUsername,
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_2",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_3",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_4",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_5",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            var result = this.service.GetAllAsNoTracking().ToList();

            Assert.Equal(teachers.Count - 1, result.Count);
            Assert.DoesNotContain(teachers.First(x => x.Id == deletedTeacherId), result);
            Assert.True(teachers.Skip(1).All(x => result.Any(y => y.Id == x.Id)));
        }

        [Fact]
        public void GetAll_Succeede()
        {
            const string deletedTeacherId = "importantId";
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = deletedTeacherId,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = GlobalConstants.DeletedUserUsername,
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_2",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_3",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_4",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_5",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            var result = this.service.GetAll().ToList();

            Assert.Equal(teachers.Count - 1, result.Count);
            Assert.DoesNotContain(teachers.First(x => x.Id == deletedTeacherId), result);
            Assert.True(teachers.Skip(1).All(x => result.Any(y => y.Id == x.Id)));
        }

        [Fact]
        public void GetHourWage_Succeede()
        {
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    HourWage = null,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = GlobalConstants.DeletedUserUsername,
                    },
                },
                new Teacher
                {
                    HourWage = 12.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_2",
                    },
                },
                new Teacher
                {
                    HourWage = 24,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_3",
                    },
                },
                new Teacher
                {
                    HourWage = 0.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_4",
                    },
                },
                new Teacher
                {
                    HourWage = 103.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_5",
                    },
                },
                new Teacher
                {
                    HourWage = 1,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                },
                new Teacher
                {
                    HourWage = 10.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                },
                new Teacher
                {
                    HourWage = 20.54M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            foreach (var teacher in teachers)
            {
                var result = this.service.GetHourWage(teacher.Id);
                Assert.Equal(teacher.HourWage, result);
            }

            var deletedTeacherWithHourWage = new Teacher
            {
                HourWage = 12.5M,
                ApplicationUser = new ApplicationUser
                {
                    UserName = GlobalConstants.DeletedUserUsername,
                },
            };
            this.teachers.Add(deletedTeacherWithHourWage);
            Assert.Null(this.service.GetHourWage(deletedTeacherWithHourWage.Id));

            Assert.Null(this.service.GetHourWage("not existing"));
        }

        [Fact]
        public void ChangeTeacherHourWageAsync_Succeede()
        {
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    HourWage = 12.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_2",
                    },
                },
                new Teacher
                {
                    HourWage = 24,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_3",
                    },
                },
                new Teacher
                {
                    HourWage = 0.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_4",
                    },
                },
                new Teacher
                {
                    HourWage = 103.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_5",
                    },
                },
                new Teacher
                {
                    HourWage = 1,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                },
                new Teacher
                {
                    HourWage = 10.5M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                },
                new Teacher
                {
                    HourWage = 20.54M,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            var random = new Random();
            foreach (var teacher in teachers)
            {
                var newTeacherWage = (random.Next(0, 200) / 100) + random.Next(0, 200);
                this.service.ChangeTeacherHourWageAsync(teacher.Id, newTeacherWage).GetAwaiter().GetResult();
                Assert.Equal(newTeacherWage, teacher.HourWage);
            }
        }

        [Fact]
        public void ChangeTeacherHourWageAsync_Fail()
        {
            var action = () => this.service.ChangeTeacherHourWageAsync("not existing", 21).GetAwaiter().GetResult();

            Assert.Throws<NullReferenceException>(action);
        }

        [Fact]
        public void GetExpressConnectedAccountId_Succeede()
        {
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    ExpressConnectedAccountId = "123_xx",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_2",
                    },
                },
                new Teacher
                {
                    ExpressConnectedAccountId = "1ha3231a3pPma",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_3",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            foreach (var teacher in teachers)
            {
                var result = this.service.GetExpressConnectedAccountId(teacher.Id);
                Assert.Equal(teacher.ExpressConnectedAccountId, result);
            }
        }

        [Fact]
        public void GetExpressConnectedAccountId_Fail()
        {
            var deletedTeacher = new Teacher
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = GlobalConstants.DeletedUserUsername,
                },
            };
            this.teachers.Add(deletedTeacher);

            Assert.Throws<InvalidOperationException>(() => this.service.GetExpressConnectedAccountId(deletedTeacher.Id));

            Assert.Throws<InvalidOperationException>(() => this.service.GetExpressConnectedAccountId("Not Existing"));
        }

        [Fact]
        public void IsTeacherConfirmed_Succeede()
        {
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    IsValidated = true,
                    ApplicationUserId = "3",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_3",
                    },
                },
                new Teacher
                {
                    IsValidated = false,
                    ApplicationUserId = "2",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_2",
                    },
                },
                new Teacher
                {
                    ApplicationUserId = "1",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_1",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            foreach (var teacher in teachers)
            {
                var result = this.service.IsTeacherConfirmed(teacher.ApplicationUserId);
                Assert.Equal(teacher.IsValidated, result);
            }
        }

        [Fact]
        public void DeleteTeacherAsync_Succeede()
        {
            var teacher = new Teacher
            {
                ApplicationUserId = "xxx",
                ApplicationUser = new ApplicationUser
                {
                    UserName = "456",
                },
            };

            this.teachers.Add(teacher);

            this.service.DeleteTeacherAsync(teacher.ApplicationUserId).GetAwaiter().GetResult();

            Assert.Equal(GlobalConstants.DeletedUserUsername, teacher.ApplicationUser.UserName);
        }

        [Fact]
        public void AcceptTeacherAsync_Succeede()
        {
            const string wantedTeacherId = "importantId";
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = wantedTeacherId,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "xxx",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            var schoolSubjects = new List<SchoolSubject>
            {
                new SchoolSubject
                {
                    Id = 1,
                    Name = "1",
                },
                new SchoolSubject
                {
                    Id = 2,
                    Name = "2",
                },
                new SchoolSubject
                {
                    Id = 3,
                    Name = "3",
                },
            };
            this.schoolSubjects.AddRange(schoolSubjects);

            var usedSchoolSubjects = schoolSubjects.Take(2).Select(x => x.Id).ToArray();

            this.service.AcceptTeacherAsync(wantedTeacherId, usedSchoolSubjects).GetAwaiter().GetResult();

            var teacher = teachers.Find(x => x.Id == wantedTeacherId);
            var teacherSchoolSubjects = teacher.Subjects;

            Assert.True(usedSchoolSubjects.All(x => teacherSchoolSubjects.Any(y => y.Id == x)));
            Assert.True(teacher.IsValidated);
            Assert.False(teacher.IsRejected);
        }

        [Fact]
        public void RejectTeacherAsync_Succeede()
        {
            const string wantedTeacherId = "importantId";
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = wantedTeacherId,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "xxx",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            this.service.RejectTeacherAsync(wantedTeacherId).GetAwaiter().GetResult();

            var teacher = teachers.Find(x => x.Id == wantedTeacherId);

            Assert.True(teacher.IsValidated);
            Assert.True(teacher.IsRejected);
        }

        [Fact]
        public void GetId_Succeede()
        {
            const string wantedTeacherId = "importantId";
            const string wantedTeacherUserId = "importantId2";
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = wantedTeacherId,
                    ApplicationUserId = wantedTeacherUserId,
                    ApplicationUser = new ApplicationUser
                    {
                        Id = wantedTeacherUserId,
                        UserName = "xxx",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            var result = this.service.GetId(wantedTeacherUserId);

            Assert.Equal(wantedTeacherId, result);
        }

        [Fact]
        public void GetOneWithSubjectsTracked_Succeede()
        {
            var schoolSubjects = new List<SchoolSubject>
            {
                new SchoolSubject
                {
                    Id = 1,
                    Name = "1",
                },
                new SchoolSubject
                {
                    Id = 2,
                    Name = "2",
                },
                new SchoolSubject
                {
                    Id = 3,
                    Name = "3",
                },
            };
            this.schoolSubjects.AddRange(schoolSubjects);

            var usedSchoolSubjects = schoolSubjects.Take(2).ToList();

            const string wantedTeacherId = "importantId";
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = wantedTeacherId,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "xxx",
                    },
                    Subjects = usedSchoolSubjects,
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                },
            };

            this.teachers.AddRange(teachers);

            var teacher = this.service.GetOneWithSubjectsTracked(wantedTeacherId);

            Assert.Equal(teacher, teachers.Find(x => x.Id == wantedTeacherId));
        }

        [Fact]
        public void GetAllOfType_Succeede()
        {
            const int wantedSubjectId = 1;
            var schoolSubjects = new List<SchoolSubject>
            {
                new SchoolSubject
                {
                    Id = wantedSubjectId,
                    Name = "1",
                },
                new SchoolSubject
                {
                    Id = 2,
                    Name = "2",
                },
                new SchoolSubject
                {
                    Id = 3,
                    Name = "3",
                },
            };
            this.schoolSubjects.AddRange(schoolSubjects);

            var expectedTeachersIds = new string[] { "xxx1", "xxx2" };

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = "xxx1",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "xxx",
                    },
                    Subjects = schoolSubjects.Take(2).ToList(),
                    IsValidated = true,
                    IsRejected = false,
                    HourWage = 12.5M,
                },
                new Teacher
                {
                    Id = "xxx2",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                    Subjects = schoolSubjects.Take(1).ToList(),
                    IsValidated = true,
                    IsRejected = false,
                    HourWage = 7.5M,
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                    Subjects = schoolSubjects.TakeLast(3).ToList(),
                    IsValidated = true,
                    IsRejected = true,
                    HourWage = 1.5M,
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                    Subjects = schoolSubjects.TakeLast(2).ToList(),
                    IsValidated = true,
                    IsRejected = false,
                    HourWage = 12,
                },
            };

            this.teachers.AddRange(teachers);

            var result = this.service.GetAllOfType(wantedSubjectId).ToList().Select(x => x.Id);

            Assert.True(expectedTeachersIds.All(x => result.Contains(x)));
        }

        [Fact]
        public void GetAllOfType_FromQuearableCollection_Succeede()
        {
            const int wantedSubjectId = 1;
            var schoolSubjects = new List<SchoolSubject>
            {
                new SchoolSubject
                {
                    Id = wantedSubjectId,
                    Name = "1",
                },
                new SchoolSubject
                {
                    Id = 2,
                    Name = "2",
                },
                new SchoolSubject
                {
                    Id = 3,
                    Name = "3",
                },
            };
            this.schoolSubjects.AddRange(schoolSubjects);

            var expectedTeachersIds = new string[] { "xxx1", "xxx2" };

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = "xxx1",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "xxx",
                    },
                    Subjects = schoolSubjects.Take(2).ToList(),
                    IsValidated = true,
                    IsRejected = false,
                    HourWage = 12.5M,
                },
                new Teacher
                {
                    Id = "xxx2",
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_6",
                    },
                    Subjects = schoolSubjects.Take(1).ToList(),
                    IsValidated = true,
                    IsRejected = false,
                    HourWage = 7.5M,
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_7",
                    },
                    Subjects = schoolSubjects.TakeLast(3).ToList(),
                    IsValidated = true,
                    IsRejected = true,
                    HourWage = 1.5M,
                },
                new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = "uN_8",
                    },
                    Subjects = schoolSubjects.TakeLast(2).ToList(),
                    IsValidated = true,
                    IsRejected = false,
                    HourWage = 12,
                },
            };

            this.teachers.AddRange(teachers);

            var result = this.service.GetAllOfType(wantedSubjectId, teachers.AsQueryable()).ToList().Select(x => x.Id);

            Assert.True(expectedTeachersIds.All(x => result.Contains(x)));
        }

        [Fact]
        public void GetAllNotConfirmed_Succeede()
        {
            var teachersData = new List<GetAllNotConfirmedTestObject>
            {
                new GetAllNotConfirmedTestObject
                {
                    Id = "xxx1",
                    ApplicationUserUserName = "xxx",
                    HourWage = 12.5M,
                },
                new GetAllNotConfirmedTestObject
                {
                    Id = "xxx2",
                    ApplicationUserUserName = "uN_6",
                    HourWage = 7.5M,
                },
                new GetAllNotConfirmedTestObject
                {
                    Id = "a4sdas2ad24421",
                    ApplicationUserUserName = "uN_7",
                    HourWage = 1.5M,
                },
                new GetAllNotConfirmedTestObject
                {
                    Id = "a5z2d24421",
                    ApplicationUserUserName = "uN_8",
                    HourWage = 12,
                },
            };

            foreach (var teacherData in teachersData)
            {
                var teacher = new Teacher
                {
                    Id = teacherData.Id,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = teacherData.ApplicationUserUserName,
                    },
                    HourWage = teacherData.HourWage,
                };

                this.teachers.Add(teacher);
            }

            var rejectedTeacher = new Teacher
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = "Gosho",
                },
                HourWage = 25.7M,
                IsValidated = true,
                IsRejected = true,
            };

            this.teachers.Add(rejectedTeacher);

            var confirmedTeacher = new Teacher
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = "Petyo",
                },
                HourWage = 3.7M,
                IsValidated = true,
            };

            this.teachers.Add(confirmedTeacher);

            var result = this.service.GetAllNotConfirmed<GetAllNotConfirmedTestObject>().ToList();

            Assert.NotEmpty(result);

            foreach (var teacher in result)
            {
                Assert.NotNull(teacher);

                var expectedTeacherData = teachersData.Find(x => x.Id == teacher.Id);

                Assert.NotNull(expectedTeacherData);
                Assert.Equal(expectedTeacherData.Id, teacher.Id);
                Assert.Equal(expectedTeacherData.HourWage, teacher.HourWage);
                Assert.Equal(expectedTeacherData.ApplicationUserUserName, teacher.ApplicationUserUserName);
            }
        }

        [Fact]
        public void GetAllRejected_Succeede()
        {
            var teachersData = new List<GetAllRejectedTestObject>
            {
                new GetAllRejectedTestObject
                {
                    Id = "xxx1",
                    ApplicationUserUserName = "xxx",
                    HourWage = 12.5M,
                    IsValidated = true,
                    IsRejected = true,
                },
                new GetAllRejectedTestObject
                {
                    Id = "xxx2",
                    ApplicationUserUserName = "uN_6",
                    HourWage = 7.5M,
                    IsValidated = true,
                    IsRejected = true,
                },
                new GetAllRejectedTestObject
                {
                    Id = "a4sdas2ad24421",
                    ApplicationUserUserName = "uN_7",
                    HourWage = 1.5M,
                    IsValidated = true,
                    IsRejected = true,
                },
                new GetAllRejectedTestObject
                {
                    Id = "a5z2d24421",
                    ApplicationUserUserName = "uN_8",
                    HourWage = 12,
                    IsValidated = true,
                    IsRejected = true,
                },
            };

            foreach (var teacherData in teachersData)
            {
                var teacher = new Teacher
                {
                    Id = teacherData.Id,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = teacherData.ApplicationUserUserName,
                    },
                    HourWage = teacherData.HourWage,
                    IsValidated = teacherData.IsValidated,
                    IsRejected = teacherData.IsRejected,
                };

                this.teachers.Add(teacher);
            }

            var confirmedTeacher = new Teacher
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = "Petyo",
                },
                HourWage = 3.7M,
                IsValidated = true,
            };

            this.teachers.Add(confirmedTeacher);

            var result = this.service.GetAllRejected<GetAllRejectedTestObject>().ToList();

            Assert.NotEmpty(result);

            foreach (var teacher in result)
            {
                Assert.NotNull(teacher);

                var expectedTeacherData = teachersData.Find(x => x.Id == teacher.Id);

                Assert.NotNull(expectedTeacherData);
                Assert.Equal(expectedTeacherData.Id, teacher.Id);
                Assert.Equal(expectedTeacherData.HourWage, teacher.HourWage);
                Assert.Equal(expectedTeacherData.ApplicationUserUserName, teacher.ApplicationUserUserName);
                Assert.True(teacher.IsValidated);
                Assert.True(teacher.IsRejected);
            }
        }

        [Fact]
        public void GetAllValidatedMappedAndTracked_Succeede()
        {
            var teachersData = new List<GetAllValidatedMappedAndTrackedTestObject>
            {
                new GetAllValidatedMappedAndTrackedTestObject
                {
                    Id = "xxx1",
                    ApplicationUserUserName = "xxx",
                    HourWage = 12.5M,
                    IsValidated = true,
                },
                new GetAllValidatedMappedAndTrackedTestObject
                {
                    Id = "xxx2",
                    ApplicationUserUserName = "uN_6",
                    HourWage = 7.5M,
                    IsValidated = true,
                },
                new GetAllValidatedMappedAndTrackedTestObject
                {
                    Id = "a4sdas2ad24421",
                    ApplicationUserUserName = "uN_7",
                    HourWage = 1.5M,
                    IsValidated = true,
                },
                new GetAllValidatedMappedAndTrackedTestObject
                {
                    Id = "a5z2d24421",
                    ApplicationUserUserName = "uN_8",
                    HourWage = 12,
                    IsValidated = true,
                },
            };

            foreach (var teacherData in teachersData)
            {
                var teacher = new Teacher
                {
                    Id = teacherData.Id,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = teacherData.ApplicationUserUserName,
                    },
                    HourWage = teacherData.HourWage,
                    IsValidated = teacherData.IsValidated,
                };

                this.teachers.Add(teacher);
            }

            var rejectedTeacher = new Teacher
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = "Gosho",
                },
                HourWage = 25.7M,
                IsValidated = true,
                IsRejected = true,
            };

            this.teachers.Add(rejectedTeacher);

            var notValidatedTeacher = new Teacher
            {
                ApplicationUser = new ApplicationUser
                {
                    UserName = "Petyo",
                },
                HourWage = 3.7M,
                IsValidated = false,
            };

            this.teachers.Add(notValidatedTeacher);

            var result = this.service.GetAllValidatedMappedAndTracked<GetAllValidatedMappedAndTrackedTestObject>().ToList();

            Assert.NotEmpty(result);

            foreach (var teacher in result)
            {
                Assert.NotNull(teacher);

                var expectedTeacherData = teachersData.Find(x => x.Id == teacher.Id);

                Assert.NotNull(expectedTeacherData);
                Assert.Equal(expectedTeacherData.Id, teacher.Id);
                Assert.Equal(expectedTeacherData.HourWage, teacher.HourWage);
                Assert.Equal(expectedTeacherData.ApplicationUserUserName, teacher.ApplicationUserUserName);
            }
        }

        [Fact]
        public void GetOne_Succeede()
        {
            var teachersData = new List<GetOneTestObject>
            {
                new GetOneTestObject
                {
                    Id = "xxx1",
                    ApplicationUserUserName = "xxx",
                    HourWage = 12.5M,
                    IsValidated = true,
                    IsRejected = true,
                },
                new GetOneTestObject
                {
                    Id = "xxx2",
                    ApplicationUserUserName = "uN_6",
                    HourWage = 7.5M,
                    IsValidated = true,
                    IsRejected = false,
                },
                new GetOneTestObject
                {
                    Id = "a4sdas2ad24421",
                    ApplicationUserUserName = "uN_7",
                    HourWage = 1.5M,
                    IsValidated = false,
                    IsRejected = false,
                },
                new GetOneTestObject
                {
                    Id = "a5z2d24421",
                    ApplicationUserUserName = "uN_8",
                    HourWage = 12,
                },
            };

            foreach (var teacherData in teachersData)
            {
                var teacher = new Teacher
                {
                    Id = teacherData.Id,
                    ApplicationUser = new ApplicationUser
                    {
                        UserName = teacherData.ApplicationUserUserName,
                    },
                    HourWage = teacherData.HourWage,
                    IsValidated = teacherData.IsValidated,
                    IsRejected = teacherData.IsRejected,
                };

                this.teachers.Add(teacher);
            }

            foreach (var teacher in teachersData)
            {
                var result = this.service.GetOne<GetOneTestObject>(teacher.Id);

                Assert.NotNull(result);
                Assert.Equal(teacher.Id, result.Id);
                Assert.Equal(teacher.HourWage, result.HourWage);
                Assert.Equal(teacher.ApplicationUserUserName, result.ApplicationUserUserName);
                Assert.Equal(teacher.IsValidated, result.IsValidated);
                Assert.Equal(teacher.IsRejected, result.IsRejected);
            }
        }

        public override void CleanWorkbench()
        {
            this.teachers = new List<Teacher>();

            var teachersRepository = GetMockedClasses.MockIDeletableEntityRepository(this.teachers);

            this.schoolSubjects = new List<SchoolSubject>();

            var schoolSubjectsRepository = GetMockedClasses.MockIDeletableEntityRepository(this.schoolSubjects);

            var deletedUser = new ApplicationUser
            {
                UserName = GlobalConstants.DeletedUserUsername,
            };

            var userManager = GetMockedClasses.MockUserManager<ApplicationUser>();
            userManager
                .Setup(u => u.FindByNameAsync(It.Is<string>(x => x == GlobalConstants.DeletedUserUsername)))
                .Returns(Task.FromResult(deletedUser));

            this.service = new TeachersService(
                teachersRepository.Object,
                schoolSubjectsRepository.Object,
                userManager.Object);
        }
    }
}
