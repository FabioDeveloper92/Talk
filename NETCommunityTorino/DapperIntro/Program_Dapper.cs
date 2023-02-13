using DapperIntro.DapperContrib;

public static class DapperContribExample
{
    public async static void Run()
    {
        var myConnString = "Server=localhost;Database=DapperIntro;User Id=;Password=;";

        Console.WriteLine("START QUERY METHOD");

        var studentRepository = new DapperIntro.DapperContrib.StudentRepository(myConnString);

        var myStudent = new Student() { Name = "Karl", BirthDate = new DateTime(1998, 5, 14), CityId = 2 };
        await studentRepository.InsertStudent(myStudent);

        var myStudents = new[] {
            new Student() { Name = "Frank", BirthDate = new DateTime(1999, 6, 29), CityId = 2 },
            new Student() { Name = "Mary", BirthDate = new DateTime(1998, 2, 27), CityId = 1 }
        };

        await studentRepository.InsertStudents(myStudents);

        var student = await studentRepository.GetStudent(1);
        var students = await studentRepository.GetStudents();

        await studentRepository.Update(new Student { Id = 1, Name = "David", BirthDate = new DateTime(2002, 2, 10) });
        await studentRepository.Update(new[] {
            new Student { Id = 1, Name = "David", BirthDate = new DateTime(2012, 12, 10) },
            new Student { Id = 2, Name = "Luka", BirthDate = new DateTime(2010, 1, 1) }
        });

        await studentRepository.Delete(new Student() { Id = 2 });
        await studentRepository.DeleteAll();

        Console.WriteLine("FINISH QUERY METHOD");
    }
}