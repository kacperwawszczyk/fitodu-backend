using System;
using System.Linq;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Model.Migrations;
using Scheduledo.Service;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Scheduledo.Test
{
    public class IntegrationTestFixture
    {
        public IServiceProvider SP { get; }

        public User User { get; set; }

        public IntegrationTestFixture()
        {
            var services = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", false, true);

            var configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.RegisterDatabase(configuration);
            services.RegisterServices();

            SP = services.BuildServiceProvider();

            var context = SP.GetService<Context>();
            Configuration.Initialize(context);

            CreateTestUser(context);
        }

        private void CreateTestUser(Context context)
        {
            var userService = SP.GetService<IUserService>();
            var email = $"{Guid.NewGuid()}@gmail.com";

            var result = userService.Register(new RegisterUserInput
            {
                FullName = $"{Guid.NewGuid()} {Guid.NewGuid()}",
                Email = email,
                Password = Guid.NewGuid().ToString(),
                Url = Guid.NewGuid().ToString()
            }).Result;

            result.Success.Should().BeTrue();

            User = context.Users.Include(x => x.Company).First(x => x.Email == email);
        }
    }
}