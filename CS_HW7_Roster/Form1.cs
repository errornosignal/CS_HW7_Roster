
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

        private PlayerCollection Collection { get; set; } = new PlayerCollection();

        public Form1() => this.InitializeComponent();

        private void BindComboBox()
        {
            this.PlayersComboBox.DataSource = null;
            this.PlayersComboBox.DataSource = this.Collection;
            this.PlayersComboBox.DisplayMember = "FullName";
            this.PlayersComboBox.ValueMember = "Id";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.PlayersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Collection = PlayerCollection.GetAll();

            this.FirstNameTextBox.Enabled = false;
            this.LastNameTextBox.Enabled = false;
            this.NumberTextBox.Enabled = false;

            this.AcceptButton.Enabled = false;
            this.CancelButton.Enabled = false;

            this.Players.Clear();
            const string query = "SELECT playerId, firstName, lastName, teamNumber " +
                                 "FROM Players";
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
                                TeamNumber = teamNumber
                            });
                        }
                    }
                }
                this.BindComboBox();
            }
            catch (SqlException sQlEx)
            {
                Console.Error.WriteLine(sQlEx.Message);
            }
        }

        private void PlayersComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.PlayersComboBox.SelectedIndex >= 0)
            {
                var player = this.Players[this.PlayersComboBox.SelectedIndex];
                this.FirstNameTextBox.Text = player.FirstName;
                this.LastNameTextBox.Text = player.LastName;
                this.NumberTextBox.Text = player.TeamNumber.ToString();
            } // else, they (someone) clicked on something not a person!! DoNothing();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            this.FirstNameTextBox.Clear();
            this.LastNameTextBox.Clear();
            this.NumberTextBox.Clear();

            this.FirstNameTextBox.Enabled = true;
            this.LastNameTextBox.Enabled = true;
            this.NumberTextBox.Enabled = true;

            this.AddButton.Enabled = false;
            this.EditButton.Enabled = false;
            this.DeleteButton.Enabled = false;

            this.AcceptButton.Enabled = true;
            this.CancelButton.Enabled = true;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            this.FirstNameTextBox.Enabled = true;
            this.LastNameTextBox.Enabled = true;
            this.NumberTextBox.Enabled = true;

            this.AddButton.Enabled = false;
            this.EditButton.Enabled = false;
            this.DeleteButton.Enabled = false;

            this.AcceptButton.Enabled = true;
            this.CancelButton.Enabled = true;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var player = this.Players[this.PlayersComboBox.SelectedIndex];

            MessageBox.Show(
                Player.Delete(player.Id) ?
                    $"Player {player.FullName} deleted :)" :
                    "Player not deleted :(");
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var firstName = this.FirstNameTextBox.Text;
            var lastName = this.LastNameTextBox.Text;
            var teamNumber = this.NumberTextBox.Text;

            var player = new Player()
            {
                FirstName = firstName,
                LastName = lastName,
                TeamNumber = Convert.ToInt32(teamNumber)
            };

            MessageBox.Show(
                player.Insert() ?
                    $"Player {player.FullName} inserted :)" :
                    "Player not inserted :(");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.FirstNameTextBox.Enabled = false;
            this.LastNameTextBox.Enabled = false;
            this.NumberTextBox.Enabled = false;

            this.AddButton.Enabled = false;
            this.AcceptButton.Enabled = false;
            this.CancelButton.Enabled = false;

            this.AddButton.Enabled = true;
            this.EditButton.Enabled = true;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}