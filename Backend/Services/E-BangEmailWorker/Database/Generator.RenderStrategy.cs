using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangEmailWorker.Database
{
    public class RenderStrategy
    {
        public int RenderStrategyId { get; set; }

        public required string AssemblyPath { get; set; }
        public required string AssemblyName { get; set; }
        public required string RenderStrategyName { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        public List<EmailRender>? EmailRenders { get; set; }    
    }
}
