using CSharpProperties.DependencyInjection.Test.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSharpProperties.DependencyInjection.Test.Controllers
{
    [ApiController]
    [Route("api/properties-key-attribute")]
    public class PropertiesKeyAttributeController : Controller
    {
        private readonly PropertiesKeyAttributeViewModel _propertiesKeyAttribute;

        public PropertiesKeyAttributeController(PropertiesKeyAttributeViewModel propertiesKeyAttribute)
        {
            _propertiesKeyAttribute = propertiesKeyAttribute;
        }

        [HttpGet]
        public IActionResult GetSeveralProperties()
        {
            return Ok(_propertiesKeyAttribute);
        }
    }
}