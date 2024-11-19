using System.ComponentModel.DataAnnotations.Schema;

namespace AlarmSystem.Models
{
    [NotMapped]
    public class ErrorProvider
    {
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
    }
}
