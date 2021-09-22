CREATE TABLE ApplicationUser
(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    RoleId INT NOT NULL,
    EmployeeId INT NULL,
    UserName VARCHAR(50) NOT NULL,
    PasswordHash VARCHAR(64) NOT NULL,
    PasswordSalt BINARY(16) NOT NULL,
    IsActive BIT NOT NULL,
    CreatedBy INT NULL,
    CreatedDate DATETIME NOT NULL,
    UpdatedBy INT NULL,
    UpdatedDate DATETIME NULL
)

CREATE TABLE UserRole
(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    RoleName VARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL,
    CreatedBy INT NULL,
    CreatedDate DATETIME NOT NULL,
    UpdatedBy INT NULL,
    UpdatedDate DATETIME NULL
)

CREATE TABLE Employee
(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EmployeeIdNo VARCHAR(50) NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    JoiningDate DATETIME NULL,
    Email VARCHAR(50) NULL,
    ContactNo VARCHAR(50) NULL,
    NationalId VARCHAR(50) NULL,
    OfficeName VARCHAR(50) NULL,
    Department VARCHAR(50) NULL,
    Designaton VARCHAR(50) NULL,
    EmployeeImagePath VARCHAR(300) NULL,
    IsActive BIT NOT NULL,
    CreatedBy INT NULL,
    CreatedDate DATETIME NOT NULL,
    UpdatedBy INT NULL,
    UpdatedDate DATETIME NULL
)

CREATE TABLE Attendance
(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EmployeeId INT NOT NULL,
    StartDateTime DATETIME NOT NULL,
    EndDateTime DATETIME NULL,
    IsActive BIT NOT NULL,
    CreatedBy INT NOT NULL,
    CreatedDate DATETIME NOT NULL
)


ALTER TABLE ApplicationUser ADD CONSTRAINT FK_ApplicationUser_RoleId
FOREIGN KEY (RoleId)
REFERENCES UserRole (Id)

ALTER TABLE ApplicationUser ADD CONSTRAINT FK_ApplicationUser_EmployeeId
FOREIGN KEY (EmployeeId)
REFERENCES Employee (Id)

ALTER TABLE ApplicationUser ADD CONSTRAINT FK_ApplicationUser_CreatedBy
FOREIGN KEY (CreatedBy)
REFERENCES ApplicationUser (Id)

ALTER TABLE ApplicationUser ADD CONSTRAINT FK_ApplicationUser_UpdatedBy
FOREIGN KEY (UpdatedBy)
REFERENCES ApplicationUser (Id)

ALTER TABLE UserRole ADD CONSTRAINT FK_UserRole_CreatedBy
FOREIGN KEY (CreatedBy)
REFERENCES ApplicationUser (Id)

ALTER TABLE UserRole ADD CONSTRAINT FK_UserRole_UpdatedBy
FOREIGN KEY (UpdatedBy)
REFERENCES ApplicationUser (Id)

ALTER TABLE Employee ADD CONSTRAINT FK_Employee_CreatedBy
FOREIGN KEY (CreatedBy)
REFERENCES ApplicationUser (Id)

ALTER TABLE Employee ADD CONSTRAINT FK_Employee_UpdatedBy
FOREIGN KEY (UpdatedBy)
REFERENCES ApplicationUser (Id)

ALTER TABLE Attendance ADD CONSTRAINT FK_Attendance_EmployeeId
FOREIGN KEY (EmployeeId)
REFERENCES Employee (Id)

ALTER TABLE Attendance ADD CONSTRAINT FK_Attendance_CreatedBy
FOREIGN KEY (CreatedBy)
REFERENCES ApplicationUser (Id)


INSERT INTO UserRole (RoleName, IsActive, CreatedDate) VALUES
('Administrator', 1, GETDATE()),
('Stuff', 1, GETDATE())

GO


CREATE Procedure SP_AttendanceList
    @PageNo INT,
    @DataPerPage INT,
    @EmployeeIdNo VARCHAR(50),
    @StrStartDate VARCHAR(20),
    @StrEndDate VARCHAR(20)
AS
BEGIN
    DECLARE
    @StartDate DATE = CONVERT(DATE, @StrStartDate),
    @EndDate DATE = CONVERT(DATE, @StrEndDate),
    @SkipCount INT = (@PageNo - 1) * @DataPerPage;

    SELECT E.EmployeeIdNo, E.FirstName + ' ' + E.LastName AS Name,
    CONVERT(VARCHAR(30), A.StartDateTime, 22) AS StartDateTime,
    (CASE WHEN A.EndDateTime IS NULL THEN '' ELSE CONVERT(VARCHAR(30), A.EndDateTime, 22) END) AS EndDateTime
    FROM Employee AS E
    JOIN Attendance AS A ON E.Id = A.EmployeeId
    WHERE CONVERT(DATE, StartDateTime) BETWEEN @StartDate AND @EndDate
    AND (@EmployeeIdNo IS NULL OR @EmployeeIdNo = '' OR E.EmployeeIdNo LIKE '%' + @EmployeeIdNo + '%')
    ORDER BY StartDateTime
    OFFSET @SkipCount ROWS
    FETCH NEXT @DataPerPage ROWS ONLY
END
GO



CREATE Procedure SP_AttendanceListTotal
    @EmployeeIdNo VARCHAR(50),
    @StrStartDate VARCHAR(20),
    @StrEndDate VARCHAR(20)
AS
BEGIN
    DECLARE
    @StartDate DATE = CONVERT(DATE, @StrStartDate),
    @EndDate DATE = CONVERT(DATE, @StrEndDate),
    @TotalRows INT = 0

    SELECT @TotalRows = COUNT(A.Id)
    FROM Employee AS E
    JOIN Attendance AS A ON E.Id = A.EmployeeId
    WHERE CONVERT(DATE, StartDateTime) BETWEEN @StartDate AND @EndDate
    AND (@EmployeeIdNo IS NULL OR @EmployeeIdNo = '' OR E.EmployeeIdNo LIKE '%' + @EmployeeIdNo + '%')
    SET @TotalRows = ISNULL(@TotalRows, 0)

    SELECT @TotalRows AS 'Total'
END
GO


CREATE Procedure SP_EmployeeList
    @PageNo INT,
    @DataPerPage INT,
    @Code VARCHAR(50),
    @Name VARCHAR(50),
    @Email VARCHAR(50),
    @Department VARCHAR(50),
    @Designation VARCHAR(50),
    @Cellphone VARCHAR(50),
    @OfficeName VARCHAR(50)
AS
BEGIN
    DECLARE
    @SkipCount INT = (@PageNo - 1) * @DataPerPage;

    SELECT
    Id,
    EmployeeIdNo,
    FirstName,
    LastName,
    Email,
    Department,
    Designaton,
    ContactNo,
    OfficeName
    FROM Employee
    WHERE (@Code IS NULL OR @Code = '' OR EmployeeIdNo LIKE '%' + @Code + '%')
    AND (@Name IS NULL OR @Name = '' OR (FirstName + ' ' + LastName) LIKE '%' + @Name + '%')
    AND (@Email IS NULL OR @Email = '' OR Email LIKE '%' + @Email + '%')
    AND (@Department IS NULL OR @Department = '' OR Department LIKE '%' + @Department + '%')
    AND (@Designation IS NULL OR @Designation = '' OR Designaton LIKE '%' + @Designation + '%')
    AND (@Cellphone IS NULL OR @Cellphone = '' OR ContactNo LIKE '%' + @Cellphone + '%')
    AND (@OfficeName IS NULL OR @OfficeName = '' OR OfficeName LIKE '%' + @OfficeName + '%')
    ORDER BY FirstName, LastName
    OFFSET @SkipCount ROWS
    FETCH NEXT @DataPerPage ROWS ONLY
END
GO


CREATE Procedure SP_EmployeeListTotal
    @Code VARCHAR(50),
    @Name VARCHAR(50),
    @Email VARCHAR(50),
    @Department VARCHAR(50),
    @Designation VARCHAR(50),
    @Cellphone VARCHAR(50),
    @OfficeName VARCHAR(50)
AS
BEGIN
    DECLARE
    @TotalRows INT = 0

    SELECT @TotalRows = COUNT(Id)
    FROM Employee
    WHERE (@Code IS NULL OR @Code = '' OR EmployeeIdNo LIKE '%' + @Code + '%')
    AND (@Name IS NULL OR @Name = '' OR (FirstName + ' ' + LastName) LIKE '%' + @Name + '%')
    AND (@Email IS NULL OR @Email = '' OR Email LIKE '%' + @Email + '%')
    AND (@Department IS NULL OR @Department = '' OR Department LIKE '%' + @Department + '%')
    AND (@Designation IS NULL OR @Designation = '' OR Designaton LIKE '%' + @Designation + '%')
    AND (@Cellphone IS NULL OR @Cellphone = '' OR ContactNo LIKE '%' + @Cellphone + '%')
    AND (@OfficeName IS NULL OR @OfficeName = '' OR OfficeName LIKE '%' + @OfficeName + '%')
    SET @TotalRows = ISNULL(@TotalRows, 0)

    SELECT @TotalRows AS 'Total'
END
GO


CREATE Procedure SP_UserList
    @PageNo INT,
    @DataPerPage INT,
    @UserName VARCHAR(50),
    @Role VARCHAR(50)
AS
BEGIN
    DECLARE
    @SkipCount INT = (@PageNo - 1) * @DataPerPage;

    SELECT A.UserName, R.RoleName
    FROM ApplicationUser AS A
    JOIN UserRole AS R ON A.RoleId = R.Id
    WHERE (@UserName IS NULL OR @UserName = '' OR A.UserName LIKE '%' + @UserName + '%')
    AND (@Role IS NULL OR @Role = '' OR R.RoleName LIKE '%' + @Role + '%')
    ORDER BY A.UserName
    OFFSET @SkipCount ROWS
    FETCH NEXT @DataPerPage ROWS ONLY
END
GO


CREATE Procedure SP_UserListTotal
    @UserName VARCHAR(50),
    @Role VARCHAR(50)
AS
BEGIN
    DECLARE
    @TotalRows INT = 0

    SELECT @TotalRows = COUNT(A.Id)
    FROM ApplicationUser AS A
    JOIN UserRole AS R ON A.RoleId = R.Id
    WHERE (@UserName IS NULL OR @UserName = '' OR A.UserName LIKE '%' + @UserName + '%')
    AND (@Role IS NULL OR @Role = '' OR R.RoleName LIKE '%' + @Role + '%')
    SET @TotalRows = ISNULL(@TotalRows, 0)

    SELECT @TotalRows AS 'Total'
END
GO
