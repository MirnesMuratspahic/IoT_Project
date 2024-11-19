namespace AlarmSystem.Models
{
    public class UserDevice
    {
        public User User { get; set; } 
        public Device Device { get; set; }
        public DateTime RegistrationDateTime { get; set; }
    }
}
