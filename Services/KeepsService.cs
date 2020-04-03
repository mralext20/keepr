using System;
using System.Collections.Generic;
using System.Data;
using Keepr.Models;
using Keepr.Repositories;

namespace Keepr.Services
{
  public class KeepsService
  {
    private readonly KeepsRepository _repo;
    public KeepsService(KeepsRepository repo)
    {
      _repo = repo;
    }
    public IEnumerable<Keep> Get()
    {
      return _repo.Get();
    }
    public Keep Get(int id, string userId = null)
    {
      if (userId is null)
      {
        return _repo.Get(id);
      }
      Keep found = _repo.Get(id, false);
      if (found.IsPrivate && found.UserId == userId)
      {
        return found;
      }
      else if (found.IsPrivate)
      {
        throw new ArgumentNullException(nameof(id), $"could Not find post with the id {id}");
      }
      return found;
    }

    public Keep Create(Keep newKeep)
    {
      return _repo.Create(newKeep);
    }

    public bool Delete(int id, string userId)
    {
      Keep keep = _repo.Get(id, false);
      if (keep.UserId == userId)
      {
        return _repo.Delete(id);
      }
      else
      {
        if (keep.IsPrivate)
        {
          throw new ArgumentNullException(nameof(id), $"could Not find post with the id {id}");
        }
        throw new PermissionException("you do not own that keep");
      }
    }
  }
}