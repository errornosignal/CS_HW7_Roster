
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CS_HW7_Roster
{
    public partial class Form1 : Form
    {
        public string ConnectionString { get; } = 
            @"Data Source=DAX;Initial Catalog=Players;Integrated Security=True";

        public List<Player> Players { get; } = new List<Player>();

        private PlayerCollection Collection { get; set; } = new PlayerCollection();

        public Form1() => this.InitializeComponent();

        private bool insertOp = false;
        private bool updateOp = false;

        /// <summary>
        /// Sets initial form load state.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.PlayersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Collection = PlayerCollection.GetAll();

            DisableTextBoxes();
            DisableAcceptCancel();

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
                    else { /*doNothing()*/ }
                }
                else { /*doNothing()*/ }

                this.BindComboBox();
            }
            catch (SqlException sQlEx)
            {
                Console.Error.WriteLine(sQlEx.Message);
            }
        }

        /// <summary>
        ///Binds datasource to combobox.
        /// </summary>
        private void BindComboBox()
        {
            this.PlayersComboBox.DataSource = null;
            this.PlayersComboBox.DataSource = this.Collection;
            this.PlayersComboBox.DisplayMember = "FullName";
            this.PlayersComboBox.ValueMember = "Id";
        }

        /// <summary>
        /// Event handler for PlayersComboBox_SelectedValueChanged.
        /// Displays combobox selected item record details in form textboxes.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void PlayersComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.PlayersComboBox.SelectedIndex > 0)
            {
                var player = this.Players[this.PlayersComboBox.SelectedIndex];
                this.FirstNameTextBox.Text = player.FirstName;
                this.LastNameTextBox.Text = player.LastName;
                this.TeamNumberTextBox.Text = player.TeamNumber.ToString();
            }
            else { /*doNothing()*/ }
        }

        /// <summary>
        /// Event handler for AddButton_Click.
        /// Sets appropriate form state upon clicking this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            insertOp = true;
            ClearTextBoxes();
            EnableTextBoxes();
            DisableAddEditDelete();
            EnableAcceptCancel();
        }

        /// <summary>
        /// Event handler for EditButton_Click.
        /// Sets appropriate form state upon clicking this button.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void EditButton_Click(object sender, EventArgs e)
        {
            updateOp = true;
            EnableTextBoxes();
            DisableAddEditDelete();
            EnableAcceptCancel();
        }

        /// <summary>
        /// Event handler for DeleteButton_Click.
        /// Calls method to delete a record matching the provided criteria and provides the user
        ///     with a success/failure message.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Collection.Count > 0)
            {
                var player = this.Players[this.PlayersComboBox.SelectedIndex];

                MessageBox.Show(
                    Player.Delete(player.Id) ? $"Player {player.FullName} deleted :)" : "Player not deleted :(");

                ClearTextBoxes();
                Form1_Load(sender, e);
            }
            else { /*doNothing()*/ }
        }

        /// <summary>
        /// Event handler for AcceptButton_Click.
        /// Calls methods to either insert a record or updatea a record matching the provided 
        ///     criteria and provides the user with a success/failure message.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var firstName = this.FirstNameTextBox.Text;
            var lastName = this.LastNameTextBox.Text;
            var teamNumber = this.TeamNumberTextBox.Text;

            if (firstName.Equals("") || lastName.Equals("") || teamNumber.Equals(""))
            {
                MessageBox.Show("You left something blank...");
            }
            else
            {
                var indexToSelect = this.PlayersComboBox.SelectedIndex;

                var teamNumberIsValid = int.TryParse(teamNumber, out var validTeamNumber);
                if (teamNumberIsValid)
                {
                    if (insertOp)
                    {
                        var player = new Player()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            TeamNumber = validTeamNumber
                        };

                        if (player.Insert())
                        {
                            indexToSelect = Collection.Count;
                            MessageBox.Show($"Player {player.FullName} inserted :)");
                        }
                        else
                        {
                            MessageBox.Show("Player not inserted :(");
                        }
                    }
                    else if (updateOp)
                    {
                        var playerToUpdate = this.Players[this.PlayersComboBox.SelectedIndex];
                        var id = playerToUpdate.Id;

                        var player = new Player()
                        {
                            Id = Convert.ToInt32(id),
                            FirstName = this.FirstNameTextBox.Text,
                            LastName = this.LastNameTextBox.Text,
                            TeamNumber = validTeamNumber
                        };

                        MessageBox.Show(
                            Player.Update(player) ? $"Player {player.FullName} updated :)" : "Player not updated :(");
                    }

                    insertOp = false;
                    updateOp = false;

                    CancelButton.PerformClick();
                    Form1_Load(sender, e);

                    this.PlayersComboBox.SelectedIndex = indexToSelect;
                }
                else
                {
                    MessageBox.Show("Team number must be an integer value...");
                }
            }
        }

        /// <summary>
        /// Event handler for CancelButton_Click.
        /// Resets the form state upon cancellation of add/edit operations.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            DisableTextBoxes();
            EnableAddEditDelete();
            DisableAcceptCancel();

            this.PlayersComboBox_SelectedValueChanged(sender, e);
        }

        /// <summary>
        /// Event handler for ExitButton_Click.
        /// Closes this form.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Clears the forms textboxes on the form.
        /// </summary>
        private void ClearTextBoxes()
        {
            this.FirstNameTextBox.Clear();
            this.LastNameTextBox.Clear();
            this.TeamNumberTextBox.Clear();
        }

        /// <summary>
        /// Enables the relevant textboxes on the form.
        /// </summary>
        private void EnableTextBoxes()
        {
            this.FirstNameTextBox.Enabled = true;
            this.LastNameTextBox.Enabled = true;
            this.TeamNumberTextBox.Enabled = true;
        }

        /// <summary>
        /// Disables the relevant textboxes on the form.
        /// </summary>
        private void DisableTextBoxes()
        {
            this.FirstNameTextBox.Enabled = false;
            this.LastNameTextBox.Enabled = false;
            this.TeamNumberTextBox.Enabled = false;
        }

        /// <summary>
        /// Enables the relevant buttons on the form.
        /// </summary>
        private void EnableAddEditDelete()
        {
            this.AddButton.Enabled = true;
            this.EditButton.Enabled = true;
            this.DeleteButton.Enabled = true;
        }

        /// <summary>
        /// DIsables the relevant buttons on the form.
        /// </summary>
        private void DisableAddEditDelete()
        {
            this.AddButton.Enabled = false;
            this.EditButton.Enabled = false;
            this.DeleteButton.Enabled = false;
        }

        /// <summary>
        /// Enables the relevant butrtons on the form.
        /// </summary>
        private void EnableAcceptCancel()
        {
            this.AcceptButton.Enabled = true;
            this.CancelButton.Enabled = true;
        }

        /// <summary>
        /// Enables the relevant buttons on the form.
        /// </summary>
        private void DisableAcceptCancel()
        {
            this.AcceptButton.Enabled = false;
            this.CancelButton.Enabled = false;
        }
    }
}