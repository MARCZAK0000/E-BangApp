using E_BangAppRabbitSharedClass.Enums;
using System.Text.Json;

namespace E_BangAppRabbitSharedClass.RabbitModel
{
    /// <summary>
    /// Represents a message model used for communication with the email service via RabbitMQ.
    /// </summary>
    /// <remarks>This model encapsulates the type of email body builder to use and the associated message
    /// payload. It is designed to facilitate structured communication between services.</remarks>
    public class EmailServiceRabbitMessageModel
    {
        public EEnumEmailBodyBuilderType EEnumEmailBodyBuilderType { get; set; }
        public JsonElement Message { get; set; }

        public override string ToString()
        {
            return $"EmailServiceRabbitMessageModel: Type={EEnumEmailBodyBuilderType}, Message={Message.GetRawText()}";
        }
    }
}
