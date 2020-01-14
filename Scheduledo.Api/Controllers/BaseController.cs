using System.Linq;
using System.Security.Claims;
using Scheduledo.Core.Enums;
using Scheduledo.Core.Extensions;
using Scheduledo.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scheduledo.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BaseController : Controller
    {
        public CurrentUser CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var identity = User.Identity as ClaimsIdentity;

                    return new CurrentUser
                    {
                        Id = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                        Role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.GetEnum<UserRole>(),
                        Email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value
                    };
                }

                return null;
            }
        }

        protected IActionResult GetResult(Result result)
        {
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Success)
                return Ok();

            switch (result.Error)
            {
                case ErrorType.NotFound:
                    return NotFound();
                case ErrorType.NoContent:
                    return NoContent();
                case ErrorType.BadRequest:
                    return BadRequest(result.ErrorMessage);
                case ErrorType.Unauthorized:
                    return Unauthorized();
                case ErrorType.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);
                case ErrorType.InternalServerError:
                    return StatusCode(StatusCodes.Status500InternalServerError);
                default:
                    return BadRequest(result.Error.GetName());
            }
        }

        protected IActionResult GetResult<T>(Result<T> result)
        {
            if (result != null && result.Success)
                return Ok(result.Data);

            return GetResult((Result)result);
        }
    }
}