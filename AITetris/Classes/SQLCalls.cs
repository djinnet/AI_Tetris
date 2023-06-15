using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AITetris.Classes
{
    public class SQLCalls
    {
        //connection string to the sql database
        private static readonly string connectionString = "Data Source=mssql15.unoeuro.com;Initial Catalog=barrelclickers_dk_db_fightinggame;Persist Security Info=True;User ID=barrelclickers_dk;Password=ErAaR9pmFwG5";

        //function that queries the sql database for the top 10 on the leaderboard
        public static List<Game> GetLeaderboardTop10()
        {
            //list to return
            List<Game> games = new List<Game>();

            //ready the sql connection
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get the top 10 on rank from leaderboard
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            //return the list
            return games;
        }

        //function that creates leaderboard entry
        public static void CreateLeaderboardEntry(Game gameToCreate)
        {
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to create leaderboard entry
                string query = "INSERT INTO Leaderboard (Name, Points, LinesCleared, GameTimeInMs, IsPlayer) VALUES (@Name, @Points, @LinesCleared, @GameTimeInMs, @IsPlayer)";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the different values to the to the parameters
                cmd.Parameters.AddWithValue("@Name", gameToCreate.character.name);
                cmd.Parameters.AddWithValue("@Points", gameToCreate.points);
                cmd.Parameters.AddWithValue("@LinesCleared", gameToCreate.linesCleared);
                cmd.Parameters.AddWithValue("@GameTimeInMs", gameToCreate.time);
                cmd.Parameters.AddWithValue("@IsPlayer", gameToCreate.isPlayer);
                //open the sql connection
                conn.Open();
                //execute the query
                cmd.ExecuteNonQuery();
                //close the connection
                conn.Close();
            }
        }

        //function that updates a leaderboard entry
        public static void UpdateLeaderboardEntry(Game gameToUpdate)
        {
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to update a leaderboard entry
                string query = "UPDATE Leaderboard SET Name = @Name, Points = @Points, LinesCleared = @LinesCleared, GameTimeInMs = @GameTimeInMs, IsPlayer = @IsPlayer WHERE Rank = @Rank";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the different values to the to the parameters
                cmd.Parameters.AddWithValue("@Name", gameToUpdate.character.name);
                cmd.Parameters.AddWithValue("@Points", gameToUpdate.points);
                cmd.Parameters.AddWithValue("@LinesCleared", gameToUpdate.linesCleared);
                cmd.Parameters.AddWithValue("@GameTimeInMs", gameToUpdate.time);
                cmd.Parameters.AddWithValue("@IsPlayer", gameToUpdate.isPlayer);
                cmd.Parameters.AddWithValue("@Rank", gameToUpdate.rank);
                //open the sql connection
                conn.Open();
                //execute the query
                cmd.ExecuteNonQuery();
                //close the connection
                conn.Close();
            }
        }

        //function that searches for leaderboard entries by name
        public static List<Game> SearchLeaderboardOnName(string name)
        {
            //list to return
            List<Game> foundGames = new List<Game>();
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to search for leaderboard entries by name
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Name LIKE @Name ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the value to the to the parameter
                cmd.Parameters.AddWithValue("@Name","%" + name + "%");
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    foundGames.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            return foundGames;
        }

        //function that gets the next 10 leaderboard entries by rank
        public static List<Game> GetNext10Leaderboard(int lastRank)
        {
            //list to return
            List<Game> games = new List<Game>();
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get the next 10 leaderboard entries by rank
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank > @Rank ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the value to the to the parameter
                cmd.Parameters.AddWithValue("@Rank", lastRank);
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            return games;
        }

        //function that gets the next 10 leaderboard entries by rank with searched name
        public static List<Game> GetNext10Leaderboard(int lastRank, string name)
        {
            //list to return
            List<Game> games = new List<Game>();
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get the next 10 leaderboard entries by rank with searched name
                string query = "SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank > @Rank AND Name LIKE @Name ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the different values to the to the parameters
                cmd.Parameters.AddWithValue("@Rank", lastRank);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            return games;
        }

        //function that gets the previous 10 leaderboard entries by rank
        public static List<Game> GetPrevious10Leaderboard(int firstRank)
        {
            //list to return
            List<Game> games = new List<Game>();
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get the previous 10 leaderboard entries by rank
                string query = "SELECT Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM (SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank < @Rank ORDER BY Rank DESC) AS subquery ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the value to the to the parameter
                cmd.Parameters.AddWithValue("@Rank", firstRank);
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            return games;
        }

        //function that gets the previous 10 leaderboard entries by rank with searched name
        public static List<Game> GetPrevious10Leaderboard(int firstRank, string name)
        {
            //list to return
            List<Game> games = new List<Game>();
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get the previous 10 leaderboard entries by rank with searched name
                string query = "SELECT Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM (SELECT TOP 10 Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank < @Rank AND Name LIKE @Name ORDER BY Rank DESC) AS subquery ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the different values to the to the parameters
                cmd.Parameters.AddWithValue("@Rank", firstRank);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            return games;
        }

        //function that gets the 4 above given rank
        public static List<Game> Get4AboveCurrentRank(int yourRank)
        {
            //list to return
            List<Game> games = new List<Game>();
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get the 4 above given rank
                string query = "SELECT Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM (SELECT TOP (@Amount) Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Rank <= @CurrentRank ORDER BY Rank DESC) AS subquery ORDER BY Rank ASC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the different values to the to the parameters
                cmd.Parameters.AddWithValue("@Amount", 5 - Math.Max(0, 6 - yourRank));
                cmd.Parameters.AddWithValue("@CurrentRank", yourRank);
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the list for later return
                    games.Add(new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5)));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }
            return games;
        }

        //function that gets a very specific leaderboard entry
        public static Game GetExactLeaderboardEntry(Game yourGame)
        {
            //Game to return
            Game foundGame = yourGame;
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //sql query to get a very specific leaderboard entry
                string query = "SELECT Name, Points, LinesCleared, GameTimeInMs, IsPlayer, Rank FROM Leaderboard WHERE Name = @YourName AND Points = @YourPoints AND LinesCleared = @YourLines AND GameTimeInMs = @YourTime AND IsPlayer = @IsPlayer ORDER BY Rank DESC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //add the different values to the to the parameters
                cmd.Parameters.AddWithValue("@YourName", yourGame.character.name);
                cmd.Parameters.AddWithValue("@YourPoints", yourGame.points);
                cmd.Parameters.AddWithValue("@YourLines", yourGame.linesCleared);
                cmd.Parameters.AddWithValue("@YourTime", yourGame.time);
                cmd.Parameters.AddWithValue("@IsPlayer", yourGame.isPlayer);
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();
                //start reading from the database
                while (reader.Read())
                {
                    //add readed entries to the variable for later return
                    foundGame = new Game(new Character(reader.GetString(0)), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5));
                }
                //close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }

            return foundGame;
        }

        public static int GetLastGenerationID()
        {
            int genID = 0;
            //reading the sql connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 Id FROM Generation ORDER BY Id DESC";
                //ready the call to the sql server
                SqlCommand cmd = new SqlCommand(query, conn);
                //open the sql connection
                conn.Open();
                //create a reader to read from the database using the query
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    genID = reader.GetInt32(0);
                }

                reader.Close();
                //close the connection
                conn.Close();
            }
            return genID;
        }

        // A function that inserts the AI Individual in the database
        public static void Create10IndividualsEntry(List<Individual> individuals, int genID)
        {
            // A list of AI weights
            List<string> weights = new List<string>();

            // Add weights as JSON for every individual
            foreach (Individual individual in individuals) 
            {
                weights.Add(JsonSerializer.Serialize(individual.chromosomes, new JsonSerializerOptions() { WriteIndented = true }));
            }

            // Creating a new database connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SQL query string that inserts an individual to the individual table
                string query = "INSERT INTO Individual (GenerationID, Weights, Rank)" +
                               "VALUES" +
                               "(@genID, @weights0, @rank0)," +
                               "(@genID, @weights1, @rank1)," +
                               "(@genID, @weights2, @rank2)," +
                               "(@genID, @weights3, @rank3)," +
                               "(@genID, @weights4, @rank4)," +
                               "(@genID, @weights5, @rank5)," +
                               "(@genID, @weights6, @rank6)," +
                               "(@genID, @weights7, @rank7)," +
                               "(@genID, @weights8, @rank8)," +
                               "(@genID, @weights9, @rank9)";

                // Create a new SQL command
                SqlCommand cmd = new SqlCommand(query, conn);

                // Prepare parameters
                cmd.Parameters.AddWithValue("@genID", genID);

                // Add individual weights parameters
                for (int i = 0; i < 10; i++)
                {
                    cmd.Parameters.AddWithValue($"@weights{i}", weights[i]);
                }

                // Add individual rank parameters
                for (int i = 0; i < 10; i++)
                {
                    cmd.Parameters.AddWithValue($"@rank{i}", i);
                }

                // Open the connection
                conn.Open();

                // Execute the query
                cmd.ExecuteNonQuery();
                //close the connection
                conn.Close();
            }
        }


        // A function that inserts the AI Generation in the database
        public static void CreateGenerationEntry(string genName, int genNumber, int seed)
        {
            // Creating a new database connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SQL query string that inserts a generation to the generation table
                string query = "INSERT INTO Generation (Name, GenerationNumber, Seed) VALUES (@genName, @genNumber, @seed)";

                // Create a new SQL command
                SqlCommand cmd = new SqlCommand(query, conn);

                // Prepare parameters
                cmd.Parameters.AddWithValue("@genName", genName);
                cmd.Parameters.AddWithValue("@genNumber", genNumber);
                cmd.Parameters.AddWithValue("@seed", seed);

                // Open the connection
                conn.Open();

                // Execute the query
                cmd.ExecuteNonQuery();
                //close the connection
                conn.Close();
            }
        }

        // Select top 10 generations
        public static List<AI> Load10AIGenerations()
        {
            // A list of string to hold generation data
            List<AI> generations = new List<AI>();

            // Creating a new database connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SQL query string that selects the top 10 generations baased on generation number in the generations table
                string query = "SELECT TOP 10 Id, Name, GenerationNumber, Seed FROM Generation ORDER BY GenerationNumber DESC";

                // Create a new SQL command
                SqlCommand cmd = new SqlCommand(query, conn);

                // Open the connection
                conn.Open();

                // Create a new SQL reader
                SqlDataReader reader = cmd.ExecuteReader();

                // Read as long as there is data
                while (reader.Read())
                {
                    // Add generation to list of generations
                    generations.Add(new AI(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(3), LoadPopulation(reader.GetInt32(0)), reader.GetInt32(2)));
                }

                // Close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }

            return generations;
        }

        // 
        public static Individual[] LoadPopulation(int generationID)
        {
            // 
            List<Individual> individuals = new List<Individual>();

            // Creating a new database connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // 
                string query = "SELECT Weights FROM Individual WHERE GenerationID = @generationid";

                // Create a new SQL command
                SqlCommand cmd = new SqlCommand(query, conn);

                //
                cmd.Parameters.AddWithValue("@generationid", generationID);

                // Open the connection
                conn.Open();

                // Create a new SQL reader
                SqlDataReader reader = cmd.ExecuteReader();

                // Read as long as there is data
                while (reader.Read())
                {
                    // 
                    individuals.Add(new Individual(JsonSerializer.Deserialize<int[]>(reader.GetString(0)), 0.0));
                }

                // Close the reader
                reader.Close();
                //close the connection
                conn.Close();
            }

            return individuals.ToArray();
        }
    }
}
