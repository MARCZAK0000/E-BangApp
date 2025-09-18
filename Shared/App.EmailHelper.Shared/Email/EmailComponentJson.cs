using App.EmailHelper.Shared.Enums;
using System.Text.Json;

namespace App.EmailHelper.Shared.Email
{
    public class EmailComponentJson
    {
        public EEmailFooterType FooterType { get; set; }
        public JsonElement FooterParameters { get; set; }
        public EEmailBodyType BodyType { get; set; }
        public JsonElement BodyParameters { get; set; }
        public EEmailHeaderType HeaderType { get; set; }
        public JsonElement HeaderParameters { get; set; }
        public string Subject { get; set; }
        public string AddressTo { get; set; }


        public JsonElement ToJsonElement()
        {
            return JsonSerializer.SerializeToElement(this);
        }
    }
}
