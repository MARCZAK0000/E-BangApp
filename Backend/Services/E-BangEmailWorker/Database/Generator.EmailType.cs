using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangEmailWorker.Database
{
    public class EmailType
    {
        public int EmailTypeId { get; set; }
        public required string EmailTypeName { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        public List<EmailRender>? EmailRenders { get; set; }    
    }
}
