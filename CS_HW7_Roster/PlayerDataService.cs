
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CS_HW7_Roster
{
    public class PlayerDataService
    {
        private bool insertOp = false;
        private bool updateOp = false;
        private bool deleteOp = false;
        
        private string ConnectionString { get; } = 
            @"Data Source=DAX;Initial Catalog=Players;Integrated Security=True";

        private string InsertPlayerQuery { get; } =
            "INSERT INTO Players (firstName, lastName, teamNumber) " +
            "VALUES (@firstName, @lastName, @teamNumber)";

        private string UpdatePlayerQuery { get; } =
            "UPDATE Players " +
            "SET firstName = @firstName, lastName = @lastName, teamNumber = @teamNumber " +
            "WHERE playerId = @playerID";

        private string DeletePlayerQuery { get; } =
            "DELETE FROM Players " +
            "WHERE playerId = @playerID";
        
        private string SelectAllQuery { get; } =
            "SELECT playerId, firstName, lastName, teamNumber " +
            "FROM Players";

        public DataSet GetAll()
        {
            var data = new DataSet();
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                using (var command = new SqlCommand(this.SelectAllQuery, connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    connection.Open();
                    adapter.Fill(data);
                    return data;
                }
            }
            catch (SqlException sQlEx)
            {
                Console.Error.WriteLine(sQlEx.Message);
                return data;
            }
        }

        private bool ExecuteNonQuery(string query, params string[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    if (parameters != null)
                    {
                        if (insertOp)
                        {
                            var parameterList = new List<SqlParameter>()
                            {
                                new SqlParameter("@firstName", SqlDbType.NVarChar)
                                    { Value = parameters[0]},
                                new SqlParameter("@lastName", SqlDbType.NVarChar)
                                    { Value = parameters[1]},
                                new SqlParameter("@teamNumber", SqlDbType.Int)
                                    { Value = parameters[2]}
                            };
                            command.Parameters.AddRange(parameterList.ToArray());
                        }
                        else if (updateOp)
                        {
                            var parameterList = new List<SqlParameter>()
                            {
                                new SqlParameter("@playerID", SqlDbType.Int)
                                    { Value = parameters[0]},
                                new SqlParameter("@firstName", SqlDbType.NVarChar)
                                    { Value = parameters[1]},
                                new SqlParameter("@lastName", SqlDbType.NVarChar)
                                    { Value = parameters[2]},
                                new SqlParameter("@teamNumber", SqlDbType.Int)
                                    { Value = parameters[3]}
                            };
                            command.Parameters.AddRange(parameterList.ToArray());
                        }
                        else if (deleteOp)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue("@playerID", param);
                            }
                        }
                    }
                    else { /*doNothing()*/ }

                    insertOp = false;
                    updateOp = false;
                    deleteOp = false;

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
            insertOp = true;
            return this.ExecuteNonQuery(this.InsertPlayerQuery,
                player.FirstName,
                player.LastName,
                player.TeamNumber.ToString());
        }

        public bool Update(Player player)
        {
            updateOp = true;
            return this.ExecuteNonQuery(this.UpdatePlayerQuery,
                player.Id.ToString(),
                player.FirstName,
                player.LastName,
                player.TeamNumber.ToString());
        }

        public bool Delete(int id)
        {
            deleteOp = true;
            return this.ExecuteNonQuery(this.DeletePlayerQuery, 
                id.ToString());
        }
    }
}