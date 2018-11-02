using CSharpProperties.DependencyInjection.Test.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSharpProperties.DependencyInjection.Test.Controllers
{
    [ApiController]
    [Route("api/using-fields")]
    public class UsingFieldsController : Controller
    {
        private UsingFieldsViewModel _usingFieldsViewModel;

        public UsingFieldsController(UsingFieldsViewModel usingFieldsViewModel)
        {
            _usingFieldsViewModel = usingFieldsViewModel;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_usingFieldsViewModel);
        }
    }
}