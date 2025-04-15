using E_BangAppRabbitSharedClass.Enums;

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
