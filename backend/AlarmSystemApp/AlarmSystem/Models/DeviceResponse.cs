using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlarmSystem.Models
{
    public class DeviceResponse
    {
        [JsonIgnore]
        [Key] public int Id { get; set; } 
        [Required]
        public Guid DeviceId { get; set; }
        [Required]
        public float Temperature { get; set; }
        [Required]
        public int MotionDetected { get; set; }
        [Required]
        public DateTime ReadingDateTime { get; set; }

    }
}
