using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models
{
    public class UserDevice
    {
        [Key]public int Id { get; set; }
        public User User { get; set; } 
        public Device Device { get; set; }
        public DateTime RegistrationDateTime { get; set; }
    }
}
