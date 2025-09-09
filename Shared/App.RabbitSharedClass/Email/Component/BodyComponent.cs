using App.EmailHelper.Shared.Enums;

namespace App.RabbitSharedClass.Email.Component
{
    public class BodyComponent<T> where T : class
    {
        public BodyComponent(EEmailBodyType headerType, T bodyParameters)
        {
            HeaderType = headerType;
            BodyParameters = bodyParameters;
        }

        public EEmailBodyType HeaderType { get; set; }

        public T BodyParameters { get; set; }
    }
}
