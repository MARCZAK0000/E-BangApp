namespace E_BangAppEmailBuilder.src.Templates
{
    public interface IReadTemplates
    {
        string GetDefaultHeaderTemplate();
        string GetDefaultFooterTemplate();
        string GetDefaultBodyTemplate(string parameter);
        string GetFullDefaultTemplate();
    }
}
