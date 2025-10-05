using App.RabbitSharedClass.Enum;
using App.RabbitSharedClass.UniModel;

namespace App.RabbitSharedClass.UniversalModel
{
    public class RabbitMessageModel : RabbitMessageModelBase
    {
        public ERabbitChannel Channel { get; set; }

    }
}
