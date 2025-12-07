using System.ComponentModel.DataAnnotations;

namespace E_BangEmailWorker.Database
{
    public class EmailRender
    {
        [Key]
        public int EmailRenderId { get; set; }
        public int EmailTypeId { get; set; }
        public int EmailRenderStrategyId { get; set; }
        public int HeaderParametersId { get; set; }
        public int BodyParametersId { get; set; }
        public int FooterParametersId { get; set; }
        public int HeaderComponenetsId { get; set; }
        public int BodyComponenetsId { get; set; }
        public int FooterComponenetsId { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        public EmailType? EmailType { get; set; }
        public RenderStrategy? RenderStrategy { get; set; }
        public AssemblyParametersEntity? HeaderParameters { get; set; }
        public AssemblyParametersEntity? BodyParameters { get; set; }
        public AssemblyParametersEntity? FooterParameters { get; set; }
        public AssemblyComponentEntity? HeaderComponenets { get; set; }
        public AssemblyComponentEntity? BodyComponenets { get; set; }
        public AssemblyComponentEntity? FooterComponenets { get; set; }
    }
}
