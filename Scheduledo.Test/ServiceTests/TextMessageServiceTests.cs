using Scheduledo.Core.Helpers;
using Scheduledo.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Scheduledo.Test
{
    [Collection(nameof(IntegrationTestCollection))]
    public class TextMessageServiceTests
    {
        private IntegrationTestFixture _fixture;

        public TextMessageServiceTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void send()
        {
            var textMessageService = _fixture.SP.GetService<ITextMessageService>();
            var to = "+48123123123";
            var text = "This is a test message";

            Language.SetLanguage(Language.DefaultCulture);
            var response = textMessageService.Send(to, text);

            response.messages[0].status.Should().Be("0");

            text = "Testowa wiadomość z ąężźćśł";
            Language.SetLanguage("pl");
            response = textMessageService.Send(to, text);

            response.messages[0].status.Should().Be("0");
        }
    }
}
