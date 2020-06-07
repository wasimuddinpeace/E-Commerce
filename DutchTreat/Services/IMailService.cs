namespace DutchTreat.Services
{
    public interface IMailService
    {
        void sendMessage(string to, string subject, string body);
    }
}