using System.ComponentModel.DataAnnotations;

namespace E_BangEmailWorker.Database
{
    public class AssemblyComponentEntity
    {
        [Key]
        public int AssemblyComponentEntityId { get; set; }

        public int AssemblyEntityTypeId { get; set; }

        public int AssemblyTypeId { get; set; }

        public required string ComponentName { get; set; }

        public required string ComponentValue { get; set; }

        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        public AssemblyType? AssemblyType { get; set; }

        public AssemblyEntityType? AssemblyEntityType { get; set; }
        public List<EmailRender>? HeaderComponents { get; set; }
        public List<EmailRender>? FooterComponents { get; set; }
        public List<EmailRender>? BodyComponents { get; set; }
    }
}
