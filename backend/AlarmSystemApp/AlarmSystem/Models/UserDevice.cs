using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models
{
    public class UserDevice
    {
        [Required]
        [Key]public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Device Device { get; set; }
        [Required]
        public DateTime RegistrationDateTime { get; set; }
    }
}
