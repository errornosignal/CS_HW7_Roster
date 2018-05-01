
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CS_HW7_Roster
{
    /// <summary>
    /// PlayerDataService class.
    /// </summary>
    public class PlayerDataService
    {
        private bool insertOp = false;
        private bool updateOp = false;
        private bool deleteOp = false;
        
        /// <summary>
        /// Gets datasource connection string.
        /// </summary>
        private string ConnectionString { get; } = 
            @"Data Source=DAX;Initial Catalog=Players;Integrated Security=True";

        /// <summary>
        /// Gets query string for insert operation.
        /// </summary>
        private string InsertPlayerQuery { get; } =
            "INSERT INTO Players (firstName, lastName, teamNumber) " +
            "VALUES (@firstName, @lastName, @teamNumber)";

        /// <summary>
        /// Gets query string for update operation.
        /// </summary>
        private string UpdatePlayerQuery { get; } =
            "UPDATE Players " +
            "SET firstName = @firstName, lastName = @lastName, teamNumber = @teamNumber " +
            "WHERE playerId = @playerID";

        /// <summary>
        /// Gets query string for delete operation.
        /// </summary>
        private string DeletePlayerQuery { get; } =
            "DELETE FROM Players " +
            "WHERE playerId = @playerID";

        /// <summary>
        /// Gets query string to select all records in table.
        /// </summary>
        private string SelectAllQuery { get; } =
            "SELECT playerId, firstName, lastName, teamNumber " +
            "FROM Players";

        /// <summary>
        /// Gets data set from datasource.
        /// </summary>
        /// <returns>DataSet</returns>
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

        /// <summary>
        /// Performs Innert,Update, or Delete operation on datasource.
        /// </summary>
        /// <param name="query">string</param>
        /// <param name="parameters">params string[]</param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Calls ExecuteNonQuery to perform Insert.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>bool</returns>
        public bool Insert(Player player)
        {
            insertOp = true;
            return this.ExecuteNonQuery(this.InsertPlayerQuery,
                player.FirstName,
                player.LastName,
                player.TeamNumber.ToString());
        }

        /// <summary>
        /// Calls ExecuteNonQuery to perform Update.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>bool</returns>
        public bool Update(Player player)
        {
            updateOp = true;
            return this.ExecuteNonQuery(this.UpdatePlayerQuery,
                player.Id.ToString(),
                player.FirstName,
                player.LastName,
                player.TeamNumber.ToString());
        }

        /// <summary>
        /// Calls ExecuteNonQuery to perform Delete..
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public bool Delete(int id)
        {
            deleteOp = true;
            return this.ExecuteNonQuery(this.DeletePlayerQuery, 
                id.ToString());
        }
    }
}