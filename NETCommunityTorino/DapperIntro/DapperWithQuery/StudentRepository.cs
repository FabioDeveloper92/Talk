using Dapper;
using DapperIntro.Model;
using System.Data.SqlClient;

namespace DapperIntro.DapperWithQuery
{
    public class StudentRepository
    {
        private const string StudentTableName = "dbo.Students";
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"INSERT INTO {StudentTableName} (Name, BirthDate, CityId)
                         VALUES(@Name, @BirthDate, @CityId)";

            await connection.ExecuteAsync(sql, student);
        }

        public async Task<int> InsertStudents(Student[] students)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"INSERT INTO {StudentTableName} (Name, BirthDate, CityId)
                         VALUES (@Name, @BirthDate, @CityId)";

            var insertedRows = await connection.ExecuteAsync(sql, students);
            return insertedRows;
        }

        public async Task<Student> GetStudent(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT Id, Name, BirthDate, CityId
                         FROM {StudentTableName} 
                         WHERE Id = @Id";

            var student = await connection.QuerySingleAsync<Student>(sql, new { id });

            return student;
        }
        public async Task<string[]> GetStudentNames(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT Id, Name, BirthDate, CityId
                         FROM {StudentTableName} 
                         WHERE Id = @Id";

            var myReader = await connection.ExecuteReaderAsync(
                sql,
                new { id }
                );

            var names = new List<string>();
            while (myReader.Read())
            {
                names.Add(myReader.GetString(0));
            }

            return names.ToArray();
        }
        public async Task<Student[]> GetStudents()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT Id, Name, BirthDate, CityId
                         FROM {StudentTableName}";

            var students = await connection.QueryAsync<Student>(sql);

            return students.ToArray();
        }

        public async Task<int> GetTotalStudents()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = $"SELECT Count(*) FROM {StudentTableName}";

            var totalStudents = await connection.ExecuteScalarAsync<int>(sql);

            return totalStudents;
        }

        public async Task<StudentWithMark> GetStudentWithMark(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT Id, Name, BirthDate,CityId FROM {StudentTableName} WHERE Id = @Id
                         SELECT Mark FROM dbo.Marks WHERE StudentId = @Id";

            var reader = await connection.QueryMultipleAsync(sql, new { id });

            var student = reader.ReadFirst<StudentWithMark>();
            var marks = reader.Read<int>().ToArray();
            student.Marks = marks;

            return student;
        }

        public async Task<StudentWithCity[]> GetStudentsByCity(string cityName)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT S.Id, S.Name, S.BirthDate, S.CityId,
                                C.Id, C.Name
                         FROM {StudentTableName} S
                         LEFT JOIN dbo.Cities C ON C.Id = S.CityId
                         WHERE C.Name = @Name";

            var studentWithCity = await connection.QueryAsync<Student, City, StudentWithCity>(
                sql,
                (student, city) =>
                {
                    var sWithCity = new StudentWithCity
                    {
                        Id = student.Id,
                        Name = student.Name,
                        BirthDate = student.BirthDate,
                        CityId = student.CityId,
                        City = city
                    };

                    return sWithCity;
                },
                new { name = cityName },
                splitOn: "Id");

            return studentWithCity.ToArray();
        }

        public async Task<StudentWithCourses[]> GetStudentSubscriber(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT S.Id, S.Name, S.BirthDate, S.CityId,
                                C.SubjectId AS Id, C.SubscribedDate,
                                SB.Id, SB.Name
                         FROM {StudentTableName} S
                         INNER JOIN dbo.Courses C ON C.StudentId = S.Id
                         LEFT JOIN dbo.Subjects SB ON SB.Id = C.SubjectId
                         WHERE S.Id = @Id";

            var studentDict = new Dictionary<int, StudentWithCourses>();

            var studentWithCourses = await connection.QueryAsync<Student, Course, Subject, StudentWithCourses>(
                sql,
                (student, course, subject) =>
                {
                    StudentWithCourses studentWithCourses;
                    if (!studentDict.TryGetValue(student.Id, out studentWithCourses))
                    {
                        studentWithCourses = new StudentWithCourses
                        {
                            Id = student.Id,
                            Name = student.Name,
                            BirthDate = student.BirthDate,
                            CityId = student.CityId,
                            Courses = new List<CourseWithSubject>()
                        };

                        studentDict.Add(studentWithCourses.Id, studentWithCourses);
                    }

                    studentWithCourses.Courses.Add(
                       new CourseWithSubject()
                       {
                           StudentId = course.StudentId,
                           SubjectId = course.SubjectId,
                           SubscribedDate = course.SubscribedDate,
                           Name = subject.Name
                       });

                    return studentWithCourses;
                },
                new { id },
                splitOn: "Id");

            return studentDict.Values.ToArray();
        }
        public async Task<StudentWithCourses[]> GetStudentSubscriberMoreThanSevenJoin(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @$"SELECT S.Id, S.Name, S.BirthDate, S.CityId,
                                C.SubjectId AS Id, C.SubscribedDate,
                                SB.Id, SB.Name
                         FROM {StudentTableName} S
                         INNER JOIN dbo.Courses C ON C.StudentId = S.Id
                         LEFT JOIN dbo.Subjects SB ON SB.Id = C.SubjectId
                         WHERE S.Id = @Id";

            var studentDict = new Dictionary<int, StudentWithCourses>();

            var studentWithCourses = await connection.QueryAsync(
                sql,
                new[]
                {
                    typeof(Student),
                    typeof(Course),
                    typeof(Subject)
                },
                obj =>
                {
                    var student = obj[0] as Student;
                    var course = obj[1] as Course;
                    var subject = obj[2] as Subject;

                    StudentWithCourses studentWithCourses;
                    if (!studentDict.TryGetValue(student.Id, out studentWithCourses))
                    {
                        studentWithCourses = new StudentWithCourses
                        {
                            Id = student.Id,
                            Name = student.Name,
                            BirthDate = student.BirthDate,
                            CityId = student.CityId,
                            Courses = new List<CourseWithSubject>()
                        };

                        studentDict.Add(studentWithCourses.Id, studentWithCourses);
                    }

                    studentWithCourses.Courses.Add(
                       new CourseWithSubject()
                       {
                           StudentId = course.StudentId,
                           SubjectId = course.SubjectId,
                           SubscribedDate = course.SubscribedDate,
                           Name = subject.Name
                       });

                    return studentWithCourses;
                },
                new { id },
                splitOn: "Id");

            return studentDict.Values.ToArray();
        }

        public async Task ReplaceStudent(int oldIdStudent, Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var sqlDelete = @$"DELETE {StudentTableName} WHERE Id=@Id";
                    var rowDeleted = await connection.ExecuteAsync(
                        sqlDelete,
                        new { Id = oldIdStudent },
                        transaction: transaction
                        );

                    var rowInsert = 0;
                    if (rowDeleted == 1)
                    {
                        var sqlInsert = @$"INSERT INTO {StudentTableName} (Name, BirthDate, CityId)
                                           VALUES (@Name, @BirthDate, @CityId)";
                        rowInsert = await connection.ExecuteAsync(
                            sqlInsert,
                            student,
                            transaction: transaction
                            );
                    }

                    if (rowInsert == 1)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }

        }
    }
}
