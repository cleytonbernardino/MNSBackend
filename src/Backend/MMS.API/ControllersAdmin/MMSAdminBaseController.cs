using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MMS.API.ControllersAdmin;

[Route("admin/[Controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class MmsAdminBaseController : ControllerBase
{
}
