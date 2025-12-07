using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangEmailWorker.Database
{
    public class AssemblyType
    {
        [Key]
        public int AssemblyTypeId { get; set; }
        public required string AssemblyName { get; set; }
        public required string AssemblyPath { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        public List<AssemblyParametersEntity>? AssemblyParametersEntities { get; set; }
        public List<AssemblyComponentEntity>? AssemblyComponentEntities { get; set; }
    }
}
