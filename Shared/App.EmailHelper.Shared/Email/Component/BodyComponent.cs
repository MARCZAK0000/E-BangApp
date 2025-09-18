using App.EmailHelper.Shared.Enums;

namespace App.EmailHelper.Shared.Email.Component
{
    public class BodyComponent<T> where T : class
    {
        public BodyComponent(EEmailBodyType bodyType, T bodyParameters)
        {
            BodyType = bodyType;
            BodyParameters = bodyParameters;
        }

        public EEmailBodyType BodyType { get; set; }

        public T BodyParameters { get; set; }
    }
}
