using System.ComponentModel.DataAnnotations;

namespace AlarmSystem.Models.DTO
{
    public class dtoUserCode
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
