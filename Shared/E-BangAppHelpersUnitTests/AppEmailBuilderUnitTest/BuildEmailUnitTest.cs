using E_BangAppEmailBuilder.src.Builder;
using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangAppEmailBuilder.src.BuildersDto.Header;
using Shouldly;
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
            message.ShouldNotBeNull();
            message.Message.ShouldNotBeNull();
            message.Message.ShouldContain("jj.marczak@gmail.com");
            message.Message.ShouldContain("http://test.com/asdasdas1231e2");
            message.Message.ShouldContain(DateTime.Now.Year.ToString());

        }
        [Fact]
        public void EmailHeaderShouldBeEmpty()
        {
            var emailBuilder = new EmailBuilder();
            EmailMessage message = emailBuilder
                .GenerateBody(new RegistrationBodyBuilder
                {
                    Token = "http://test.com/asdasdas1231e2"
                })
                .GenerateFooter()
                .BuildMessage();
            message.ShouldNotBeNull();
            message.Message.ShouldNotBeNull();
            message.Message.ShouldNotContain("jj.marczak@gmail.com");
            message.Message.ShouldContain(DateTime.Now.Year.ToString());

        }
        [Fact]
        public void EmailShouldHaveError()
        {
            var emailBuilder = new EmailBuilder();

            Action action = () =>
            {
                EmailMessage message = emailBuilder.BuildMessage();
            };
            action.ShouldThrow<Exception>("Empty Body");
        }

    }
}
