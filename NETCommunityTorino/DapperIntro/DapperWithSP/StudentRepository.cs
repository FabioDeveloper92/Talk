using Dapper;
using DapperIntro.Model;
using System.Data;
using System.Data.SqlClient;

namespace DapperIntro.DapperWithSP
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertStudent(Student student)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "dbo.InsertStudent",
                new
                {
                    student.Name,
                    student.BirthDate,
                    student.CityId
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<Student> GetStudent(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var student = await connection.QuerySingleAsync<Student>(
                "dbo.GetStudent",
                new { id },
                commandType: CommandType.StoredProcedure
                );

            return student;
        }
      
        public async Task<Student[]> GetStudents()
        {
            using var connection = new SqlConnection(_connectionString);

            var students = await connection.QueryAsync<Student>(
                "dbo.GetStudents",
                commandType: CommandType.StoredProcedure);

            return students.ToArray();
        }

        public async Task<int> GetTotalStudents()
        {
            using var connection = new SqlConnection(_connectionString);
            var totalStudents = await connection.ExecuteScalarAsync<int>(
                "dbo.GetTotalStudents",
                commandType: CommandType.StoredProcedure
                );

            return totalStudents;
        }

        public async Task<StudentWithMark> GetStudentWithMark(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var reader = await connection.QueryMultipleAsync(
                "dbo.GetStudentWithMark",
                new { id },
                commandType: CommandType.StoredProcedure
            );

            var student = reader.ReadFirst<StudentWithMark>();
            var marks = reader.Read<int>().ToArray();
            student.Marks = marks;

            return student;
        }

        public async Task<StudentWithCity[]> GetStudentsByCity(string cityName)
        {
            using var connection = new SqlConnection(_connectionString);

            var studentWithCity = await connection.QueryAsync<Student, City, StudentWithCity>(
                "dbo.GetStudentsByCity",
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
                commandType: CommandType.StoredProcedure,
                splitOn: "Id");

            return studentWithCity.ToArray();
        }

        public async Task<StudentWithCourses[]> GetStudentSubscriber(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var studentDict = new Dictionary<int, StudentWithCourses>();

            var studentWithCourses = await connection.QueryAsync<Student, Course, Subject, StudentWithCourses>(
                "dbo.GetStudentSubscriber",
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
                commandType: CommandType.StoredProcedure,
                splitOn: "Id");

            return studentDict.Values.ToArray();
        }

        public async Task<StudentWithCourses[]> GetStudentSubscriberMoreThanSevenJoin(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var studentDict = new Dictionary<int, StudentWithCourses>();

            var studentWithCourses = await connection.QueryAsync(
                "dbo.GetStudentSubscriber",
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
                commandType: CommandType.StoredProcedure,
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
                    var rowDeleted = await connection.ExecuteAsync(
                        "dbo.DeleteStudent",
                        new { Id = oldIdStudent },
                        commandType: CommandType.StoredProcedure,
                        transaction: transaction
                        );

                    var rowInsert = 0;
                    if (rowDeleted == 1)
                    {
                        rowInsert = await connection.ExecuteAsync(
                            "dbo.InsertStudent",
                            new
                            {
                                student.Name,
                                student.BirthDate,
                                student.CityId
                            },
                            commandType: CommandType.StoredProcedure,
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
