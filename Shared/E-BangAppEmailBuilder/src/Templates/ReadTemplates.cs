using E_BangAppRabbitSharedClass.BuildersDto.Body;

namespace E_BangAppEmailBuilder.src.Templates
{
    public class ReadTemplates : IReadTemplates
    {
        private static string _defualtFooterTemplate = string.Empty;

        private static string _defualtHeaderTemplate = string.Empty;

        private static List<BodyEmailTemplateTypeOptions> _defaultBodyTemplate = Enumerable.Empty<BodyEmailTemplateTypeOptions>().ToList();

        private static string _defaultFullHtmlTemplate = string.Empty;

        private static object _lockObject = new();

        private static ReadTemplates _instance = null!;
        private ReadTemplates()
        {

        }
        public static ReadTemplates GetInstance()
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ReadTemplates();
                        _defaultBodyTemplate = ReadFromFile.ReadBodyTemplateFromFile();
                        _defualtFooterTemplate = ReadFromFile.ReadTemplateFromFile("DefaultFooter.txt");
                        _defualtHeaderTemplate = ReadFromFile.ReadTemplateFromFile("DefaultHeader.txt");
                        _defaultFullHtmlTemplate = ReadFromFile.ReadTemplateFromFile("DefaultFullHtml.txt");
                    }
                }
            }
            return _instance;
        }

        public string GetDefaultFooterTemplate()
        {
            return _defualtFooterTemplate;
        }

        public string GetDefaultHeaderTemplate()
        {
            return _defualtHeaderTemplate;
        }

        public string GetDefaultBodyTemplate(string templateName)
        {
            return _defaultBodyTemplate
                .Where(pr => pr.TemplateTypeName.Equals(templateName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault()?.TemplateBody ?? string.Empty;
        }

        public string GetFullDefaultTemplate()
        {
            return _defaultFullHtmlTemplate;
        }


    }
}
