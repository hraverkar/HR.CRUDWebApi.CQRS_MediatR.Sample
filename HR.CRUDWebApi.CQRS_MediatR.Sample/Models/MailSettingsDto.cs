namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Models
{
    public record MailSettingsDto
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string DisPlayName { get; set; } = string.Empty;
    }
}
