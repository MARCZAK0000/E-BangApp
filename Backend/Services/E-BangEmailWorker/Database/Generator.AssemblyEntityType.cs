using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangEmailWorker.Database
{
    public class AssemblyEntityType
    {
        public int AssemblyEntityTypeId { get; set; }
        public required string AssemblyEntityTypeName { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        public List<AssemblyParametersEntity>? AssemblyParameterEntities { get; set; }
        public List<AssemblyComponentEntity>? AssemblyComponentEntities { get; set; }
    }
}
