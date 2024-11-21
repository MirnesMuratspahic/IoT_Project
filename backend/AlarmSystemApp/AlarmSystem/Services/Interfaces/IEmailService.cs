namespace AlarmSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string message, string email);
    }
}
