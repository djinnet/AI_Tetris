using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard ORDER BY Rank ASC";
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

        public static List<Game> SearchLeaderboardOnName(string name)
        {
            List<Game> foundGames = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Name LIKE @Name ORDER BY Rank ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name","%" + name + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    foundGames.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
            }
            return foundGames;
        }

        public static List<Game> GetNext10Leaderboard(int lastRank)
        {
            List<Game> games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank > @Rank ORDER BY Rank ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Rank", lastRank);

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

        public static List<Game> GetNext10Leaderboard(int lastRank, string name)
        {
            List<Game> games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank > @Rank AND Name LIKE @Name ORDER BY Rank ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Rank", lastRank);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

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

        public static List<Game> GetPrevious10Leaderboard(int firstRank)
        {
            List<Game> games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank < @Rank ORDER BY Rank ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Rank", firstRank);

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

        public static List<Game> GetPrevious10Leaderboard(int firstRank, string name)
        {
            List<Game> games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank < @Rank AND Name LIKE @Name ORDER BY Rank ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Rank", firstRank);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

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

        public static List<Game> Get4AboveCurrentRank(int yourRank)
        {
            List<Game> games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM (SELECT TOP (@Amount) Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank < @CurrentRank ORDER BY Rank DESC) AS subquery ORDER BY Rank ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Amount", 4 - Math.Max(0, 5 - yourRank));
                cmd.Parameters.AddWithValue("@CurrentRank", yourRank);

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

        public static Game GetExactLeaderboardEntry(Game yourGame)
        {
            Game foundGame = yourGame;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Name = @YourName AND Points = @YourPoints AND LinesCleared = @YourLines AND GameTimeInMs = @YourTime AND IsPlayer = @IsPlayer ORDER BY Rank DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@YourName", yourGame.character.name);
                cmd.Parameters.AddWithValue("@YourPoints", yourGame.points);
                cmd.Parameters.AddWithValue("@YourLines", yourGame.linesCleared);
                cmd.Parameters.AddWithValue("@YourTime", yourGame.time);
                cmd.Parameters.AddWithValue("@IsPlayer", yourGame.isPlayer);


                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    foundGame = new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5));
                }
                reader.Close();
            }

            return foundGame;
        }
    }
}
