using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.Enums;

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
        /* 
         * DISCLAIMER!!!!!
         * WHEN YOU ADD NEW FILE 
         * FILE NAME SHOULD HAVE NAME THE SAME LIKE IN EEnumEmailBodyBuilderType
         * DEFAULT TEMPLATE FOR FILE IS 'Name of Template'+BodyDefault
         * DEFUALT TEMPLATE FOR ENUM IS 'Name of Template' 
         * NAME OF ENUM MUST BE THE SAME LIKE NAME OF FILE(TEMPLATE)
         * DISCLAIMER!!!!!
         */

        private readonly Dictionary<Type, IGenerateBodyBase> strategyDictionary = new()
        {
            {typeof(RegistrationBodyBuilder), new GenerateRegistrationBody()},
            {typeof(ConfirmEmailTokenBodyBuilder), new GenerateConfirmEmailBody()},
            {typeof(TwoWayTokenBodyBuilder), new GenerateTwoWayTokenBody()},
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
