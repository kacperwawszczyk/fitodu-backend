using System;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Scheduledo.Test
{
    [Collection(nameof(IntegrationTestCollection))]
    public class EmailMarketingServiceTests
    {
        private IntegrationTestFixture _fixture;

        public EmailMarketingServiceTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void subscribe_and_unsubscribe()
        {
            var emailMarketingService = _fixture.SP.GetService<IEmailMarketingService>();

            var model = new User()
            {
                FullName = "Jan Kowalski",
                Email = $"{Guid.NewGuid()}@gmail.com",
                PhoneNumber = "+48123123123"
            };

            var subscribeResult = await emailMarketingService.Subscribe(model);

            subscribeResult.Success.Should().BeTrue();

            var unsubscribeResult = await emailMarketingService.Unsubscribe(subscribeResult.Data);

            unsubscribeResult.Should().BeTrue();
        }

        [Fact]
        public async void subscribe_existing_and_unsubscribe()
        {
            var emailMarketingService = _fixture.SP.GetService<IEmailMarketingService>();
            var user = _fixture.User;

            var model = new User()
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            var subscribeResult = await emailMarketingService.Subscribe(model);

            subscribeResult.Success.Should().BeTrue();

            var unsubscribeResult = await emailMarketingService.Unsubscribe(subscribeResult.Data);

            unsubscribeResult.Should().BeTrue();
        }
    }
}