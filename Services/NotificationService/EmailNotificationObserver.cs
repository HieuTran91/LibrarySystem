namespace LibraryProject.Services.NotificationService
{
    public class EmailNotificationObserver
    {
        private readonly string _email;
        public EmailNotificationObserver(string email)
        {
            _email = email;
        }

        public void Notify(string message)
        {
            Console.WriteLine($"Send message to {_email} : {message}");
        }
    }
}
