
using System;
using System.Data.SqlClient;

namespace CS_HW7_Roster
{
    public class PlayerDataService
    {
        private string ConnectionString { get; } = 
            @"Data Source=DAX;Initial Catalog=Players;Integrated Security=True";

        private string InsertPersonQuery { get; } =
            "INSERT INTO Players (firstName, lastName, teamNumber) VALUES (?, ?, ?)";

        private string DeletePlayerQuery { get; } =
            "DELETE FROM Players WHERE playerId = ?";

        public bool Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                using (var command = new SqlCommand(DeletePlayerQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("?", id);
                    return command.ExecuteNonQuery() == 1;
                }
            }
            catch (SqlException sQlEx)
            {
                Console.Error.WriteLine(sQlEx.Message);
                return false;
            }
        }

        public bool Insert(Player player)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                using (var command = new SqlCommand(InsertPersonQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("?", player.FirstName);
                    command.Parameters.AddWithValue("?", player.LastName);
                    command.Parameters.AddWithValue("?", player.TeamNumber);
                    return command.ExecuteNonQuery() == 1;
                }
            }
            catch (SqlException sQlEx)
            {
                Console.Error.Write(sQlEx.Message);
                return false;
            }
        }
    }
}