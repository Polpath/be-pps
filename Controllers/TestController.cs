using be.Model;
using Microsoft.AspNetCore.Mvc;

namespace be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet(Name = "Test")]
        public bool Get()
        {
            return true;
        }
    }
}
