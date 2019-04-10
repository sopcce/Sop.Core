using Microsoft.EntityFrameworkCore.Internal;

namespace Sop.FileUpload.Models
{
    public static class DbInitializer
    {
        public static void Initialize(SopFileUploadContext context)
        {
            // context.Database.EnsureCreated();

            // Look for any students.
            if (context.Fileserver.Any())
            {
                return;   // DB has been seeded
            }

            //var students = new Student[]
            //{
            //    new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            //    new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            //    new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            //    new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            //    new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            //    new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            //    new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            //    new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            //};
            //foreach (Student s in students)
            //{
            //    context.Student.Add(s);
            //}

        }
    }
}