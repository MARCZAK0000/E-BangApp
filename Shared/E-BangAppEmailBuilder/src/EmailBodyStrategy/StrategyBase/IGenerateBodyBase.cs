namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    public interface IGenerateBodyBase
    {
        string GenerateBody<T>(T parameters);
    }
}
