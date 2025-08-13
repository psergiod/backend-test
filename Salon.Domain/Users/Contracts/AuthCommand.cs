namespace Salon.Domain.Users.Contracts
{
    public class AuthCommand
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
