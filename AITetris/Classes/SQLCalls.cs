using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class SQLCalls
    {
        private static readonly string connectionString = "Data Source=mssql15.unoeuro.com;Initial Catalog=barrelclickers_dk_db_fightinggame;Persist Security Info=True;User ID=barrelclickers_dk;Password=ErAaR9pmFwG5";

        public static List<Game> GetLeaderboardTop10()
        {
            List<Game> games = new List<Game>();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard ORDER BY Points DESC";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }

                reader.Close();
            }
            return games;
        }

        public static void CreateLeaderboardEntry(Game gameToCreate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Leaderboard (Name, Points, LinesCleared, GameTimeInMs, IsPlayer) VALUES (@Name, @Points, @LinesCleared, @GameTimeInMs, @IsPlayer)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", gameToCreate.character.name);
                cmd.Parameters.AddWithValue("@Points", gameToCreate.points);
                cmd.Parameters.AddWithValue("@LinesCleared", gameToCreate.linesCleared);
                cmd.Parameters.AddWithValue("@GameTimeInMs", gameToCreate.time);
                cmd.Parameters.AddWithValue("@IsPlayer", gameToCreate.isPlayer);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
