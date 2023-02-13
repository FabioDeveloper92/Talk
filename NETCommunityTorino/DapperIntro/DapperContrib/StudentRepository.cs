using Dapper.Contrib.Extensions;
using System.Data.SqlClient;

namespace DapperIntro.DapperContrib
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Student> GetStudent(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var student = await connection.GetAsync<Student>(id);

            return student;
        }

        public async Task<Student[]> GetStudents()
        {
            using var connection = new SqlConnection(_connectionString);
            var students = await connection.GetAllAsync<Student>();

            return students.ToArray();
        }

        public async Task InsertStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.InsertAsync(student);
        }

        public async Task InsertStudents(Student[] students)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.InsertAsync(students);
        }

        public async Task Update(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.UpdateAsync(student);
        }

        public async Task Update(Student[] students)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.UpdateAsync(students);
        }

        public async Task Delete(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.DeleteAsync(student);
        }

        public async Task Delete(Student[] students)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.DeleteAsync(students);
        }

        public async Task DeleteAll()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.DeleteAllAsync<Student>();
        }
    }
}
