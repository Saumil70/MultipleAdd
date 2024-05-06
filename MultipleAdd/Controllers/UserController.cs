
using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MultipleAdd.Models;
using MultipleAdd.Repository.IRepository;
using MultipleAdd.ViewModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MultipleAdd.Controllers
{


    public class UserController : Controller
    {
        string Baseurl = "https://localhost:7053/api/";
        private readonly IUnitOfWork _unitOfWork;
        private static int formCounter = 1;


        public enum Status
        {
            Draft = 1,
            Submitted = 2,
            Approved = 3,
            Rejected =4,
        }


    public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
          
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            List<UserViewModel> data = new List<UserViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi");
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<UserViewModel>>(UserResponse);
                }

                return PartialView("_ShowUserPartial", data);
            }
        }


        public ActionResult AddRowPartial()
        {
            // Generate a unique form ID
            var formId = formCounter;
            formCounter++;

            // Create a model to hold the form ID
            var model = new FormViewModel
            {
                FormId = formId
            };

            // Pass the model to the partial view
            return PartialView("_AddRowPartial", model);
        }

        [HttpGet]
        public async Task<JsonResult> CountryList()
        {
            List<Countries> data = new List<Countries>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi/CountryList");
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<Countries>>(UserResponse);
                }

                return Json(data);
            }

        }
        [HttpGet]
        public async Task<JsonResult> StateList(int countryId)
        {
            List<State> data = new List<State>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi/StateList?countryId="+ countryId);
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<State>>(UserResponse);
                }

                return Json(data); // Return states as JSON

            }

        }
        [HttpGet]
        public async Task<JsonResult> CityList(int stateId)
        {
            List<City> data = new List<City>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi/CityList?stateId="+ stateId);
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<City>>(UserResponse);
                }

                return Json(data); 


            }
        }

        [HttpPost]
        public async Task<JsonResult> AddUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Status status = Status.Draft;
                    int statusValue = (int)status;

                    var obj = new User
                    {
                        Name = user.Name,
                        Count = 1,
                        CountryId = user.CountryId,
                        CityId = user.CityId,
                        StateId = user.StateId,
                        Status = statusValue
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(obj); // Serialize object to JSON
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("UserApi", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return new JsonResult(new { success = true });
                        }
                        else
                        {
                            // Log or handle unsuccessful response
                            return new JsonResult(new { success = false });
                        }
                    }
                }
                else
                {
                    // Log or handle invalid model state
                    return new JsonResult(new { success = false });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                return new JsonResult(new { success = false });
            }
        }


        [HttpPost]
        public async Task<JsonResult> SaveAll(List<User> jsonData)
        {
            if (ModelState.IsValid)
            {
                Status status = Status.Draft;
                int statusValue = (int)status;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    foreach (var user in jsonData)
                    {
                        var obj = new User
                        {
                            Name = user.Name,
                            Count = 1,
                            CountryId = user.CountryId,
                            CityId = user.CityId,
                            StateId = user.StateId,
                            Status = statusValue
                        };

                        var jsonContent = JsonConvert.SerializeObject(obj);
                        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("UserApi", httpContent);

                        if (!response.IsSuccessStatusCode)
                        {
                            return new JsonResult(new { success = false });
                        }
                    }
                }

                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false });
            }
        }


        [HttpPost]
        public async Task<JsonResult> StatusChange(StatusViewModel user)
        {
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage res = await client.GetAsync($"UserApi/{user.UserId}");
                    
                    if (res.IsSuccessStatusCode)
                    {
                        var UserResponse = await res.Content.ReadAsStringAsync();
                        var existingUser = JsonConvert.DeserializeObject<User>(UserResponse);

                        var patchDoc = new JsonPatchDocument<User>();
                        patchDoc.Replace(e => e.Status, user.Status);


                        if(user.Status == (int)Status.Draft)
                        {
                            patchDoc.Replace(e => e.Count, 2);
                        }
                        var json = JsonConvert.SerializeObject(patchDoc);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PatchAsync($"UserApi/{existingUser.UserId}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            return new JsonResult(new { success = true, user = user });
                        }   


                    }
                }

                    return new JsonResult(new {success =  true});   
            }
            else
            {
                return new JsonResult(new { success = false });
            }
        }


    }
}
