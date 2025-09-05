using E_BangAppEmailBuilder.src.Builder;
using E_BangAppEmailBuilder.src.Options;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;
using E_BangAppRabbitSharedClass.Enums;
using System.Text.Json;
namespace E_BangAppEmailBuilder.src.Abstraction
{
    public interface IBuilderEmail
    {
        /// <summary>
        ///   Generate email Message.
        ///     <para>Uses <see cref="RegistrationBodyBuilder"/> to create a Registration body</para>
        ///     <para>Uses <see cref="ConfirmEmailTokenBodyBuilder"/> to create a Confirmation Email body.</para>
        /// </summary>
        /// <param name="body"></param>
        /// <param name="footer"></param>
        /// <returns>Returns an instance of <see cref="EmailMessage"/></returns>
        EmailMessage GenerateMessage(HeaderDefaultTemplateBuilder header, (EEnumEmailBodyBuilderType type, JsonElement element) body, FooterDefualtTemplateBuilder footer);

    }
}
