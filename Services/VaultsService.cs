using System;
using System.Collections.Generic;
using Keepr.Models;
using Keepr.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Services
{
  public class VaultsService
  {
    private readonly VaultsRepository _repo;

    public VaultsService(VaultsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Vault> Get(string userId)
    {
      return _repo.Get(userId);
    }

    internal Vault Create(Vault newVault)
    {
      return _repo.Create(newVault);
    }

    internal Vault Get(int id, string userId)
    {
      Vault found = _repo.Get(id, userId);
      if (found is null)
      {
        throw new ArgumentNullException(nameof(found), "vault can not be found or you do not own it");

      }
      return found;

    }

    internal string Delete(int id, string userId)
    {
      return _repo.Delete(id, userId) ? "deleted" : throw new ArgumentNullException(nameof(id), "you do not own that vault or it does not exist");
    }
  }
}
