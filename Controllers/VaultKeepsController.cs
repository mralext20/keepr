using System;
using System.Security.Claims;
using Keepr.Models;
using Keepr.Services;
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

    [HttpPost]
    public ActionResult<VaultKeep> Create([FromBody] VaultKeep newRelation)
    {
      try
      {
        return _vks.Create(newRelation, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpDelete("{id}")]
    public ActionResult<bool> Delete(int id)
    {
      try
      {
        return _vks.Delete(id, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}
