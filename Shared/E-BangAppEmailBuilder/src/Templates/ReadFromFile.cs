using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.Enums;
using System.Reflection;


namespace E_BangAppEmailBuilder.src.Templates
{
    internal static class ReadFromFile
    {
        internal static string ReadTemplateFromFile(string fileName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"E_BangAppEmailBuilder.Files.{fileName}";
                
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                
                string rootPath = GetProjectDirectoryPath();
                string filePath = Path.Combine(rootPath, fileName);
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Template file '{fileName}' not found as embedded resource or file. Error: {ex.Message}", ex);
            }
        }

        internal static List<BodyEmailTemplateTypeOptions> ReadBodyTemplateFromFile()
        {
            var templateList = new List<BodyEmailTemplateTypeOptions>();
            
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceNames = assembly.GetManifestResourceNames()
                    .Where(name => name.Contains("Files") && name.Contains("body", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                foreach (var resourceName in resourceNames)
                {
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        using var reader = new StreamReader(stream);
                        var content = reader.ReadToEnd();
                       
                        var fileName = resourceName.Split('.').LastOrDefault() ?? string.Empty;
                        var templateTypeName = Enum.GetNames<EEnumEmailBodyBuilderType>()
                            .FirstOrDefault(name => fileName.Contains(name, StringComparison.OrdinalIgnoreCase));
                        
                        if (!string.IsNullOrEmpty(templateTypeName))
                        {
                            templateList.Add(new BodyEmailTemplateTypeOptions
                            {
                                TemplateTypeName = templateTypeName,
                                TemplateBody = content
                            });
                        }
                    }
                }
                
                if (templateList.Count == 0)
                {
                    throw new InvalidOperationException("No email body templates found in embedded resources");
                }
                
                return templateList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading email template files from embedded resources: {ex.Message}", ex);
            }
        }
        private static string GetProjectDirectoryPath()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? string.Empty;
            return Path.Combine(assemblyDirectory, "Files");
        }
    }
}
