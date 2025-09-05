using E_BangAppEmailBuilder.src.Options;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    public interface IGenerateBodyBase
    {
        string GenerateBody(JsonElement parameters);
    }
}
