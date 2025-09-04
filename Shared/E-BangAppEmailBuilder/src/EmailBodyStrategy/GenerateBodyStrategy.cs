using E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase;
using E_BangAppRabbitSharedClass.Enums;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy
{
    public class GenerateBodyStrategy : IGenerateBodyStrategy
    {
        private static GenerateBodyStrategy _instance = null;
        private static object _instanceLock = new object();
        private GenerateBodyStrategy() { }

        public static GenerateBodyStrategy GetInstance()
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
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
        private readonly Dictionary<EEnumEmailBodyBuilderType,IGenerateBodyBase> strategyDictionary = new()
        {
            {EEnumEmailBodyBuilderType.Registration, new GenerateRegistrationBody()},
            {EEnumEmailBodyBuilderType.ConfirmEmail,new GenerateConfirmEmailBody()},
            {EEnumEmailBodyBuilderType.TwoWayToken, new GenerateTwoWayTokenBody()},
        };
        //private readonly Dictionary<EEnumEmailBodyBuilderType, Func<JsonElement, IGenerateBodyBase>> strategyDictionary = new()
        //{
        //    {EEnumEmailBodyBuilderType.Registration, json => JsonSerializer.Deserialize<GenerateRegistrationBody>(json) ?? throw new InvalidDataException("Invalid Email Builder Data Strategy")},
        //    {EEnumEmailBodyBuilderType.ConfirmEmail,json => JsonSerializer.Deserialize<GenerateConfirmEmailBody>(json)?? throw new InvalidDataException("Invalid Email Builder Data Strategy")},
        //    {EEnumEmailBodyBuilderType.TwoWayToken, json => JsonSerializer.Deserialize<GenerateTwoWayTokenBody>(json)?? throw new InvalidDataException("Invalid Email Builder Data Strategy")},
        //};
        public IGenerateBodyBase SwitchStrategy(EEnumEmailBodyBuilderType emailType)
        {
            if (strategyDictionary.TryGetValue(emailType, out var strategy))
            {
                return strategy;
            }
            throw new InvalidOperationException("Invalid Email Body Type");
        }


    }
}
