using CSharpProperties.DependencyInjection.Test.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSharpProperties.DependencyInjection.Test.Controllers
{
    [ApiController]
    [Route("api/several-properties")]
    public class SeveralPropertiesController : Controller
    {
        private readonly SeveralPropertiesViewModel _severalProperties;

        public SeveralPropertiesController(SeveralPropertiesViewModel severalProperties)
        {
            _severalProperties = severalProperties;
        }

        [HttpGet]
        public IActionResult GetSeveralProperties()
        {
            return Ok(_severalProperties);
        }
    }
}