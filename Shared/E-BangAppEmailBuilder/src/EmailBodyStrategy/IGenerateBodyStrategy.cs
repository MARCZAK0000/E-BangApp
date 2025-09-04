using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;
using E_BangAppRabbitSharedClass.Enums;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy
{
    public interface IGenerateBodyStrategy
    {
        IGenerateBodyBase SwitchStrategy(EEnumEmailBodyBuilderType messageType);
    }
}
