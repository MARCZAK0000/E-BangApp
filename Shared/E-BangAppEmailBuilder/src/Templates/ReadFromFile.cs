using E_BangAppEmailBuilder.src.BuildersDto.Body;
using E_BangAppEmailBuilder.src.Enums;

namespace E_BangAppEmailBuilder.src.Templates
{
    internal static class ReadFromFile
    {
        internal static string ReadTemplateFromFile(string fileName)
        {
            string rootPath = GetProjectDirectoryPath();
            string filePath = Path.Combine(rootPath, fileName);
            if (File.Exists(filePath))
            {
                return string.Join(Environment.NewLine, File.ReadLines(filePath));
            }
            else
            {
                return string.Empty;
            }
        }
        internal static List<BodyEmailTemplateTypeOptions> ReadBodyTemplateFromFile()
        {
            var templateList = new List<BodyEmailTemplateTypeOptions>();
            var folderPath = GetProjectDirectoryPath();

            if (!Directory.Exists(folderPath))
            {
                throw new Exception("Invalid Directory");
            }
            var files = Directory.GetFiles(folderPath).Where(pr => pr.Contains("body", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var file in files)
            {
                string filePath = Path.Combine(folderPath, file);
                if (File.Exists(filePath))
                {
                    string fileName = file.Remove(startIndex: file.IndexOf("Body"))
                        .Replace(folderPath+"\\", string.Empty);
                    string templateTypeName = Enum.GetNames<EEnumEmailBodyBuilderType>()
                        .Where(pr => pr.ToLower()
                            .Contains(fileName, StringComparison.CurrentCultureIgnoreCase))
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
        private static string GetProjectDirectoryPath()
        {
            string currentAsemblyDirectory = Directory.GetCurrentDirectory();
            string rootFolder = Directory.GetParent(currentAsemblyDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            return Path.Combine(rootFolder, "E-BangAppEmailBuilder", "Files");

        }
    }
}
