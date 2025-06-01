using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MedicalID.Backend.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [HttpGet] // Required by Swagger to know what method this is
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            // Optional: return the error message for development
            return Problem(
                detail: exception?.Message,
                title: "An unexpected error occurred."
            );
        }
    }
}
