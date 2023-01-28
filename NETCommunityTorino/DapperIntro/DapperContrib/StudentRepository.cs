//using Dapper.Contrib.Extensions;

namespace DapperIntro.DapperContrib
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public Student GetStudent(int id)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    var student = connection.Get<Student>(id);

        //    return student;
        //}

        //public Student[] GetStudents()
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    var students = connection.GetAll<Student>();

        //    return students.ToArray();
        //}

        //public void Insert(Student student)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.Insert(student);
        //}

        //public void Insert(Student[] students)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.Insert(students);
        //}

        //public void Update(Student student)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.Update(student);
        //}

        //public void Update(Student[] students)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.Update(students);
        //}

        //public void Delete(Student student)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.Delete(student);
        //}

        //public void Delete(Student[] students)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.Delete(students);
        //}

        //public void DeleteAll()
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    connection.DeleteAll<Student>();
        //}
    }
}
