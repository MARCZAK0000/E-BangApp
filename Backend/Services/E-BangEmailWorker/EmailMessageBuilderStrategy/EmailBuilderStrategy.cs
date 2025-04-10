using E_BangEmailWorker.EmailMessageBuilderFactory;
using E_BangEmailWorker.EmailMessageBuilderStrategy.BuilderOptions;
using E_BangAppEmailBuilder.src.BuildersDto.Body;
namespace E_BangEmailWorker.EmailMessageBuilderStrategy
{
    public class EmailBuilderStrategy : IEmailBuilderStrategy
    {
        Func<object, IBuilderEmailBase> _builderEmailStrategy;
        public EmailBuilderStrategy(Func<object, IBuilderEmailBase> builderEmailStrategy)
        {
            _builderEmailStrategy = builderEmailStrategy;
        }
        /// <summary>
        ///     Used in Strategy pattern to decied email type based on parameters type
        ///     <para>Uses <see cref="RegistrationBodyBuilder"/> to create a Registration body</para>
        ///     <para>Uses <see cref="ConfirmEmailTokenBodyBuilder"/> to create a Confirmation Email body.</para>
        /// </summary>
        /// <param name="parameters">object</param>
        /// <returns>HTML Email Text</returns>
        public string EmailBuilderRoundRobin(object parameters)
        {
            var strategy = _builderEmailStrategy.Invoke(parameters);
            return strategy.GenerateMessage(parameters);
        }
    }
}
