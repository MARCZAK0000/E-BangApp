using E_BangAppEmailBuilder.src.Builder;
namespace E_BangAppEmailBuilder.src.Abstraction
{
    public interface IBuilderEmail
    {
        EmailMessage GenerateMessage(object header, object body, object footer);
        
    }
}
