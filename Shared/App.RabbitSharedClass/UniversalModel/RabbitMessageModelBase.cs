namespace App.RabbitSharedClass.UniModel
{
    public class RabbitMessageModelBase<T> where T : class
    {
        public T Message { get; set; }
    }
}
