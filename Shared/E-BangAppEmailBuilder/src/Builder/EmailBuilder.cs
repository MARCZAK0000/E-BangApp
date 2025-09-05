using E_BangAppEmailBuilder.src.EmailBodyStrategy;
using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;
using E_BangAppEmailBuilder.src.Options;
using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;
using E_BangAppRabbitSharedClass.Enums;
using System.Text.Json;
namespace E_BangAppEmailBuilder.src.Builder
{
    public class EmailBuilder
    {
        private string _header;
        private string _body;
        private string _footer;

        public EmailBuilder()
        {
            _header = string.Empty;
            _body = string.Empty;
            _footer = string.Empty;
        }


        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();

        private readonly IGenerateBodyStrategy _generateBodyStrategy = GenerateBodyStrategy.GetInstance();
        #region Header 


        /// <summary>
        ///     Generates a defualt header for the email based on the provided options.
        /// </summary>
        /// <remarks>
        ///     This method uses the <see cref="HeaderDefaultTemplateBuilder"/> object to set the header with a default message.
        ///     It is designed to work as part of a chainable method call.
        /// </remarks>
        /// <param name="options">
        ///     An instance of <see cref="HeaderDefaultTemplateBuilder"/> containing customization details for the header.
        /// </param>
        /// <returns>
        ///     Returns the current instance of <see cref="EmailBuilder"/> to facilitate method chaining.
        /// </returns>
        /// 
        public EmailBuilder GenerateHeader(HeaderDefaultTemplateBuilder options)
        {
            string defaultTemplate = _readTemplates.GetDefaultHeaderTemplate();
            if (defaultTemplate.Contains("[email]"))
            {
                _header = defaultTemplate.Replace("[email]", options.Email);
            }
            else
            {
                _header = defaultTemplate;
            }
            return this;
        }
        
        /// <summary>
        ///     Generates a customized header for the email based on the provided options.
        /// </summary>
        /// <remarks>
        ///     This method uses the <see cref="HeaderCustomBuilderOptions"/> object to set the header with a custom message.
        ///     It is designed to work as part of a chainable method call.
        /// </remarks>
        /// <param name="options">
        ///     An instance of <see cref="HeaderCustomBuilderOptions"/> containing customization details for the header.
        /// </param>
        /// <returns>
        ///     Returns the current instance of <see cref="EmailBuilder"/> to facilitate method chaining.
        /// </returns>
        private EmailBuilder GenerateHeader(HeaderCustomBuilderOptions options)
        {
            _header = options.CustomMessage;
            return this;
        }

        #endregion

        #region Footer
        /// <summary>
        ///     Generates the default footer for the email.
        /// </summary>
        /// <remarks>
        ///     This method retrieves the default footer template and applies it to the email.
        ///     It is designed to work seamlessly as part of a chainable method call.
        /// </remarks>
        /// <returns>
        ///     Returns the current instance of <see cref="EmailBuilder"/> to facilitate method chaining.
        /// </returns>
        public EmailBuilder GenerateFooter(FooterDefualtTemplateBuilder defaultFooter)
        {
            var template = _readTemplates.GetDefaultFooterTemplate();
            if (template.Contains("[year]", StringComparison.OrdinalIgnoreCase))
            {
                _footer = template.Replace("[year]", DateTime.Now.Year.ToString());
            }
            else
            {
                _footer = template;
            }
            return this;

        }

        #endregion

        #region Body
        /// <summary>
        ///     Generate the default body for each scenario.
        ///     <para>Uses <see cref="RegistrationBodyBuilder"/> to create a Registration body</para>
        ///     <para>Uses <see cref="ConfirmEmailTokenBodyBuilder"/> to create a Confirmation Email body.</para>
        /// <remarks>
        ///     This method retrieves the default body template and applies it to the email.
        ///     It is designed to work seamlessly as part of a chainable method call.
        /// </remarks>
        /// </summary>
        /// <returns>Returns an instance of <see cref="EmailBuilder"/> with the generated body.</returns>
        public EmailBuilder GenerateBody(EEnumEmailBodyBuilderType emailType, JsonElement elements)
        {
            IGenerateBodyBase strategy = _generateBodyStrategy.SwitchStrategy(emailType);
            _body = strategy.GenerateBody(elements);
            return this;
        }
        #endregion

        #region Build Message
        /// <summary>
        ///     Builds a complete email message by replacing placeholders in the full default template.
        /// </summary>
        /// <remarks>
        ///     This method combines the header, body, and footer into a single email message.
        /// </remarks>
        /// <returns>
        ///     Returns a new instance of <see cref="EmailMessage"/> containing the fully constructed message.
        /// </returns>

        public EmailMessage BuildMessage()
        {
            string template = _readTemplates.GetFullDefaultTemplate();
            if(string.IsNullOrEmpty(_header))
            {
                string toRemove = "[header]";
                template = template.Remove(template.IndexOf(toRemove), toRemove.Length);
            }
            if(string.IsNullOrEmpty(_footer))
            {
                string toRemove = "[footer]";
                template = template.Remove(template.IndexOf(toRemove), toRemove.Length);
            }
            if(string.IsNullOrEmpty(_body))
            {
                throw new Exception("Empty Body");
            }
            string message = template.Replace("[header]", _header)
                .Replace("[body]", _body)
                .Replace("[footer]", _footer);
            return new EmailMessage(message);
        }
        #endregion
    }
}
