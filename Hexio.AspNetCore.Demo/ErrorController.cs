using Hexio.AspNetCore.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hexio.AspNetCore.Demo
{
    [ApiController]
    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            throw new DomainException("You fucked it up didn't you?");

            return Ok();
        }
    }
}