using System.Text;

namespace HackathonBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IWebHostEnvironment env, ILogger<EmailService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var folder = Path.Combine(_env.ContentRootPath, "wwwroot", "emails");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var safeTo = string.IsNullOrWhiteSpace(to) ? "no-email" : to.Replace("@", "_at_");
            var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{safeTo}.eml";
            var path = Path.Combine(folder, fileName);

            var sb = new StringBuilder();
            sb.AppendLine($"To: {to}");
            sb.AppendLine($"From: noreply@bytebrigade.pharmacy");
            sb.AppendLine($"Subject: {subject}");
            sb.AppendLine($"Date: {DateTime.UtcNow:R}");
            sb.AppendLine();
            sb.AppendLine(body);

            await File.WriteAllTextAsync(path, sb.ToString());

            _logger.LogInformation("Email queued -> {Path} | To: {To} | Subject: {Subject}",
                path, to, subject);
        }
    }
}
