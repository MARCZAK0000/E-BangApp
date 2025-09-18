using App.EmailHelper.Shared.Email.Component;
using App.EmailHelper.Shared.Enums;

namespace App.EmailHelper.Shared.Email
{
    public class EmailComponentBuilder<THeader, TBody, TFooter>
        where THeader : class
        where TBody : class
        where TFooter : class
    {
        private HeaderComponent<THeader> _header;
        private BodyComponent<TBody> _body;
        private FooterComponent<TFooter> _footer;
        private string _subject;
        private string _addressTo;

        public EmailComponentBuilder<THeader, TBody, TFooter> WithHeader(EEmailHeaderType eEmailHeaderType, THeader header)
        {
            _header = new HeaderComponent<THeader>(eEmailHeaderType, header);
            return this;
        }
        public EmailComponentBuilder<THeader, TBody, TFooter> WithBody(EEmailBodyType eEmailBodyType, TBody body)
        {
            _body = new BodyComponent<TBody>(eEmailBodyType, body);
            return this;
        }
        public EmailComponentBuilder<THeader, TBody, TFooter> WithFooter(EEmailFooterType eEmailFooterType, TFooter footer)
        {
            _footer = new FooterComponent<TFooter>(eEmailFooterType, footer);
            return this;
        }
        public EmailComponentBuilder<THeader, TBody, TFooter> WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }
        public EmailComponentBuilder<THeader, TBody, TFooter> WithAddressTo(string addressTo)
        {
            _addressTo = addressTo;
            return this;
        }
        public EmailComponent<THeader, TBody, TFooter> Build()
        {
            if (_header == null)
                throw new InvalidOperationException("Header component is not set.");
            if (_body == null)
                throw new InvalidOperationException("Body component is not set.");
            if (_footer == null)
                throw new InvalidOperationException("Footer component is not set.");
            if (string.IsNullOrWhiteSpace(_subject))
                throw new InvalidOperationException("Subject is not set.");
            if (string.IsNullOrWhiteSpace(_addressTo))
                throw new InvalidOperationException("AddressTo is not set.");
            return new EmailComponent<THeader, TBody, TFooter>(_header, _body, _footer, _subject, _addressTo);
        }


    }
}
