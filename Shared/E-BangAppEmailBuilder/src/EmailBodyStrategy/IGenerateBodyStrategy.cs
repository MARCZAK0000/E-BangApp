using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy
{
    internal interface IGenerateBodyStrategy
    {
        IGenerateBodyBase SwitchStrategy(object parameters);
    }
}
