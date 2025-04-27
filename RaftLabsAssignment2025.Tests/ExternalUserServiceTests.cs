using Moq;
using RaftLabsAssignment2025.Clients;
using RaftLabsAssignment2025.Models;
using RaftLabsAssignment2025.Services;

namespace RaftLabsAssignment2025.Tests
{
    public class ExternalUserServiceTests
    {
        private readonly Mock<IReqResClient> _mockClient;
        private readonly ExternalUserService _service;

        public ExternalUserServiceTests()
        {
            _mockClient = new Mock<IReqResClient>();
            _service = new ExternalUserService(_mockClient.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { id = userId, first_name = "John", last_name = "Doe", email = "john.doe@example.com" };
            _mockClient.Setup(c => c.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var user = await _service.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(expectedUser.first_name, user.first_name);
            Assert.Equal(expectedUser.last_name, user.last_name);
            Assert.Equal(expectedUser.email, user.email);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUsersList()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { id = 1, first_name = "John", last_name = "Doe" },
                new User { id = 2, first_name = "Jane", last_name = "Smith" }
            };

            _mockClient.Setup(c => c.GetAllUsersAsync()).ReturnsAsync(expectedUsers);

            // Act
            var users = await _service.GetAllUsersAsync();

            // Assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
        }
    }
}
