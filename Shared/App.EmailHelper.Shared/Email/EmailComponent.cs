using App.EmailHelper.Shared.Email.Component;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace App.EmailHelper.Shared.Email
{
    public class EmailComponent<THeader, TBody, TFooter>
        where THeader : class
        where TBody : class
        where TFooter : class
    {
        public HeaderComponent<THeader> Header { get; }
        public BodyComponent<TBody> Body { get;  }
        public FooterComponent<TFooter> Footer { get; }
        public string Subject { get; } 
        public string AddressTo { get; }


        public EmailComponent(HeaderComponent<THeader> header, BodyComponent<TBody> body, FooterComponent<TFooter> footer, 
            string subject, string addressTo )
        {
            Header = header;
            Body = body;
            Footer = footer;
            Subject = subject;
            AddressTo = addressTo;
        }

        public static EmailComponentBuilder<THeader, TBody, TFooter> Builder()
        {
            return new EmailComponentBuilder<THeader, TBody, TFooter>();
        }

        public EmailComponentJson ToEmailComponentJson()
        {
            return new EmailComponentJson
            {
                FooterType = Footer.FooterType,
                FooterParameters = JsonSerializer.SerializeToElement(Footer.FooterParameters),
                BodyType = Body.BodyType,
                BodyParameters = JsonSerializer.SerializeToElement(Body.BodyParameters),
                HeaderType = Header.HeaderType,
                HeaderParameters = JsonSerializer.SerializeToElement(Header.HeaderParameters),
                Subject = Subject,
                AddressTo = AddressTo
            };
        }
    }
}
