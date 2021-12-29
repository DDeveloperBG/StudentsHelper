namespace StudentsHelper.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public class SchoolSubjectsSeeder : ISeeder
    {
        private const string SchoolDataPath = @"C:\Users\Dani\Documents\GitHub\StudentsHelper\src\Data\StudentsHelper.Data\Seeding\Data\schoolSubjects.txt";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Subjects.Any())
            {
                return;
            }

            var dataRaw = await File.ReadAllTextAsync(SchoolDataPath);
            List<SchoolSubject> subjects = dataRaw
                .Split(Environment.NewLine)
                .ToHashSet()
                .Select(row =>
                {
                    var dataParts = row.Split(", ");

                    return new SchoolSubject { Name = dataParts[0], IconPath = dataParts[1] };
                })
                .ToList();

            await dbContext.Subjects.AddRangeAsync(subjects);
            await dbContext.SaveChangesAsync();
        }
    }
}
