using CSharpProperties.DependencyInjection.Test.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSharpProperties.DependencyInjection.Test.Controllers
{
    [ApiController]
    [Route("api/several-properties-nullable")]
    public class SeveralPropertiesNullableController : Controller
    {
        private readonly SeveralPropertiesNullableViewModel _severalPropertiesNullable;

        public SeveralPropertiesNullableController(SeveralPropertiesNullableViewModel severalPropertiesNullable)
        {
            _severalPropertiesNullable = severalPropertiesNullable;
        }

        [HttpGet]
        public IActionResult GetSeveralProperties()
        {
            return Ok(_severalPropertiesNullable);
        }
    }
}