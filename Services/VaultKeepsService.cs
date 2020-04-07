using System;
using System.Collections.Generic;
using Keepr.Models;
using Keepr.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _repo;
    private readonly VaultsRepository _vrepo;
    private readonly KeepsRepository _krepo;

    public VaultKeepsService(VaultKeepsRepository repo, KeepsRepository krepo, VaultsRepository vrepo)
    {
      _repo = repo;
      _krepo = krepo;
      _vrepo = vrepo;
    }
    /// <summary>
    /// creates a new relation. checks for private / public
    /// </summary>
    /// <param name="newRelation">a vaultKeep object representign the new assiciation</param>
    /// <param name="userId">the user trying to create the association</param>
    internal VaultKeep Create(VaultKeep newRelation, string userId)
    {
      Keep foundKeep = _krepo.Get(newRelation.KeepId, false);
      // two states:
      // found, is private, not created by person makign the request 
      // found, not private

      if (foundKeep.IsPrivate && foundKeep.UserId != userId)
      {
        throw new ArgumentNullException(nameof(newRelation.KeepId), "That Keep does not exist");
      }
      // found is public or we made it and it is private

      // _vrepo.Get does userId checking internally
      Vault foundVault = _vrepo.Get(newRelation.VaultId, userId);


      newRelation.UserId = userId;
      return _repo.Create(newRelation);

    }

    internal ActionResult<bool> Delete(int id, string userId)
    {
      return _repo.Delete(id, userId);
    }

    internal IEnumerable<VaultKeepViewModel> GetKeeps(int id, string userId)
    {
      return _repo.GetKeeps(id, userId);
    }
  }
}