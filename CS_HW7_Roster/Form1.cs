
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CS_HW7_Roster
{
    public partial class Form1 : Form
    {
        public string ConnectionString { get; } = @"Data Source=DAX;Initial Catalog=Players;Integrated Security=True";

        public List<Player> Players { get; } = new List<Player>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.PlayersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Players.Clear();
            const string query = "SELECT playerId, firstName, lastName, teamNumber FROM Players";
            try
            {
                var data = new DataSet();

                using (var connection = new SqlConnection(this.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    connection.Open();
                    adapter.Fill(data);
                }

                if (data.Tables.Count > 0)
                {
                    var tableOne = data.Tables[0];
                    if (tableOne.Rows.Count > 0)
                    {
                        foreach (DataRow row in tableOne.Rows)
                        {
                            var id = Convert.ToInt32(row["playerId"]);
                            var firstName = row["firstName"].ToString();
                            var lastName = row["lastName"].ToString();
                            var teamNumber = Convert.ToInt32(row["teamNumber"].ToString());

                            this.Players.Add(new Player()
                            {
                                Id = id,
                                FirstName = firstName,
                                LastName = lastName,
                                TeamNumber =  teamNumber
                            });
                        }
                    }
                }
                this.BindListBox();
            }
            catch (SqlException sQlEx)
            {
                Console.Error.WriteLine(sQlEx.Message);
            }
        }

        private void BindListBox()
        {
            this.PlayersComboBox.DataSource = null;
            this.PlayersComboBox.DataSource = this.Players;
            this.PlayersComboBox.DisplayMember = "FullName";
            this.PlayersComboBox.ValueMember = "Id";
        }
    }
}