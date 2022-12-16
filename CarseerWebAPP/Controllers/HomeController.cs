using CarseerWebAPP.Helpers;
using CarseerWebAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarseerWebAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();
            PrepareList(ref model);
            return View(model);
        }

        [HttpPost]
        public JsonResult Search(HomeViewModel model)
        {
            #region Get Vehicle Types by Make Id 

            var vehicleTypes = GetAllVehicleTypesByMakeId(model.SelectedMakeId);

            #endregion

            #region Get Models by Make Id & Manufacturing Year

            var models = GetAllModelsByMakeIdAndManufacturingYear(model.SelectedMakeId,model.SelectedManufacturingYearId);

            #endregion

            return Json(new 
            { 
                success = true,
                vehicleTypes = vehicleTypes,
                vehicleModels = models
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Helpers

        private void PrepareList(ref HomeViewModel model)
        {
            #region Populate Makes List

            var AllMakes = GetAllMakes();
            if (AllMakes != null && AllMakes.Count > 0)
            {
                var setupForMakesSelectList = AllMakes
                    .Select(s => new SetupViewModel()
                    {
                        Id = s.Make_ID,
                        Description = s.Make_Name
                    })
                    .ToList();

                model.ListOfMakes = new SelectList(setupForMakesSelectList, "Id", "Description", model.SelectedMakeId);
            }
            #endregion

            #region Populate Manufacturing Years List

            var ListOfYearsFrom50sUntilNow = Enumerable.Range(DateTime.Now.Year - 72, 73);
            var setupForYearsSelectList = ListOfYearsFrom50sUntilNow.Select(s => new SetupViewModel() 
            {
                Id = s,
                Description = s.ToString()
            })
            .ToList();

            model.ListOfManufacturingYear = new SelectList(setupForYearsSelectList, "Id", "Description", model.SelectedManufacturingYearId);
            
            #endregion
        }

        private List<AllMakesResultViewModel> GetAllMakes() 
        {
            var ObjMakes = new AllMakesViewModel();
            var lstMakesResult = new List<AllMakesResultViewModel>();
            var baseUrl = _configuration.GetSection("BASEWEBAPIURL").Value;

            var MakesApiURl = baseUrl + "/getallmakes?format=json";
            var MakesApiResponse = Task.Run(() => APIManager.GetURI(new Uri(MakesApiURl)));
            MakesApiResponse.Wait();
            if (MakesApiResponse.IsCompleted && !string.IsNullOrEmpty(MakesApiResponse.Result))
            {
                ObjMakes = JsonConvert.DeserializeObject<AllMakesViewModel>(MakesApiResponse.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                
                if (ObjMakes != null)
                    lstMakesResult = ObjMakes.Results;
            }
            return lstMakesResult;
        }

        private List<AllVehicleTypesResultViewModel> GetAllVehicleTypesByMakeId(int Id)
        {
            var ObjVehicleTypes = new AllVehicleTypesViewModel();
            var lstVehicleTypesResult = new List<AllVehicleTypesResultViewModel>();
            var baseUrl = _configuration.GetSection("BASEWEBAPIURL").Value;

            var VehicleTypesApiURl = baseUrl + "/GetVehicleTypesForMakeId/" + Id + "?format=json";
            var VehicleTypesApiResponse = Task.Run(() => APIManager.GetURI(new Uri(VehicleTypesApiURl)));
            VehicleTypesApiResponse.Wait();
            if (VehicleTypesApiResponse.IsCompleted && !string.IsNullOrEmpty(VehicleTypesApiResponse.Result))
            {
                ObjVehicleTypes = JsonConvert.DeserializeObject<AllVehicleTypesViewModel>(VehicleTypesApiResponse.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                if (ObjVehicleTypes != null)
                    lstVehicleTypesResult = ObjVehicleTypes.Results;
            }
            return lstVehicleTypesResult;
        }

        private List<AllModelsResultViewModel> GetAllModelsByMakeIdAndManufacturingYear(int Id,int Year)
        {
            var ObjModels = new AllModelsViewModel();
            var lstModelsResult = new List<AllModelsResultViewModel>();
            var baseUrl = _configuration.GetSection("BASEWEBAPIURL").Value;

            var ModelsApiURl = baseUrl + "/GetModelsForMakeIdYear/makeId/" + Id + "/modelyear/" + Year + "?format=json";
            var ModelsApiResponse = Task.Run(() => APIManager.GetURI(new Uri(ModelsApiURl)));
            ModelsApiResponse.Wait();
            if (ModelsApiResponse.IsCompleted && !string.IsNullOrEmpty(ModelsApiResponse.Result))
            {
                ObjModels = JsonConvert.DeserializeObject<AllModelsViewModel>(ModelsApiResponse.Result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                if (ObjModels != null)
                    lstModelsResult = ObjModels.Results;
            }
            return lstModelsResult;
        }

        #endregion
    }
}
