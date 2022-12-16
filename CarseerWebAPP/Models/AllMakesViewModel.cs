
using System.Collections.Generic;

namespace CarseerWebAPP.Models
{
    public class AllMakesViewModel : APIViewModelBase
    {
        public AllMakesViewModel()
        {
            Results = new List<AllMakesResultViewModel>(); 
        }
    
        public List<AllMakesResultViewModel> Results { get; set; }
    }
    public class AllVehicleTypesViewModel : APIViewModelBase
    {
        public AllVehicleTypesViewModel()
        {
            Results = new List<AllVehicleTypesResultViewModel>();
        }

        public List<AllVehicleTypesResultViewModel> Results { get; set; }
    }
    public class AllModelsViewModel : APIViewModelBase
    {
        public AllModelsViewModel()
        {
            Results = new List<AllModelsResultViewModel>();
        }

        public List<AllModelsResultViewModel> Results { get; set; }
    }

    public class AllMakesResultViewModel
    {
        public int Make_ID { get; set; }
        public string Make_Name { get; set; }
    }
    public class AllVehicleTypesResultViewModel
    {
        public int VehicleTypeId { get; set; }
        public string VehicleTypeName { get; set; }
    }
    public class AllModelsResultViewModel
    {
        public int Make_ID { get; set; }
        public string Make_Name { get; set; }
        public int Model_ID { get; set; }
        public string Model_Name { get; set; }
    }
}
