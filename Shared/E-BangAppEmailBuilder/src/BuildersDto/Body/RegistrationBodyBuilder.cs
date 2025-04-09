using E_BangAppEmailBuilder.src.Enums;

namespace E_BangAppEmailBuilder.src.BuildersDto.Body
{
    /// <summary>
    /// Used to pass Registration Parameters
    /// </summary>
    public class RegistrationBodyBuilder : EmailBodyBuilderBase
    {
        internal override string TemplateName {
            get
            {
                return Enum.GetName<EEnumEmailBodyBuilderType>(EEnumEmailBodyBuilderType.Registration)!;
            }
        }
    }
}
