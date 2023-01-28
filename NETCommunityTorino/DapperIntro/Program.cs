using DapperIntro.Model;

var myConnString = "Server=localhost;Database=DapperIntro;User Id=YOURUSERNAME;Password=YOURPASSWORD;";

Console.WriteLine("START QUERY METHOD");

var studentRepository = new DapperIntro.DapperWithSP.StudentRepository(myConnString);

var myStudent = new Student() { Name = "Karl", BirthDate = new DateTime(1998, 5, 14), CityId = 2 };
await studentRepository.InsertStudent(myStudent);

var myStudents = new[] {
    new Student() { Name = "Frank", BirthDate = new DateTime(1999, 6, 29), CityId = 2 },
    new Student() { Name = "Mary", BirthDate = new DateTime(1998, 2, 27), CityId = 1 }
};

//var insertedRows = await studentRepository.InsertStudents(myStudents);

var student = await studentRepository.GetStudent(1);
var students = await studentRepository.GetStudents();
var totalStudents = await studentRepository.GetTotalStudents();

var studentWithMark = await studentRepository.GetStudentWithMark(1);
var studentsFromNY = await studentRepository.GetStudentsByCity("NY");
var studentSubscriber = await studentRepository.GetStudentSubscriber(1);
var studentSubscriberAlt = await studentRepository.GetStudentSubscriberMoreThanSevenJoin(1);

await studentRepository.ReplaceStudent(students.First(x => x.Name.Equals("Karl")).Id, myStudent);

Console.WriteLine("FINISH QUERY METHOD");