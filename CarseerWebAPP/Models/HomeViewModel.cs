using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarseerWebAPP.Models
{
    public class HomeViewModel
    {
        public int SelectedMakeId { get; set; }
        public SelectList ListOfMakes { get; set; }
        public int SelectedManufacturingYearId { get; set; }
        public SelectList ListOfManufacturingYear { get; set; }
    }
}
