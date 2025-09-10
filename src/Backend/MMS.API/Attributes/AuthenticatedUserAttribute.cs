using Microsoft.AspNetCore.Mvc;
using MMS.API.Filter;

namespace MMS.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter))
{
}
