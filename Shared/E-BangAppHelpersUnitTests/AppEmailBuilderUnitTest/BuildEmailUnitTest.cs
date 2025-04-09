using E_BangAppEmailBuilder.src.Builder;
using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangAppEmailBuilder.src.BuildersDto.Header;

namespace E_BangAppHelpersUnitTests.AppEmailBuilderUnitTest
{
    public class BuildEmailUnitTest
    {
        [Fact]
        public void EmailBuilderShouldBeOk()
        {
            var emailBuilder = new EmailBuilder();

            EmailMessage message = emailBuilder.GenerateHeader(new HeaderDefaultBuilderOptions()
            {
                Email = "jj.marczak@gmail.com"
            }).GenerateBody(new RegistrationBodyBuilder
            {
                Email = "jj.marczak@gmail.com",
                Token = "http://test.com/asdasdas1231e2"
            })
            .GenerateFooter()
            .BuildMessage();

            Assert.NotNull(message);
            Assert.NotNull(message.Message);
            //Assert.Contains(message.Message, "jj.marczak@gmail.com");
            //Assert.Contains(message.Message, "http://test.com/asdasdas1231e2");
            //Assert.Contains(message.Message, "2025");

        }
    }
}
