namespace E_BangEmailWorker.EmailMessageBuilderStrategy.BuilderOptions
{
    public interface IBuilderEmailBase
    {
        public string GenerateMessage(object parameters);
    }
}
