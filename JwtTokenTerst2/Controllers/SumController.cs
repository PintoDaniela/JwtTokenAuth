using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenTerst2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SumController : ControllerBase
    {
        [HttpGet("sum")]
        public IActionResult GetSum(int a, int b)
        {
            int result = a + b;
            return Ok(result);
        }
    }
}
