using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangAppEmailBuilder.src.Enums;
using System.Reflection;

namespace E_BangAppEmailBuilder.src.Templates
{
    internal static class ReadFromFile
    {
        internal static string ReadTemplateFromFile(string fileName)
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            if (string.IsNullOrEmpty(currentDirectory)) return string.Empty;
            string folderName = "Files";
            string filePath = Path.Combine(currentDirectory, folderName, fileName);
            if (File.Exists(filePath))
            {
                return string.Join(Environment.NewLine, File.ReadLines("filePath"));
            }
            else
            {
                return string.Empty;
            }
        }
        internal static List<BodyEmailTemplateTypeOptions> ReadBodyTemplateFromFile()
        {
            var templateList = new List<BodyEmailTemplateTypeOptions>();
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            if (string.IsNullOrEmpty(currentDirectory)) 
                throw new Exception("Invalid Directory");
            string folderName = "Files";
            string combinePath = Path.Combine(currentDirectory, folderName);
            if (!Directory.Exists(combinePath))
            {
                throw new Exception("Invalid Directory");
            }
            var files = Directory.GetFiles(combinePath).Where(pr=>pr.Contains("body", StringComparison.OrdinalIgnoreCase));
            foreach (var file in files)
            {
                string filePath = Path.Combine(combinePath, file);
                if (File.Exists(filePath))
                {
                    string templateTypeName = Enum.GetNames<EEnumEmailBodyBuilderType>()
                        .Where(pr => pr.ToLower()
                            .Contains(file.Remove(startIndex: file.IndexOf("Body")).ToLower(), StringComparison.CurrentCultureIgnoreCase))
                                .FirstOrDefault()
                                    ?? string.Empty;
                    if (string.IsNullOrEmpty(templateTypeName))
                    {
                        continue;
                    }
                    templateList.Add(new BodyEmailTemplateTypeOptions
                    {
                        TemplateTypeName = templateTypeName,
                        TemplateBody = string.Join(Environment.NewLine, File.ReadLines(file))
                    });
                }
            }
            return templateList;
        }
    }
}
