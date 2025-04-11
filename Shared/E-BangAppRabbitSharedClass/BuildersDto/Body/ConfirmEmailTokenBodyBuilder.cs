using E_BangAppEmailBuilder.src.Enums;

namespace E_BangAppRabbitSharedClass.BuildersDto.Body
{
    public class ConfirmEmailTokenBodyBuilder : EmailBodyBuilderBase
    {
        public override string TemplateName
        {
            get
            {
                return Enum.GetName(EEnumEmailBodyBuilderType.ConfirmEmail)!;
            }
        }
    }
}
