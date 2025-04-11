using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;
using E_BangAppRabbitSharedClass.BuildersDto.Body;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy
{
    internal class GenerateBodyStrategy : IGenerateBodyStrategy
    {
        private static GenerateBodyStrategy _instance = null!;


        private static object obj = new object();
        internal static GenerateBodyStrategy GetInstance()
        {
            if (_instance == null)
            {
                lock (obj)
                {
                    if(_instance == null)
                    {
                        _instance = new GenerateBodyStrategy();
                    }
                }
            }
            return _instance;
        }

        internal IGenerateBodyBase SwitchStrategy(object parameters)
        {
            switch(parameters)
            {
                case RegistrationBodyBuilder:
                    return new GenerateRegistrationBody();
                case ConfirmEmailTokenBodyBuilder:
                    return new GenerateConfirmEmailBody
            }
        }

        
    }
}
