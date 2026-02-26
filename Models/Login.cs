namespace TaskWebApi.Models
{
    public class Login
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
