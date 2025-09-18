using App.EmailHelper.Shared.Enums;

namespace App.EmailHelper.Shared.Email.Component
{
    public class HeaderComponent<T> where T : class
    {
        public HeaderComponent(EEmailHeaderType headerType, T headerParameters)
        {
            HeaderType = headerType;
            HeaderParameters = headerParameters;
        }

        public EEmailHeaderType HeaderType { get; set; }
        public T HeaderParameters { get; set; }
    }
}
