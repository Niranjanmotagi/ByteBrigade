namespace HackathonBackend.Services
{
    public interface IEmailService
    {
        // Hackathon-friendly: writes a .eml-style file to wwwroot/emails so the
        // demo can show the "sent" email. Replace with real SMTP later.
        Task SendAsync(string to, string subject, string body);
    }
}
