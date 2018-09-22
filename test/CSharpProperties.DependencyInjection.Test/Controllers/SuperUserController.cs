using CSharpProperties.DependencyInjection.Test.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSharpProperties.DependencyInjection.Test.Controllers
{

    [ApiController]
    [Route("api/super-user")]
    public class SuperUserController : Controller
    {
        private readonly SuperUserViewModel _superUser;

        public SuperUserController(SuperUserViewModel superUser)
        {
            _superUser = superUser;
        }

        [HttpGet]
        public IActionResult GetSuperUser()
        {
            return Ok(_superUser);
        }
    }
}