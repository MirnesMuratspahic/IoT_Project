using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlarmSystem.Models
{
    public class User
    {
        [JsonIgnore][Key] public int Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber {  get; set; } = string.Empty;
        [Required]
        public bool EmailConfirmed { get; set; } = false;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [JsonIgnore]
        [Required]
        public string Status { get; set; } = string.Empty;
        [JsonIgnore]
        [Required]
        public string Role { get; set; } = string.Empty;

    }
}
