using E_BangAppEmailBuilder.src.Enums;

namespace E_BangAppRabbitSharedClass.BuildersDto.Body
{
    /// <summary>
    /// Used to pass Registration Parameters
    /// </summary>
    public class RegistrationBodyBuilder : EmailBodyBuilderBase
    {
        public override string TemplateName {
            get
            {
                return Enum.GetName(EEnumEmailBodyBuilderType.Registration)!;
            }
        }
    }
}
