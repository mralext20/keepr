using System;
using System.Collections.Generic;
using System.Data;
using Keepr.Models;
using Dapper;

namespace Keepr.Repositories
{
  public class KeepsRepository
  {
    private readonly IDbConnection _db;

    public KeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal IEnumerable<Keep> Get()
    {
      string sql = @"SELECT k.*,COUNT(vaultkeeps.id) as keeps FROM keeps k 
      LEFT JOIN vaultkeeps 
       ON vaultkeeps.keepId = k.id 
       AND  k.isPrivate = 0
       GROUP BY k.id
       ;";
      return _db.Query<Keep>(sql);
    }

    internal Keep Get(int id, bool FilterPrivate = true)
    {
      string sql;
      if (FilterPrivate)
      {
        sql = @"SELECT data.* FROM (
          SELECT k.*,COUNT(vaultkeeps.id) as keeps 
          FROM keeps k
          LEFT JOIN vaultkeeps
            ON vaultkeeps.keepId = k.id
            AND  k.isPrivate = 0
            GROUP BY k.id
        ) as data
        WHERE id = @id
        ;";
      }
      else
      {
        sql = @"SELECT data.* FROM (
          SELECT k.*,COUNT(vaultkeeps.id) as keeps 
          FROM keeps k
          LEFT JOIN vaultkeeps
            ON vaultkeeps.keepId = k.id
            GROUP BY k.id
        ) as data
        WHERE id = @id
        ;";
      }
      Keep found = _db.QueryFirstOrDefault<Keep>(sql, new { id });
      if (found is null)
      {
        throw new ArgumentNullException(nameof(id), $"could Not find post with the id {id}");
      }
      return found;
    }

    internal IEnumerable<Keep> GetMine(object userId)
    {
      string sql = @"SELECT data.* FROM (
          SELECT k.*,COUNT(vaultkeeps.id) as keeps 
          FROM keeps k
          LEFT JOIN vaultkeeps
            ON vaultkeeps.keepId = k.id
            GROUP BY k.id
        ) as data
        WHERE userId = @userId
        ;";
      return _db.Query<Keep>(sql, new { userId });
    }

    internal bool Delete(int id)
    {
      string sql = @"DELETE FROM keeps WHERE id = @id LIMIT 1;";
      return _db.Execute(sql, new { id }) == 1;
    }

    internal void Update(Keep update)
    {
      string sql = @"UPDATE keeps SET 
          name = @Name,
          description=@description,
          img = @Img
          WHERE
          id = @id
        ";

      _db.Execute(sql, update);
    }

    internal void View(int id)
    {
      string sql = @"UPDATE keeps SET views = views + 1 WHERE id = @id";
      _db.Execute(sql, new { id });
    }


    //CREATE TABLE keeps (
    //    id int NOT NULL AUTO_INCREMENT,
    //    name VARCHAR(255) NOT NULL,
    //    description VARCHAR(255) NOT NULL,
    //    userId VARCHAR(255),
    //    img VARCHAR(255),
    //    isPrivate TINYINT,
    //    views INT DEFAULT 0,
    //    shares INT DEFAULT 0,
    //    keeps INT DEFAULT 0,
    //    INDEX userId (userId),
    //    PRIMARY KEY (id)
    //);

    internal Keep Create(Keep KeepData)
    {
      string sql = @"INSERT INTO keeps 
      (name, description, userId, img, isPrivate ) 
      VALUES (@Name, @Description, @userId, @img, @isPrivate);
      SELECT LAST_INSERT_ID();";
      KeepData.Id = _db.ExecuteScalar<int>(sql, KeepData);
      return KeepData;
    }
  }
}