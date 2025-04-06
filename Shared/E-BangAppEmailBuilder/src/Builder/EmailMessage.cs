using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace E_BangAppEmailBuilder.src.Builder
{
    public class EmailMessage
    {
        public string Header { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public byte[]? Attachments { get; set; }

        public string SerializeMessage()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
