using App.EmailHelper.Shared.Enums;
namespace App.RabbitSharedClass.Email.Component
{
    public class FooterComponent<T> where T : class
    {
        public FooterComponent(EEmailFooterType footerType, T footerParameters)
        {
            FooterType = footerType;
            FooterParameters = footerParameters;
        }

        public EEmailFooterType FooterType { get; set; }

        public T FooterParameters { get; set; }

    }
}
