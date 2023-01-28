namespace DapperIntro.Model
{
    public class StudentWithCity : Student
    {
        public City City { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
