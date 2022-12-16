using System.ComponentModel.DataAnnotations;

namespace CarseerWebAPP.Models
{
    public class SetupViewModel
    {
        public long Id { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
