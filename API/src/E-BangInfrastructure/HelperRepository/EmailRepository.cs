using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;
using E_BangAppRabbitSharedClass.BuildersDto.RabbitMessageChilds;
using E_BangAppRabbitSharedClass.Enums;
using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangDomain.HelperRepository;
using E_BangDomain.ModelDtos.MessageSender;
using System.Text.Json;

namespace E_BangInfrastructure.HelperRepository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IRabbitSenderRepository _rabbitSenderRepository;

        public EmailRepository(IRabbitSenderRepository rabbitSenderRepository)
        {
            _rabbitSenderRepository = rabbitSenderRepository;
        }

        public async Task SendEmailConfirmAccountAsync(string token, string email, CancellationToken cancellationToken)
        {
            var buildEmail = new EmailBody()
            {
                AdressedTo = email,
                Subject = "Confirm Email",
                Header = new HeaderDefaultTemplateBuilder
                {
                    Email = email,
                },
                Footer = new FooterDefualtTemplateBuilder()
                {
                    Year = DateTime.Now.Year.ToString(),
                },
                Body = JsonSerializer.SerializeToElement(new ConfirmEmailTokenBodyBuilder()
                {
                    Email = email,
                    Token = token
                })

            };
            var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            {
                Message = new EmailServiceRabbitMessageModel()
                {
                    EEnumEmailBodyBuilderType = EEnumEmailBodyBuilderType.ConfirmEmail,
                    Message = JsonSerializer.SerializeToElement(buildEmail)
                },
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }

        public async Task SendForgetPasswordTokenEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            var buildEmail = new EmailBody()
            {
                AdressedTo = email,
                Subject = "Forget Password Token",
                Header = new HeaderDefaultTemplateBuilder
                {
                    Email = email,
                },
                Footer = new FooterDefualtTemplateBuilder()
                {
                    Year = DateTime.Now.Year.ToString(),
                }
            };
            var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            {
                Message = new EmailServiceRabbitMessageModel()
                {
                    EEnumEmailBodyBuilderType = EEnumEmailBodyBuilderType.Registration,
                    Message = JsonSerializer.SerializeToElement(buildEmail)
                },
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }

        public async Task SendRegistrationConfirmAccountEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            var buildEmail = new EmailBody()
            {
                AdressedTo = email,
                Subject = "Registration Account",
                Header = new HeaderDefaultTemplateBuilder
                {
                    Email = email,
                },
                Footer = new FooterDefualtTemplateBuilder()
                {
                    Year = DateTime.Now.Year.ToString(),
                },
                Body = JsonSerializer.SerializeToElement(new RegistrationBodyBuilder()
                {
                    Email = email,
                    Token = token
                })

            };
            var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            {
                Message = new EmailServiceRabbitMessageModel()
                {
                    EEnumEmailBodyBuilderType = EEnumEmailBodyBuilderType.ConfirmEmail,
                    Message = JsonSerializer.SerializeToElement(buildEmail)
                },
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };

            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }
        public async Task SendTwoWayTokenEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            var buildEmail = new EmailBody()
            {
                AdressedTo = email,
                Subject = "Two Way Token",
                Header = new HeaderDefaultTemplateBuilder
                {
                    Email = email,
                },
                Footer = new FooterDefualtTemplateBuilder()
                {
                    Year = DateTime.Now.Year.ToString(),
                },
                Body = JsonSerializer.SerializeToElement(new TwoWayTokenBodyBuilder()
                {
                    Email = email,
                    Token = token
                })

            };
            var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            {
                Message = new EmailServiceRabbitMessageModel()
                {
                    EEnumEmailBodyBuilderType = EEnumEmailBodyBuilderType.ConfirmEmail,
                    Message = JsonSerializer.SerializeToElement(buildEmail)
                },
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }
    }
}
