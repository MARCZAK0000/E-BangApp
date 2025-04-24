using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangEmailWorker.Database
{
    [Table("Email", Schema = "Recent")]
    public class Email
    {
        [Key]
        public int EmailID { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public bool IsSend { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SendTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }
    }
}