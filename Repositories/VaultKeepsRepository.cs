using System;
using System.Data;
using Keepr.Models;
using Dapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Repositories
{
  public class VaultKeepsRepository
  {
    private readonly IDbConnection _db;

    public VaultKeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal VaultKeep Create(VaultKeep newRelation)
    {
      string sql = @"INSERT INTO vaultkeeps 
      (vaultId, KeepId, userId)
       VALUES
        (@VaultId, @KeepId, @userId); 
        SELECT LAST_INSERT_ID();";
      newRelation.Id = _db.ExecuteScalar<int>(sql, newRelation);
      return newRelation;
    }

    internal IEnumerable<VaultKeepViewModel> GetKeeps(int vaultId, string userId)
    {
      string sql = @"
      SELECT 
      k.*,
      vk.id as vaultKeepId
      FROM vaultkeeps vk
      INNER JOIN (
        
      SELECT k.*,COUNT(vaultkeeps.id) as keeps FROM keeps k 
      LEFT JOIN vaultkeeps 
       ON vaultkeeps.keepId = k.id 
       AND  k.isPrivate = 0
       GROUP BY k.id

      ) k ON k.id = vk.keepId 
      WHERE (vaultId = @vaultId AND vk.userId = @userId) ";
      return _db.Query<VaultKeepViewModel>(sql, new { vaultId, userId });
    }

    internal ActionResult<bool> Delete(int id, string userId)
    {
      string sql = @"
      DELETE FROM vaultkeeps WHERE id = @id AND userId = @userId LIMIT 1;";
      return _db.Execute(sql, new { id, userId }) == 1;
    }
  }
}