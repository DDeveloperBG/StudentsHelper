namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Location;
    using StudentsHelper.Services.Data.Teachers;
    using Xunit;

    public class LocationServiceTests : BaseTest
    {
        private List<Region> regions;
        private List<Teacher> teachers;
        private LocationService locationService;
        private int id = 1;

        [Fact]
        public void GetTeacherLocation_ReturnsLocationInputModel()
        {
            var region = new Region
            {
                Id = this.GetId(),
            };
            var township = new Township
            {
                Id = this.GetId(),
                Region = region,
                RegionId = region.Id,
            };
            var populatedArea = new PopulatedArea
            {
                Id = this.GetId(),
                Township = township,
                TownshipId = township.Id,
            };
            var school = new School
            {
                Id = this.GetId(),
                PopulatedArea = populatedArea,
                PopulatedAreaId = populatedArea.Id,
            };
            const string teacherUserId = "1";
            var teacher = new Teacher
            {
                School = school,
                SchoolId = school.Id,
                ApplicationUserId = teacherUserId,
                ApplicationUser = new ApplicationUser
                {
                    Id = teacherUserId,
                    UserName = "no deleted user",
                },
            };

            this.teachers.Add(teacher);

            var location = this.locationService.GetTeacherLocation(teacherUserId);

            Assert.Equal(region.Id, location.RegionId);
            Assert.Equal(township.Id, location.TownshipId);
            Assert.Equal(populatedArea.Id, location.PopulatedAreaId);
            Assert.Equal(school.Id, location.SchoolId);
        }

        [Fact]
        public void ChangeTeacherLocationAsync_UpdatesLocationSuccessfully()
        {
            var region = new Region
            {
                Id = this.GetId(),
            };
            var township = new Township
            {
                Id = this.GetId(),
                Region = region,
                RegionId = region.Id,
            };
            var populatedArea = new PopulatedArea
            {
                Id = this.GetId(),
                Township = township,
                TownshipId = township.Id,
            };
            var school = new School
            {
                Id = this.GetId(),
                PopulatedArea = populatedArea,
                PopulatedAreaId = populatedArea.Id,
            };
            const string teacherUserId = "1";
            var teacher = new Teacher
            {
                School = school,
                SchoolId = school.Id,
                ApplicationUserId = teacherUserId,
                ApplicationUser = new ApplicationUser
                {
                    Id = teacherUserId,
                    UserName = "no deleted user",
                },
            };

            this.teachers.Add(teacher);

            const int newSchoolId = 7;
            this.locationService.ChangeTeacherLocationAsync(teacherUserId, newSchoolId).GetAwaiter().GetResult();

            Assert.Equal(newSchoolId, teacher.SchoolId);
        }

        [Fact]
        public void GetTeachersInLocation_ReturnsTeachersInLocation_ByRegion()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id = this.GetId(),
                },
                new Region
                {
                    Id = this.GetId(),
                },
            };
            var townships = new List<Township>
            {
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
            };
            foreach (var item in regions)
            {
                item.Townships = townships.Where(x => x.RegionId == item.Id).ToList();
            }

            var populatedAreas = new List<PopulatedArea>
            {
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[0],
                    TownshipId = townships[0].Id,
                },
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[1],
                    TownshipId = townships[1].Id,
                },
            };
            foreach (var item in townships)
            {
                item.PopulatedAreas = populatedAreas.Where(x => x.TownshipId == item.Id).ToList();
            }

            var schools = new List<School>
            {
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[1],
                    PopulatedAreaId = populatedAreas[1].Id,
                },
            };
            foreach (var item in populatedAreas)
            {
                item.Schools = schools.Where(x => x.PopulatedAreaId == item.Id).ToList();
            }

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[1],
                    SchoolId = schools[1].Id,
                },
                new Teacher
                {
                    School = schools[2],
                    SchoolId = schools[2].Id,
                },
            };
            foreach (var item in schools)
            {
                item.Teachers = teachers.Where(x => x.SchoolId == item.Id).ToList();
            }

            this.regions.AddRange(regions);

            var result = this.locationService.GetTeachersInLocation(regions[0].Id, 0, 0, 0).ToList();
            Assert.Equal(teachers.Count, result.Count);
            Assert.True(teachers.All(x => result.Any(y => y.Id == x.Id)));
        }

        [Fact]
        public void GetTeachersInLocation_ReturnsTeachersInLocation_ByTownship()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id = this.GetId(),
                },
                new Region
                {
                    Id = this.GetId(),
                },
            };
            var townships = new List<Township>
            {
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
            };
            foreach (var item in regions)
            {
                item.Townships = townships.Where(x => x.RegionId == item.Id).ToList();
            }

            var populatedAreas = new List<PopulatedArea>
            {
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[0],
                    TownshipId = townships[0].Id,
                },
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[1],
                    TownshipId = townships[1].Id,
                },
            };
            foreach (var item in townships)
            {
                item.PopulatedAreas = populatedAreas.Where(x => x.TownshipId == item.Id).ToList();
            }

            var schools = new List<School>
            {
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[1],
                    PopulatedAreaId = populatedAreas[1].Id,
                },
            };
            foreach (var item in populatedAreas)
            {
                item.Schools = schools.Where(x => x.PopulatedAreaId == item.Id).ToList();
            }

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[1],
                    SchoolId = schools[1].Id,
                },
                new Teacher
                {
                    School = schools[2],
                    SchoolId = schools[2].Id,
                },
            };
            foreach (var item in schools)
            {
                item.Teachers = teachers.Where(x => x.SchoolId == item.Id).ToList();
            }

            this.regions.AddRange(regions);

            var expectedResult = teachers.Where(x => x.School.PopulatedArea.Township.Id == townships[1].Id);
            var result = this.locationService.GetTeachersInLocation(regions[0].Id, townships[1].Id, 0, 0).ToList();
            Assert.Equal(expectedResult.Count(), result.Count);
            Assert.True(expectedResult.All(x => result.Any(y => y.Id == x.Id)));
        }

        [Fact]
        public void GetTeachersInLocation_ReturnsTeachersInLocation_ByPopulatedArea()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id = this.GetId(),
                },
                new Region
                {
                    Id = this.GetId(),
                },
            };
            var townships = new List<Township>
            {
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
            };
            foreach (var item in regions)
            {
                item.Townships = townships.Where(x => x.RegionId == item.Id).ToList();
            }

            var populatedAreas = new List<PopulatedArea>
            {
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[0],
                    TownshipId = townships[0].Id,
                },
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[1],
                    TownshipId = townships[1].Id,
                },
            };
            foreach (var item in townships)
            {
                item.PopulatedAreas = populatedAreas.Where(x => x.TownshipId == item.Id).ToList();
            }

            var schools = new List<School>
            {
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[1],
                    PopulatedAreaId = populatedAreas[1].Id,
                },
            };
            foreach (var item in populatedAreas)
            {
                item.Schools = schools.Where(x => x.PopulatedAreaId == item.Id).ToList();
            }

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[1],
                    SchoolId = schools[1].Id,
                },
                new Teacher
                {
                    School = schools[2],
                    SchoolId = schools[2].Id,
                },
            };
            foreach (var item in schools)
            {
                item.Teachers = teachers.Where(x => x.SchoolId == item.Id).ToList();
            }

            this.regions.AddRange(regions);

            var expectedResult1 = teachers.Where(x => x.School.PopulatedArea.Id == populatedAreas[0].Id);
            var result1 = this.locationService.GetTeachersInLocation(regions[0].Id, townships[0].Id, populatedAreas[0].Id, 0).ToList();
            Assert.Equal(expectedResult1.Count(), result1.Count);
            Assert.True(expectedResult1.All(x => result1.Any(y => y.Id == x.Id)));

            var expectedResult2 = teachers.Where(x => x.School.PopulatedArea.Id == populatedAreas[1].Id);
            var result2 = this.locationService.GetTeachersInLocation(regions[0].Id, townships[1].Id, populatedAreas[1].Id, 0).ToList();
            Assert.Equal(expectedResult2.Count(), result2.Count);
            Assert.True(expectedResult2.All(x => result2.Any(y => y.Id == x.Id)));
        }

        [Fact]
        public void GetTeachersInLocation_ReturnsTeachersInLocation_BySchool()
        {
            var regions = new List<Region>
            {
                new Region
                {
                    Id = this.GetId(),
                },
                new Region
                {
                    Id = this.GetId(),
                },
            };
            var townships = new List<Township>
            {
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
                new Township
                {
                    Id = this.GetId(),
                    Region = regions[0],
                    RegionId = regions[0].Id,
                },
            };
            foreach (var item in regions)
            {
                item.Townships = townships.Where(x => x.RegionId == item.Id).ToList();
            }

            var populatedAreas = new List<PopulatedArea>
            {
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[0],
                    TownshipId = townships[0].Id,
                },
                new PopulatedArea
                {
                    Id = this.GetId(),
                    Township = townships[1],
                    TownshipId = townships[1].Id,
                },
            };
            foreach (var item in townships)
            {
                item.PopulatedAreas = populatedAreas.Where(x => x.TownshipId == item.Id).ToList();
            }

            var schools = new List<School>
            {
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[0],
                    PopulatedAreaId = populatedAreas[0].Id,
                },
                new School
                {
                    Id = this.GetId(),
                    PopulatedArea = populatedAreas[1],
                    PopulatedAreaId = populatedAreas[1].Id,
                },
            };
            foreach (var item in populatedAreas)
            {
                item.Schools = schools.Where(x => x.PopulatedAreaId == item.Id).ToList();
            }

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[0],
                    SchoolId = schools[0].Id,
                },
                new Teacher
                {
                    School = schools[1],
                    SchoolId = schools[1].Id,
                },
                new Teacher
                {
                    School = schools[2],
                    SchoolId = schools[2].Id,
                },
            };
            foreach (var item in schools)
            {
                item.Teachers = teachers.Where(x => x.SchoolId == item.Id).ToList();
            }

            this.regions.AddRange(regions);

            var expectedResult1 = teachers.Where(x => x.School.Id == schools[0].Id);
            var result1 = this.locationService.GetTeachersInLocation(regions[0].Id, townships[0].Id, populatedAreas[0].Id, schools[0].Id).ToList();
            Assert.Equal(expectedResult1.Count(), result1.Count);
            Assert.True(expectedResult1.All(x => result1.Any(y => y.Id == x.Id)));

            var expectedResult2 = teachers.Where(x => x.School.Id == schools[1].Id);
            var result2 = this.locationService.GetTeachersInLocation(regions[0].Id, townships[0].Id, populatedAreas[0].Id, schools[1].Id).ToList();
            Assert.Equal(expectedResult2.Count(), result2.Count);
            Assert.True(expectedResult2.All(x => result2.Any(y => y.Id == x.Id)));

            var expectedResult3 = teachers.Where(x => x.School.Id == schools[2].Id);
            var result3 = this.locationService.GetTeachersInLocation(regions[0].Id, townships[1].Id, populatedAreas[1].Id, schools[2].Id).ToList();
            Assert.Equal(expectedResult3.Count(), result3.Count);
            Assert.True(expectedResult3.All(x => result3.Any(y => y.Id == x.Id)));
        }

        public override void CleanWorkbench()
        {
            this.regions = new List<Region>();

            var regionsRepository = GetMockedClasses.MockIRepository(this.regions);

            this.teachers = new List<Teacher>();

            var teachersRepository = GetMockedClasses.MockIDeletableEntityRepository(this.teachers);

            var teachersService = new TeachersService(teachersRepository.Object, null, null);

            this.locationService = new LocationService(regionsRepository.Object, teachersService);
        }

        private int GetId()
        {
            return this.id++;
        }
    }
}
