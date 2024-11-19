using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models
{
    public class Device
    {
        [Key]public Guid DeviceId { get; set; }
        public string Name { get; set; } = string.Empty;    
        public DateTime RegisteredAt { get; set; }  
        public string Status { get; set; } = string.Empty;
    }
}
