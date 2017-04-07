using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StatTracker.Models;
using System.Data.SqlClient;

namespace StatTracker.Controllers
{

    public class PlayerController : ApiController
    {

        static private List<Player> TempDataBase = new List<Player>
        {
            new Player { Id = 1, FullName= "Billy Bob", PreferedJerseyNumber= 12, PreferedPosition = "Center", SkillLevel= 3},
            new Player { Id = 2, FullName= "Sally", PreferedJerseyNumber= 12, PreferedPosition = "right", SkillLevel= 1},
            new Player { Id = 3, FullName= "Susan", PreferedJerseyNumber= 12, PreferedPosition = "defense", SkillLevel= 3},
            new Player { Id = 4, FullName= "Jimmy", PreferedJerseyNumber= 12, PreferedPosition = "left", SkillLevel= 5},
        };

        const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=LegaueDatabase;Trusted_Connection=True;";
        // Open with using something

        [HttpGet]
        public IEnumerable<Player> GetAllPlayers()
        {
            var rv = new List<Player>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = "SELECT * FROM Players ORDER BY FullName";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rv.Add(new Player
                    {
                        FullName = reader["FullName"].ToString(),
                        Id = (int)reader["Id"],
                        PreferedJerseyNumber = reader["PreferedJerseyNumber"] as int?,
                        PreferedPosition = reader["PreferedPosition"].ToString(),
                        SkillLevel = reader["SkillLevel"] as int?
                    });
                }
                connection.Close();

            }
            return rv;

        }


        [HttpGet]
        public IHttpActionResult GetPlayer(int id)
        {
            var player = TempDataBase.FirstOrDefault(f => f.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(player);

                var nextFriday = DateTime.Now.AddDays(7);
            }
        }

        [HttpGet]
        public IEnumerable<Player> SearchPlayers(string position)
        {
            return TempDataBase.Where(w => w.PreferedPosition == position);
        }


        [HttpPut]
        public IHttpActionResult CreatePlayer([FromBody]Player player)
        {
            // Update a player 
            using (var connection = new SqlConnection(ConnectionString))
            {
                var query = @"INSERT INTO[dbo].[Players]  ([FullName],[PreferedJerseyNumber],[PreferedPosition],[SkillLevel])
                              OUTPUT INSERTED.Id                          
                              VALUES (@FullName, @PreferedJerseyNumber, @PreferedPosition, @SkillLevel)";
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@FullName", player.FullName);
                cmd.Parameters.AddWithValue("@PreferedJerseyNumber", player.PreferedJerseyNumber);
                cmd.Parameters.AddWithValue("@PreferedPosition", player.PreferedPosition);
                cmd.Parameters.AddWithValue("@SkillLevel", player.SkillLevel);
                var newId = cmd.ExecuteScalar();
                player.Id = (int)newId;
                connection.Close();
            }
            return Ok(player);
        }

        [HttpPost]
        public IHttpActionResult UpdatePlayer([FromUri]int id, [FromBody] Player player)
        {

            var oldPlayer = TempDataBase.FirstOrDefault(p => p.Id == id);
            if (oldPlayer == null)
            {
                return NotFound();
            }
            else
            {
                player.Id = oldPlayer.Id;
                TempDataBase.Remove(oldPlayer);
                TempDataBase.Add(player);
                return Ok(player);
            }
        }

        [HttpDelete]
        public IHttpActionResult DeletePlayer(int id)
        {
            var oldPlayer = TempDataBase.FirstOrDefault(p => p.Id == id);
            if (oldPlayer == null)
            {
                return NotFound();
            }
            else
            {
                TempDataBase.Remove(oldPlayer);
                return Ok();
            }
        }






    }
}
