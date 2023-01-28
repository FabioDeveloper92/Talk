CREATE TABLE dbo.Students (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar(50),
    [BirthDate] DATETIME,
    [CityId] int
);

CREATE TABLE dbo.Cities (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar(50)
);

CREATE TABLE dbo.Subjects (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [Name] varchar(50)
);

CREATE TABLE dbo.Courses (
    [StudentId] int,
    [SubjectId] int,
    [SubscribedDate] DATETIME,
);

CREATE TABLE dbo.Marks (
    [StudentId] int,
    [SubjectId] int,
    [Date] DATETIME,
	[Mark] int
);

INSERT INTO dbo.Students
VALUES('David', '1998-05-01', 1),
('Louis', '2000-12-11', 1),
('Jenny', '2000-07-23', 2)

INSERT INTO dbo.Cities
VALUES('NY'), ('LA')

INSERT INTO dbo.Subjects
VALUES('Maths'), ('English'), ('History')

INSERT INTO dbo.Courses
VALUES (1, 1, '2022-10-10'), (1, 2, '2022-11-07'),
(2, 3, '2023-01-01'), (2, 2, '2022-12-12')

INSERT INTO dbo.Marks
VALUES (1,1,'2023-01-07', 10),
(1,1, '2023-01-08', 8),
(1,2, '2023-01-08', 5)

CREATE OR ALTER PROCEDURE dbo.InsertStudent
@Name VARCHAR(50),
@BirthDate DATETIME,
@CityId INT
AS
BEGIN

	INSERT INTO dbo.Students([Name], BirthDate, CityId)
    VALUES(@Name, @BirthDate, @CityId)

END
GO

CREATE OR ALTER PROCEDURE dbo.GetStudent
@Id INT
AS
BEGIN

	SELECT Id, [Name], BirthDate, CityId
	FROM dbo.Students 
	WHERE Id = @Id

END
GO

CREATE OR ALTER PROCEDURE dbo.GetStudents
AS
BEGIN

	SELECT Id, [Name], BirthDate, CityId
	FROM dbo.Students 

END
GO

CREATE OR ALTER PROCEDURE dbo.GetTotalStudents
AS
BEGIN

	SELECT Count(*) 
	FROM dbo.Students 

END
GO

CREATE OR ALTER PROCEDURE dbo.GetStudentWithMark
@Id INT
AS
BEGIN

	SELECT Id, Name, BirthDate,CityId
	FROM dbo.Students WHERE Id = @Id

    SELECT Mark FROM dbo.Marks WHERE StudentId = @Id
END
GO

CREATE OR ALTER PROCEDURE dbo.GetStudentsByCity
@Name VARCHAR(50)
AS
BEGIN

	SELECT S.Id, S.Name, S.BirthDate, S.CityId,
           C.Id, C.Name
	FROM dbo.Students S
	LEFT JOIN dbo.Cities C ON C.Id = S.CityId
	WHERE C.Name = @Name
END
GO


CREATE OR ALTER PROCEDURE dbo.GetStudentSubscriber
@Id INT
AS
BEGIN
	SELECT S.Id, S.Name, S.BirthDate, S.CityId,
		   C.SubjectId AS Id, C.SubscribedDate,
		   SB.Id, SB.Name
	FROM dbo.Students S
	INNER JOIN dbo.Courses C ON C.StudentId = S.Id
	LEFT JOIN dbo.Subjects SB ON SB.Id = C.SubjectId
	WHERE S.Id = @Id
END
GO

CREATE OR ALTER PROCEDURE dbo.DeleteStudent
@Id INT
AS
BEGIN
	
	DELETE dbo.Students WHERE Id = @Id

END
GO