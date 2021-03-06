using System;
using System.Collections.Generic;
using System.Security.Claims;
using Keepr.Models;
using Keepr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class KeepsController : ControllerBase
  {
    private readonly KeepsService _ks;
    public KeepsController(KeepsService ks)
    {
      _ks = ks;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Keep>> Get()
    {
      try
      {
        return Ok(_ks.Get());
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      };
    }
    [HttpGet("mine")]
    [Authorize]
    public ActionResult<Keep> GetMine()
    {
      try
      {

        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_ks.GetMine(userId));


      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpGet("{id}")]
    public ActionResult<Keep> Get(int id)
    {
      try
      {
        if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
        {
          string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
          return Ok(_ks.Get(id, userId));
        }
        else
        {
          return Ok(_ks.Get(id));
        }
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost("{id}/view")]
    public ActionResult View(int id)
    {
      try
      {
        _ks.View(id);
        return Ok();
      }
      catch (Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpPost("{id}/share")]
    public ActionResult Share(int id)
    {
      try
      {
        _ks.Share(id);
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpPost]
    [Authorize]
    public ActionResult<Keep> Post([FromBody] Keep newKeep)
    {
      try
      {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        newKeep.UserId = userId;
        return Ok(_ks.Create(newKeep));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpPut("{id}")]
    [Authorize]
    public ActionResult<Keep> Edit([FromBody]Keep update, int id)
    {
      try
      {
        update.Id = id;
        return _ks.Edit(update, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult<string> Delete(int id)
    {
      try
      {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        _ks.Delete(id, userId);
        return Ok("deleted");

      }
      catch (PermissionException e)
      {
        return Unauthorized(e.Message);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

  }
}