using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBase;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;
using System.Text.Json;

namespace E_BangAppRabbitSharedClass.BuildersDto.RabbitMessageChilds
{
    /// <summary>
    /// Represents the content of an email, including its subject, recipient, and structured components such as header,
    /// body, and footer.
    /// </summary>W
    /// <remarks>This class provides a structured representation of an email's content, allowing customization
    /// of the subject, recipient, and  templated sections such as the header and footer. The body is stored as a JSON
    /// element, enabling flexible content formatting.</remarks>
    public class EmailBody
    {
        public string Subject { get; set; } 
        public string AdressedTo { get; set; }

        public HeaderDefaultTemplateBuilder Header { get; set; }
        public JsonElement Body { get; set; }
        public FooterDefualtTemplateBuilder Footer { get; set; }

        public override string ToString()
        {
            return $"Subject: {Subject}, AdressedTo: {AdressedTo}, Header: [{Header?.Email}], Body: {Body.GetRawText()}, Footer: [{Footer?.Year}]";
        }
    }
}
