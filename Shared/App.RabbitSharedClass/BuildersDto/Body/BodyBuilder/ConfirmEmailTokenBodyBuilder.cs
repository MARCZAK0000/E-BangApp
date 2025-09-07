using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBase;
using E_BangAppRabbitSharedClass.Enums;

namespace E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder
{
    public class ConfirmEmailTokenBodyBuilder : EmailBodyBuilderBase
    {
        public string Email { get; set; } = string.Empty;
        public string Token {  get; set; }  = string.Empty ;
        public override string? TemplateName
        {
            get
            {
                return Enum.GetName(EEnumEmailBodyBuilderType.ConfirmEmail);
            }
        }
    }
}
