using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlarmSystem.Models
{
    public class Device
    {
        [JsonIgnore]
        [Required]
        [Key]public Guid DeviceId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime RegisteredAt { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
