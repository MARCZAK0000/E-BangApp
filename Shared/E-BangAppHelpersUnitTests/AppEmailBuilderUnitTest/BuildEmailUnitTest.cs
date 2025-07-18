using E_BangAppEmailBuilder.src.Abstraction;
using E_BangAppEmailBuilder.src.Builder;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;
using Shouldly;
namespace E_BangAppHelpersUnitTests.AppEmailBuilderUnitTest
{
    public class BuildEmailUnitTest
    {
        [Fact]

        public void RegistrationEmailBuildeShouldBeOk()
        {
            IBuilderEmail builderEmail = new BuilderEmail();
            HeaderDefaultTemplateBuilder header = new HeaderDefaultTemplateBuilder()
            {
                Email = "test@test.com"
            };
            RegistrationBodyBuilder body = new RegistrationBodyBuilder()
            {
                Email = "test@test.com",
                Token = "http://test.com/asdasdas1231e2",
            };
            FooterDefualtTemplateBuilder footer = new FooterDefualtTemplateBuilder()
            {
                Year = DateTime.Now.Year.ToString(),
            };
            var message = builderEmail.GenerateMessage(header,body,footer);
            message.ShouldNotBeNull();
            message.Message.ShouldNotBeNull();
            message.Message.ShouldContain("test@test.com");
            message.Message.ShouldContain("http://test.com/asdasdas1231e2");
            message.Message.ShouldContain(DateTime.Now.Year.ToString());

        }
        [Fact]

        public void TwoWayTokenShouldBeOK()
        {
            IBuilderEmail builderEmail = new BuilderEmail();
            HeaderDefaultTemplateBuilder header = new HeaderDefaultTemplateBuilder()
            {
                Email = "test@test.com"
            };
            TwoWayTokenBodyBuilder body = new TwoWayTokenBodyBuilder()
            {
                Token = "000000",
            };
            FooterDefualtTemplateBuilder footer = new FooterDefualtTemplateBuilder()
            {
                Year = DateTime.Now.Year.ToString(),
            };
            var message = builderEmail.GenerateMessage(header, body, footer);
            message.ShouldNotBeNull();
            message.Message.ShouldNotBeNull();
            message.Message.ShouldContain("test@test.com");
            message.Message.ShouldContain("000000");
            message.Message.ShouldContain(DateTime.Now.Year.ToString());
        }
        [Fact]
        public void ConfirmEmailShouldBeOK()
        {
            IBuilderEmail builderEmail = new BuilderEmail();
            HeaderDefaultTemplateBuilder header = new HeaderDefaultTemplateBuilder()
            {
                Email = "test@test.com"
            };
            ConfirmEmailTokenBodyBuilder body = new ConfirmEmailTokenBodyBuilder()
            {
                Token = "http://test.com/asdasdas1231e2",
            };
            FooterDefualtTemplateBuilder footer = new FooterDefualtTemplateBuilder()
            {
                Year = DateTime.Now.Year.ToString(),
            };
            var message = builderEmail.GenerateMessage(header, body, footer);
            message.ShouldNotBeNull();
            message.Message.ShouldNotBeNull();
            message.Message.ShouldContain("test@test.com");
            message.Message.ShouldContain("http://test.com/asdasdas1231e2");
            message.Message.ShouldContain(DateTime.Now.Year.ToString());
        }
        [Fact]
        public void EmailHeaderShouldBeEmpty()
        {
            IBuilderEmail builderEmail = new BuilderEmail();
            HeaderDefaultTemplateBuilder header = new HeaderDefaultTemplateBuilder()
            {
               
            };
            RegistrationBodyBuilder body = new RegistrationBodyBuilder()
            {
                Email = "test@test.com",
                Token = "http://test.com/asdasdas1231e2",
            };
            FooterDefualtTemplateBuilder footer = new FooterDefualtTemplateBuilder()
            {
                Year = DateTime.Now.Year.ToString(),
            };
            var message = builderEmail.GenerateMessage(header, body, footer);
            message.Message.ShouldNotBeNull();
            message.Message.ShouldNotContain("test@test.com");
            message.Message.ShouldContain(DateTime.Now.Year.ToString());
        }
        [Fact]
        public void EmailShouldHaveError()
        {

            IBuilderEmail builderEmail = new BuilderEmail();
            HeaderDefaultTemplateBuilder header = new HeaderDefaultTemplateBuilder()
            {

            };
            
            FooterDefualtTemplateBuilder footer = new FooterDefualtTemplateBuilder()
            {
                Year = DateTime.Now.Year.ToString(),
            };

            Action action = () =>
            {
                EmailMessage message = builderEmail.GenerateMessage(header, null, footer);
            };
            action.ShouldThrow<Exception>("Invalid Strategy");
        }

        

    }
}
