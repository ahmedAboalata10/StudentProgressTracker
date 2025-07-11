using Microsoft.Extensions.Logging;
using Moq;
using StudentProgress.API.IRepositories;
using StudentProgress.API.Models.Students;
using StudentProgress.API.Services.Students;
namespace StudentProgress.API.Tests.Services
{
    namespace StudentProgress.API.Tests.Services.Students
    {
        public class StudentServiceTests
        {
            private readonly Mock<IStudentRepository> _mockStudentRepo;
            private readonly Mock<ILogger<StudentService>> _mockLogger;
            private readonly StudentService _studentService;

            public StudentServiceTests()
            {
                _mockStudentRepo = new Mock<IStudentRepository>();
                _mockLogger = new Mock<ILogger<StudentService>>();
                _studentService = new StudentService(_mockStudentRepo.Object, _mockLogger.Object);
            }

            #region GetAllStudentsAsync Tests

            [Fact]
            public async Task GetAllStudentsAsync_ReturnsAllStudents_WhenSuccessful()
            {
                // Arrange
                var expectedStudents = new List<Student>
            {
                new Student { Id = Guid.NewGuid(), FullName = "John Doe" },
                new Student { Id = Guid.NewGuid(), FullName = "Jane Smith" }
            };
                _mockStudentRepo.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(expectedStudents);

                // Act
                var result = await _studentService.GetAllStudentsAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedStudents.Count, result.Count());
                Assert.Equal(expectedStudents, result);
                _mockStudentRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
            }

            [Fact]
            public async Task GetAllStudentsAsync_LogsInformation_WhenCalled()
            {
                // Arrange
                var students = new List<Student>();
                _mockStudentRepo.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(students);

                // Act
                await _studentService.GetAllStudentsAsync();

                // Assert
                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching all students")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            [Fact]
            public async Task GetAllStudentsAsync_LogsError_WhenExceptionOccurs()
            {
                // Arrange
                var exception = new Exception("Database error");
                _mockStudentRepo.Setup(repo => repo.GetAllAsync())
                               .ThrowsAsync(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => _studentService.GetAllStudentsAsync());

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error occurred while fetching all students")),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            #endregion

            #region GetStudentByIdAsync Tests

            [Fact]
            public async Task GetStudentByIdAsync_ReturnsStudent_WhenStudentExists()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var expectedStudent = new Student { Id = studentId, FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync(expectedStudent);

                // Act
                var result = await _studentService.GetStudentByIdAsync(studentId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedStudent.Id, result.Id);
                Assert.Equal(expectedStudent.FullName, result.FullName);
                _mockStudentRepo.Verify(repo => repo.GetByIdAsync(studentId), Times.Once);
            }

            [Fact]
            public async Task GetStudentByIdAsync_ReturnsNull_WhenStudentNotFound()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync((Student?)null);

                // Act
                var result = await _studentService.GetStudentByIdAsync(studentId);

                // Assert
                Assert.Null(result);
                _mockStudentRepo.Verify(repo => repo.GetByIdAsync(studentId), Times.Once);
            }

            [Fact]
            public async Task GetStudentByIdAsync_LogsInformation_WhenCalled()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync((Student?)null);

                // Act
                await _studentService.GetStudentByIdAsync(studentId);

                // Assert
                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Fetching student with ID: {studentId}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            [Fact]
            public async Task GetStudentByIdAsync_LogsError_WhenExceptionOccurs()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var exception = new Exception("Database error");
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ThrowsAsync(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => _studentService.GetStudentByIdAsync(studentId));

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error occurred while fetching student by ID: {studentId}")),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            #endregion

            #region GetStudentWithProgressAsync Tests

            [Fact]
            public async Task GetStudentWithProgressAsync_ReturnsStudentWithProgress_WhenSuccessful()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var expectedStudent = new Student { Id = studentId, FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.GetWithProgressAsync(studentId))
                               .ReturnsAsync(expectedStudent);

                // Act
                var result = await _studentService.GetStudentWithProgressAsync(studentId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedStudent.Id, result.Id);
                _mockStudentRepo.Verify(repo => repo.GetWithProgressAsync(studentId), Times.Once);
            }

            [Fact]
            public async Task GetStudentWithProgressAsync_ReturnsNull_WhenStudentNotFound()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                _mockStudentRepo.Setup(repo => repo.GetWithProgressAsync(studentId))
                               .ReturnsAsync((Student?)null);

                // Act
                var result = await _studentService.GetStudentWithProgressAsync(studentId);

                // Assert
                Assert.Null(result);
                _mockStudentRepo.Verify(repo => repo.GetWithProgressAsync(studentId), Times.Once);
            }

            [Fact]
            public async Task GetStudentWithProgressAsync_LogsError_WhenExceptionOccurs()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var exception = new Exception("Database error");
                _mockStudentRepo.Setup(repo => repo.GetWithProgressAsync(studentId))
                               .ThrowsAsync(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => _studentService.GetStudentWithProgressAsync(studentId));

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error occurred while fetching student with progress. ID: {studentId}")),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            #endregion

            #region AddStudentAsync Tests

            [Fact]
            public async Task AddStudentAsync_AddsStudent_WhenSuccessful()
            {
                // Arrange
                var student = new Student { Id = Guid.NewGuid(), FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.AddAsync(student))
                               .Returns(Task.CompletedTask);
                _mockStudentRepo.Setup(repo => repo.SaveAsync())
                               .Returns(Task.CompletedTask);

                // Act
                await _studentService.AddStudentAsync(student);

                // Assert
                _mockStudentRepo.Verify(repo => repo.AddAsync(student), Times.Once);
                _mockStudentRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            }

            [Fact]
            public async Task AddStudentAsync_LogsInformationBeforeAndAfter_WhenSuccessful()
            {
                // Arrange
                var student = new Student { Id = Guid.NewGuid(), FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.AddAsync(student))
                               .Returns(Task.CompletedTask);
                _mockStudentRepo.Setup(repo => repo.SaveAsync())
                               .Returns(Task.CompletedTask);

                // Act
                await _studentService.AddStudentAsync(student);

                // Assert
                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Adding new student: {student.FullName}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Student added successfully: {student.FullName}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            [Fact]
            public async Task AddStudentAsync_LogsError_WhenExceptionOccurs()
            {
                // Arrange
                var student = new Student { Id = Guid.NewGuid(), FullName = "John Doe" };
                var exception = new Exception("Database error");
                _mockStudentRepo.Setup(repo => repo.AddAsync(student))
                               .ThrowsAsync(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => _studentService.AddStudentAsync(student));

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error occurred while adding student: {student.FullName}")),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            #endregion

            #region UpdateStudentAsync Tests

            [Fact]
            public async Task UpdateStudentAsync_UpdatesStudent_WhenSuccessful()
            {
                // Arrange
                var student = new Student { Id = Guid.NewGuid(), FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.Update(student));
                _mockStudentRepo.Setup(repo => repo.SaveAsync())
                               .Returns(Task.CompletedTask);

                // Act
                await _studentService.UpdateStudentAsync(student);

                // Assert
                _mockStudentRepo.Verify(repo => repo.Update(student), Times.Once);
                _mockStudentRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            }

            [Fact]
            public async Task UpdateStudentAsync_LogsInformationBeforeAndAfter_WhenSuccessful()
            {
                // Arrange
                var student = new Student { Id = Guid.NewGuid(), FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.Update(student));
                _mockStudentRepo.Setup(repo => repo.SaveAsync())
                               .Returns(Task.CompletedTask);

                // Act
                await _studentService.UpdateStudentAsync(student);

                // Assert
                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Updating student: {student.Id}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Student updated successfully: {student.Id}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            [Fact]
            public async Task UpdateStudentAsync_LogsError_WhenExceptionOccurs()
            {
                // Arrange
                var student = new Student { Id = Guid.NewGuid(), FullName = "John Doe" };
                var exception = new Exception("Database error");
                _mockStudentRepo.Setup(repo => repo.Update(student))
                               .Throws(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => _studentService.UpdateStudentAsync(student));

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error occurred while updating student: {student.Id}")),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            #endregion

            #region DeleteStudentAsync Tests

            [Fact]
            public async Task DeleteStudentAsync_DeletesStudent_WhenStudentExists()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var student = new Student { Id = studentId, FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync(student);
                _mockStudentRepo.Setup(repo => repo.DeleteAsync(studentId))
                               .Returns(Task.CompletedTask);
                _mockStudentRepo.Setup(repo => repo.SaveAsync())
                               .Returns(Task.CompletedTask);

                // Act
                await _studentService.DeleteStudentAsync(studentId);

                // Assert
                _mockStudentRepo.Verify(repo => repo.GetByIdAsync(studentId), Times.Once);
                _mockStudentRepo.Verify(repo => repo.DeleteAsync(studentId), Times.Once);
                _mockStudentRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            }

            [Fact]
            public async Task DeleteStudentAsync_DoesNotDelete_WhenStudentNotFound()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync((Student?)null);

                // Act
                await _studentService.DeleteStudentAsync(studentId);

                // Assert
                _mockStudentRepo.Verify(repo => repo.GetByIdAsync(studentId), Times.Once);
                _mockStudentRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
                _mockStudentRepo.Verify(repo => repo.SaveAsync(), Times.Never);
            }

            [Fact]
            public async Task DeleteStudentAsync_LogsSuccess_WhenStudentDeleted()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var student = new Student { Id = studentId, FullName = "John Doe" };
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync(student);
                _mockStudentRepo.Setup(repo => repo.DeleteAsync(studentId))
                               .Returns(Task.CompletedTask);
                _mockStudentRepo.Setup(repo => repo.SaveAsync())
                               .Returns(Task.CompletedTask);

                // Act
                await _studentService.DeleteStudentAsync(studentId);

                // Assert
                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Student deleted successfully: {studentId}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            [Fact]
            public async Task DeleteStudentAsync_LogsWarning_WhenStudentNotFound()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ReturnsAsync((Student?)null);

                // Act
                await _studentService.DeleteStudentAsync(studentId);

                // Assert
                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Warning,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Delete failed. Student not found: {studentId}")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            [Fact]
            public async Task DeleteStudentAsync_LogsError_WhenExceptionOccurs()
            {
                // Arrange
                var studentId = Guid.NewGuid();
                var exception = new Exception("Database error");
                _mockStudentRepo.Setup(repo => repo.GetByIdAsync(studentId))
                               .ThrowsAsync(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => _studentService.DeleteStudentAsync(studentId));

                _mockLogger.Verify(
                    logger => logger.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Error occurred while deleting student: {studentId}")),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }

            #endregion
        }
    }
}
