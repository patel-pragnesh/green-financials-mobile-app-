namespace SterlingSwitch.Services.Abstractions.Entities
{
    public class ChangePassword
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string UserID { get; set; }
        public string TPIN { get; set; }
    }
}
