using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models
{
    public class DeviceData
    {
        [Required]
        public Guid DeviceId { get; set; }
        public float Temperature { get; set; }
        public DateTime ReadingDateTime { get; set; }

    }
}
