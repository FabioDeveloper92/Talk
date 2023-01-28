namespace DapperIntro.Model
{
    public class StudentWithCourses : Student
    {
        public List<CourseWithSubject> Courses { get; set; }
    }

    public class CourseWithSubject : Course
    {
        public string Name { get; set; }
    }

    public class Course
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime SubscribedDate { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
