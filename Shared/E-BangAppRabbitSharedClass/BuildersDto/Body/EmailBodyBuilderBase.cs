namespace E_BangAppRabbitSharedClass.BuildersDto.Body
{
    public class EmailBodyBuilderBase
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public virtual string TemplateName { get; set; }
    }
}
