using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models.DTO
{
    public class dtoUserInformations
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
