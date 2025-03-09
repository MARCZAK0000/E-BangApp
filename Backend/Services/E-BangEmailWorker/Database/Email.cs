using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using E_BangEmailWorker.Model;

namespace E_BangEmailWorker.Database
{
    [Table("Email", Schema = "Recent")]
    public class Email
    {
        [Key]
        public int EmailID { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string EmailBody { get; set; }
        [NotMapped]
        public EmailBody EmailBodyJson
        {
            get => JsonSerializer.Deserialize<EmailBody>(EmailBody)!;
            set => JsonSerializer.Serialize(value);
        }
        public bool IsSend { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }
    }
}