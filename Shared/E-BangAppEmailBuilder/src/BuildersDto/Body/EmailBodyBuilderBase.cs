namespace E_BangAppEmailBuilder.src.BuildersDto.Body
{
    public class EmailBodyBuilderBase
    {
        public string Email { get; set; }
        public string Token { get; set; }
        internal virtual string TemplateName { get; set; }
    }
}
