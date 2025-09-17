using Microsoft.AspNetCore.Mvc;
using MMS.API.Attributes;

namespace MMS.API.Controllers;

[Route("api/[Controller]")]
[ApiController]
[AuthenticatedUser]
public class MMSBaseController : ControllerBase
{
}
