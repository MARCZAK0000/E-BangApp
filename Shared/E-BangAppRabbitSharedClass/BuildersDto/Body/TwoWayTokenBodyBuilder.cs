using E_BangAppRabbitSharedClass.Enums;

namespace E_BangAppRabbitSharedClass.BuildersDto.Body
{
    public class TwoWayTokenBodyBuilder : EmailBodyBuilderBase
    {
        public override string TemplateName { get 
            {
                return Enum.GetName(EEnumEmailBodyBuilderType.TwoWayToken)!;
            } 
        }
    }
}
