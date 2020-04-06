using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class VaultKeepsController : ControllerBase
  {
    private readonly VaultKeepsService _vks;

    public VaultKeepsController(VaultKeepsService vks)
    {
      _vks = vks;
    }
  }
}