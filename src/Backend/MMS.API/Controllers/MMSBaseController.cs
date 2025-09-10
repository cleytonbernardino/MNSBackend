using Microsoft.AspNetCore.Mvc;
using MMS.API.Attributes;

namespace MMS.API.Controllers;

[Route("[Controller]")]
[ApiController]
[AuthenticatedUser]
public class MMSBaseController : ControllerBase
{
}
