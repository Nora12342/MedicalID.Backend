using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MedicalID.Backend.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("/error")]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            return Problem(
                detail: exception?.Message,
                statusCode: 500,
                title: "An unexpected error occurred"
            );
        }
    }
}
