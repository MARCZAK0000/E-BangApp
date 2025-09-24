using App.RabbitSharedClass.Enum;
using App.RabbitSharedClass.UniModel;

namespace App.RabbitSharedClass.UniversalModel
{
    public class RabbitMessageModel<T> : RabbitMessageModelBase<T> where T : class
    {
        public ERabbitChannel Channel { get; set; }
    }
}
