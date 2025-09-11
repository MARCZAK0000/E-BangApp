using App.RabbitSharedClass.Email.Component;

namespace App.RabbitSharedClass.Email
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
    }
}
