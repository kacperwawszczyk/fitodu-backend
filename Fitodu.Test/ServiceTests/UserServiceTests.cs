using System;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Scheduledo.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Scheduledo.Core.Enums;

namespace Scheduledo.Test
{
    [Collection(nameof(IntegrationTestCollection))]
    public class UserServiceTests
    {
        private readonly IntegrationTestFixture _fixture;
        private readonly IUserService _userService;

        public UserServiceTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _userService = _fixture.SP.GetService<IUserService>();
        }

        [Fact]
        public async void register_login_and_refresh()
        {
            var email = $"{Guid.NewGuid()}@gmail.com";
            var password = Guid.NewGuid().ToString();

            var registerResult = await _userService.CoachRegister(new RegisterCoachInput()
            {
                Email = email,
                Password = password,
                Url = Guid.NewGuid().ToString()
            });

            registerResult.Success.Should().BeTrue();

            var tokenResult = await _userService.CreateToken(new CreateTokenInput()
            {
                Email = email,
                Password = password
            });

            tokenResult.Success.Should().BeTrue();
            tokenResult.Data.AccessToken.Should().NotBeNullOrEmpty();

            var refreshTokenResult = await _userService.RefreshToken(new RefreshTokenInput()
            {
               Token = tokenResult.Data.RefreshToken
            });

            refreshTokenResult.Success.Should().BeTrue();
            refreshTokenResult.Data.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void get_user()
        {
            var result = await _userService.Get(_fixture.User.Id);

            result.Success.Should().BeTrue();
            result.Data.Id.Should().Be(_fixture.User.Id);
            result.Data.Email.Should().Be(_fixture.User.Email);
        }

        [Fact]
        public async void forgot_and_reset_password()
        {
            var fullName = Guid.NewGuid().ToString();
            var email = "testowy@scheduledo.com";
            var password = Guid.NewGuid().ToString();

            await _userService.CoachRegister(new RegisterCoachInput()
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Url = Guid.NewGuid().ToString()
            });

            var forgotResult = await _userService.ForgotPassword(email);

            forgotResult.Success.Should().BeTrue();

            var newPassword = Guid.NewGuid().ToString();

            var user = await _fixture.SP.GetService<Context>().Users.FirstAsync(x => x.Email == email);

            var resetResult = await _userService.ResetPassword(new ResetPasswordInput()
            {
                NewPassword = newPassword,
                ResetToken = user.ResetToken,
                UserId = user.Id
            });

            resetResult.Success.Should().BeTrue();

            var tokenResult = await _userService.CreateToken(new CreateTokenInput()
            {
                Email = email,
                Password = newPassword
            });

            tokenResult.Success.Should().BeTrue();
            tokenResult.Data.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void create_get_update_and_delete_user()
        {
            var fullName = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var password = Guid.NewGuid().ToString();
            var adminId = _fixture.User.Id;

            var createResult = await _userService.Create(new CreateUserInput()
            {
                FullName = fullName,
                Email = email,
                Password = password,
                AdminId = adminId
            });

            createResult.Success.Should().BeTrue();

            var listResult = await _userService.GetList(adminId);

            listResult.Success.Should().BeTrue();
            var user = listResult.Data.FirstOrDefault(x => x.Email == email);
            user.FullName.Should().Be(fullName);
            user.Email.Should().Be(email);

            var updatedFullName = Guid.NewGuid().ToString();
            var updatedEmail = $"{Guid.NewGuid()}@gmail.com";

            var updateResult = await _userService.Update(new UpdateUserInput()
            {
                Id = user.Id,
                FullName = updatedFullName,
                Email = updatedEmail,
                Password = password,
                AdminId = adminId
            });

            updateResult.Success.Should().BeTrue();
            updateResult.Data.Should().Be(user.Id);

            var getResult = await _userService.Get(user.Id);

            getResult.Success.Should().BeTrue();
            getResult.Data.FullName.Should().Be(updatedFullName);
            getResult.Data.Email.Should().Be(updatedEmail);

            var deleteResult = await _userService.Delete(adminId, user.Id);

            deleteResult.Success.Should().BeTrue();
            deleteResult.Data.Should().Be(user.Id);

            getResult = await _userService.Get(user.Id);

            getResult.Success.Should().BeFalse();
            getResult.Error.Should().Be(ErrorType.NotFound);
        }
    }
}
