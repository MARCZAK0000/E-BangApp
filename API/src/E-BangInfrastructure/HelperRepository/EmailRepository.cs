using E_BangAppRabbitSharedClass.BuildersDto.Header;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangDomain.ModelDtos.MessageSender;
using E_BangDomain.HelperRepository;

namespace E_BangInfrastructure.HelperRepository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IRabbitSenderRepository _rabbitSenderRepository;

        public EmailRepository(IRabbitSenderRepository rabbitSenderRepository)
        {
            _rabbitSenderRepository = rabbitSenderRepository;
        }

        public Task SendEmailConfirmAccountAsync(string token, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SendRegistrationConfirmAccountEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            var buildEmail = new EmailServiceRabbitMessageModel()
            {
                AddressTo = email,
                Subject = "Account Registration",
                Body = new()
                {
                    Header = new HeaderDefaultTemplateBuilder()
                    {
                        Email = email,
                    },
                    Body = new RegistrationBodyBuilder()
                    {
                        Email = email,
                        Token = token
                    },
                    Footer = new()
                    {
                        Year = DateTime.Now.Year.ToString()
                    }
                }
            };
            var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            {
                Message = buildEmail,
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }
        public async Task SendTwoWayTokenEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            var buildEmail = new EmailServiceRabbitMessageModel()
            {
                AddressTo = email,
                Subject = "Two-Way Authentication Token",
                Body = new()
                {
                    Header = new HeaderDefaultTemplateBuilder()
                    {
                        Email = email,
                    },
                    Body = new TwoWayTokenBodyBuilder()
                    {
                        Email = email,
                        Token = token
                    },
                    Footer = new()
                    {
                        Year = DateTime.Now.Year.ToString()
                    }
                }
            };
            var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            {
                Message = buildEmail,
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }
    }
}
