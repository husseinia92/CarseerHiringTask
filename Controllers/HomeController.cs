using CarseerWebAPP.Helpers;
using CarseerWebAPP.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();
            PrepareList(ref model);
            return View(model);
        }

        public JsonResult GetMakes()
        {
            int pageSize = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Query["pageSize"]);
            int skip = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Query["skip"]);
            string search = _httpContextAccessor.HttpContext.Request.Query["filter[filters][0][value]"];
            var countriesList = GetAllMakes();
            var total = countriesList.Count();
            var data = countriesList.Skip(skip).Take(pageSize).ToList();
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                var result = countriesList.Where(x => x.Make_Name.ToString().ToLower().Contains(search.ToLower())).ToList();
                total = result.Count;
                return Json(new { total = total, data = result });

            }
            return Json(new { total = total, data = data });
        }

        public JsonResult MakesValueMapper(int[] values)
        {
            var indices = new List<int>();
            var countryList = GetAllMakes();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var item in countryList)
                {
                    if (values.Contains(item.Make_ID))
                    {
                        indices.Add(index);
                    }
                    index += 1;
                }
            }
            return Json(indices);
        }

        [HttpPost]
        public JsonResult Search(HomeViewModel model)
        {
            #region Get Vehicle Types by Make Id 

            var vehicleTypes = GetAllVehicleTypesByMakeId(model.make_ID ?? 0);

            #endregion

            #region Get Models by Make Id & Manufacturing Year

            var models = GetAllModelsByMakeIdAndManufacturingYear(model.make_ID ?? 0,model.SelectedManufacturingYearId);

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

            /*
             * For Better performance, loading the makes and models will be by 
             * server filtering and paging using kendo virtualization methodology
             */

            //var AllMakes = GetAllMakes();
            //if (AllMakes != null && AllMakes.Count > 0)
            //{
            //    var setupForMakesSelectList = AllMakes
            //        .Select(s => new SetupViewModel()
            //        {
            //            Id = s.Make_ID,
            //            Description = s.Make_Name
            //        })
            //        .ToList();

            //    model.ListOfMakes = new SelectList(setupForMakesSelectList, "Id", "Description", model.SelectedMakeId);
            //}

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
