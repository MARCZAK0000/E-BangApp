using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy
{
    public interface IGenerateBodyStrategy
    {
        IGenerateBodyBase SwitchStrategy(object parameters);
    }
}
