using Xunit;
using TravelSureAPI.Models;
using TravelSureAPI.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace TravelSureAPI.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void Register_ValidUser_ShouldReturnSuccess()
        {
            // Arrange
            var controller = new UsersController();
            var newUser = new UserAccount
            {
                UserName = "Laila",
                Email = "laila@example.com",
                PasswordHash = "Pass123",
                MembershipTier = "Basic"
            };

            // Act
            var result = controller.Register(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public void Register_WithExistingEmail_ShouldReturnBadRequest()
        {
            //arrange
            var controller = new UsersController();

            var existingUser = new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserName = "Ahmed",
                Email = "ahmed@example.com",
                PasswordHash = "A123A",
                MembershipTier = "Basic",
            };
            UserStore.AddUser(existingUser);

            var newUser = new UserAccount
            {
                UserName = "Hassan",
                Email = "ahmed@example.com", // same email
                PasswordHash = "67890",
                MembershipTier = "Basic"
            };
            //Act
            var result = controller.Register(newUser);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);


        }
        [Fact]
        public void Register_WithExistingUsername_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = new UsersController();

            var existingUser = new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserName = "Ahmed",
                Email = "ahmed@example.com",
                PasswordHash = "A123A",
                MembershipTier = "Basic"
            };
            UserStore.AddUser(existingUser);

            var newUser = new UserAccount
            {
                UserName = "Ahmed", //same user name
                Email = "newemail@example.com",
                PasswordHash = "B456B",
                MembershipTier = "Basic"
            };

            // Act
            var result = controller.Register(newUser);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }
        [Fact]
        public void Login_WithCorrectCredentials_ShouldReturnSuccess()
        {
            // Arrange
            var controller = new UsersController();

            var user = new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserName = "Omar",
                Email = "omar@example.com",
                PasswordHash = "CorrectPass",
                MembershipTier = "Basic"
            };

            user.PasswordHash = HashPassword(user.PasswordHash);
            UserStore.AddUser(user);

            var loginRequest = new LoginRequest
            {
                Email = "omar@example.com",
                Password = "CorrectPass"
            };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public void Login_WithWrongPassword_ShouldReturnUnauthorized()
        {
            // Arrange
            var controller = new UsersController();

            var user = new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserName = "Nour",
                Email = "nour@example.com",
                PasswordHash = "CorrectPass",
                MembershipTier = "Basic"
            };

            user.PasswordHash = HashPassword(user.PasswordHash);
            UserStore.AddUser(user);

            var loginRequest = new LoginRequest
            {
                Email = "nour@example.com",
                Password = "WrongPass"
            };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorized.StatusCode);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }


}