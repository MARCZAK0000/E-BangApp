using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;
using E_BangAppRabbitSharedClass.BuildersDto.Body;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy
{
    public class GenerateBodyStrategy : IGenerateBodyStrategy
    {
        private static GenerateBodyStrategy _instance = null;
        private static object _instanceLock = new object(); 
        private GenerateBodyStrategy() { }

        public static GenerateBodyStrategy GetInstance()
        {
            if(_instance == null)
            {
                lock (_instanceLock)
                {
                    if(_instance == null)
                    {
                        _instance = new GenerateBodyStrategy();
                    }
                }
            }
            return _instance;
            
        }
        private readonly Dictionary<Type, IGenerateBodyBase> strategyDictionary = new()
        {
            {typeof(RegistrationBodyBuilder), new GenerateRegistrationBody()},
            {typeof(ConfirmEmailTokenBodyBuilder), new GenerateConfirmEmailBody()},
        };
        public IGenerateBodyBase SwitchStrategy(object parameters)
        {
            if(strategyDictionary.TryGetValue(parameters.GetType(), out var strategy))
            {
                return strategy;
            }
            throw new Exception("Invalid Strategy");
        }

        
    }
}
