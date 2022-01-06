namespace StudentsHelper.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    internal class SchoolsAndLocationSeeder : ISeeder
    {
        private readonly string schoolsDataPath = @".\SeedingData\SchoolsData.json";

        private readonly List<Region> regions;
        private readonly List<Township> townships;
        private readonly List<PopulatedArea> populatedAreas;

        private readonly List<School> schools;

        public SchoolsAndLocationSeeder()
        {
            this.regions = new List<Region>();
            this.townships = new List<Township>();
            this.populatedAreas = new List<PopulatedArea>();

            this.schools = new List<School>();
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Schools.Any())
            {
                return;
            }

            // 0 - "﻿№",
            // 1 - "Код по НЕИСПУО",
            // 2 - "Име на институция",
            // 3 - "Директор",
            // 4 - "Област",
            // 5 - "Община",
            // 6 - "Населено място",
            // 7 - "Пощенски код",
            // 8 - "Адрес",
            // 9 - "Уеб сайт"
            string schoolsData = await File.ReadAllTextAsync(this.schoolsDataPath);

            var origin = JsonSerializer
                .Deserialize<List<List<string>>>(schoolsData)
                .Skip(1);

            foreach (var schoolData in origin)
            {
                string name = schoolData[2];

                if (name.ToLower().Contains("детска"))
                {
                    continue;
                }

                var region = this.AddIfNewerRegion(schoolData[4]);
                var township = this.AddIfNewerTownship(schoolData[5], region);
                var populatedArea = this.AddIfNewerPopulatedArea(schoolData[6], township);

                this.AddIfNewerSchool(schoolData[2], populatedArea);
            }

            dbContext.Regions.AddRange(this.regions);
            dbContext.Townships.AddRange(this.townships);
            dbContext.PopulatedAreas.AddRange(this.populatedAreas);

            dbContext.Schools.AddRange(this.schools);
        }

        private Region AddIfNewerRegion(string name)
        {
            var region = this.regions.FirstOrDefault(x => x.Name == name);

            if (region == null)
            {
                region = new Region
                {
                    Name = name,
                };

                this.regions.Add(region);
            }

            return region;
        }

        private Township AddIfNewerTownship(string name, Region region)
        {
            var township = this.townships.FirstOrDefault(x => x.Name == name && x.Region == region);

            if (township == null)
            {
                township = new Township
                {
                    Name = name,
                    Region = region,
                };

                this.townships.Add(township);
            }

            return township;
        }

        private PopulatedArea AddIfNewerPopulatedArea(string name, Township township)
        {
            var populatedArea = this.populatedAreas.FirstOrDefault(x => x.Name == name && x.Township == township);

            if (populatedArea == null)
            {
                populatedArea = new PopulatedArea
                {
                    Name = name,
                    Township = township,
                };

                this.populatedAreas.Add(populatedArea);
            }

            return populatedArea;
        }

        private School AddIfNewerSchool(string name, PopulatedArea populatedArea)
        {
            var school = this.schools.FirstOrDefault(x => x.Name == name && x.PopulatedArea == populatedArea);

            if (school == null)
            {
                school = new School
                {
                    Name = name,
                    PopulatedArea = populatedArea,
                };

                this.schools.Add(school);
            }

            return school;
        }
    }
}
