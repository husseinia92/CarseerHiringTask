using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarseerWebAPP.Models
{
    public class HomeViewModel
    {
        [Required]
        public int? make_ID { get; set; }
        
        /// <summary>
        /// for better performance in loading makes dropdown, i performed virtualization methodology for kendo,
        /// to server filter and paginate makes.
        /// </summary>
        //public int SelectedMakeId { get; set; }
        //public SelectList ListOfMakes { get; set; }

        public int SelectedManufacturingYearId { get; set; }
        public SelectList ListOfManufacturingYear { get; set; }
    }
}
