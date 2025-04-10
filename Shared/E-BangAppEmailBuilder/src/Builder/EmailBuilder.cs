using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangAppEmailBuilder.src.BuildersDto.Header;
using E_BangAppEmailBuilder.src.Templates;

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
        /// <summary>
        ///     Generates a defualt header for the email based on the provided options.
        /// </summary>
        /// <remarks>
        ///     This method uses the <see cref="HeaderDefaultBuilderOptions"/> object to set the header with a default message.
        ///     It is designed to work as part of a chainable method call.
        /// </remarks>
        /// <param name="options">
        ///     An instance of <see cref="HeaderDefaultBuilderOptions"/> containing customization details for the header.
        /// </param>
        /// <returns>
        ///     Returns the current instance of <see cref="EmailBuilder"/> to facilitate method chaining.
        /// </returns>
        public EmailBuilder GenerateHeader(HeaderDefaultBuilderOptions options)
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
        public EmailBuilder GenerateHeader(HeaderCustomBuilderOptions options)
        {
            _header = options.CustomMessage;
            return this;
        }
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
        public EmailBuilder GenerateFooter()
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
        public EmailBuilder GenerateBody(object parameters)
        {
            if (parameters is EmailBodyBuilderBase registrationParameters)
            {
                string template = _readTemplates.GetDefaultBodyTemplate(registrationParameters.TemplateName);
                if (string.IsNullOrEmpty(template))
                {
                    return this;
                }
                _body = template.Replace("[email]", registrationParameters.Email).Replace("[token]", registrationParameters.Token);
            }
            return this;
        }

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
    }
}
