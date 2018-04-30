using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

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
                using (var connection = new SqlCommand(this.ConnectionString))
                using (var command = new SqlCommand(DeletePlayerQuery/*, connection*/))
                {
                    //connection.Open();
                    command.Parameters.AddWithValue("?", id);
                    return command.ExecuteNonQuery() == 1;
                }
            }
            catch (SqlException sQLEx)
            {
                Console.Error.WriteLine(sQLEx.Message);
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
            catch (SqlException sQLEx)
            {
                Console.Error.Write(sQLEx.Message);
                return false;
            }
        }
    }
}
