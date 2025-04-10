using E_BangAppEmailBuilder.src.Builder;
using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangAppEmailBuilder.src.BuildersDto.Header;
using E_BangEmailWorker.EmailMessageBuilderStrategy.BuilderOptions;

namespace E_BangEmailWorker.EmailMessageBuilderFactory.BuilderOptions
{
    public class RegistrationEmailBuilder : IBuilderEmailBase
    {
        public string GenerateMessage(object parameters)
        {
            EmailBuilder emailBuilder = new();
            if (parameters == null || parameters is not RegistrationBodyBuilder registration)
            {
                throw new ArgumentNullException("Something went wrong");
            }
            EmailMessage email = emailBuilder
                .GenerateHeader(new HeaderDefaultBuilderOptions
                    {
                        Email = registration.Email,
                    })
                .GenerateBody(parameters)
                .GenerateFooter()
                .BuildMessage();
            return email.Message;
        }
    }
}
