using E_BangAppEmailBuilder.src.Enums;

namespace E_BangAppEmailBuilder.src.BuildersDto.Body
{
    public class ConfirmEmailTokenBodyBuilder : EmailBodyBuilderBase
    {
        internal override string TemplateName
        {
            get
            {
                return Enum.GetName<EEnumEmailBodyBuilderType>(EEnumEmailBodyBuilderType.ConfirmEmail)!;
            }
        }
    }
}
