using System.ComponentModel.DataAnnotations;

namespace E_BangEmailWorker.Database
{
    public class AssemblyParametersEntity
    {
        [Key]
        public int AssemblyParametersEntityId { get; set; }
        public int AssemblyEntityTypeId { get; set; }
        public int AssemblyTypeId { get; set; }
        public required string EntityParametersName { get; set; }
        public required string EntityParametersValue { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        public AssemblyType? AssemblyType { get; set; }
        public AssemblyEntityType? AssemblyEntityType { get; set; }
        
        public List<EmailRender>? HeaderParameters { get; set; }
        public List<EmailRender>? FooterParameters { get; set; }
        public List<EmailRender>? BodyParameters { get; set; }
    }
}
