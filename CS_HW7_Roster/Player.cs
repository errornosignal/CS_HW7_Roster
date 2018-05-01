
using System;
using System.Data;

namespace CS_HW7_Roster
{
    /// <inheritdoc />
    /// <summary>
    /// Player Class
    /// </summary>
    public class Player : BaseObject
    {
        private int id = 0;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private int teamNumber = 0;
        
        /// <summary>
        /// Gets/Sets Player object.
        /// </summary>
        public Player player { get; set; }

        /// <summary>
        /// Gets/Sets Id property.
        /// </summary>
        public int Id
        {
            get => this.id;
            set => this.id = value >= 0 ? value : 0;
        }

        /// <summary>
        /// Gets/Sets FirstName property.
        /// </summary>
        public string FirstName
        {
            get => this.firstName;
            set => this.firstName = value ?? string.Empty;
        }

        /// <summary>
        /// Gets/Sets LastName property.
        /// </summary>
        public string LastName
        {
            get => this.lastName;
            set => this.lastName = value ?? string.Empty;
        }

        /// <summary>
        /// Gets/Sets TeamNumber property.
        /// </summary>
        public int TeamNumber
        {
            get => this.teamNumber;
            set => this.teamNumber = value >= 0 ? value : 0;
        }

        /// <summary>
        /// Concatenate FirstName and LastName propertiers into FullName.
        /// </summary>
        public string FullName => $"{this.FirstName} {this.LastName}";

        /// <summary>
        /// Calls Insert method from PlayerDataService class.
        /// </summary>
        /// <returns>bool</returns>
        public bool Insert()
        {
            var dataService = new PlayerDataService();
            return dataService.Insert(this);
        }

        /// <summary>
        /// Calls Update method.
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            return Player.Update(this.player);
        }

        /// <summary>
        /// Calls Update method from PlayerDataService class.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>bool</returns>
        public static bool Update(Player player)
        {
            return new PlayerDataService().Update(player);
        }

        /// <summary>
        /// Calls Delete method.
        /// </summary>
        /// <returns>bool</returns>
        public bool Delete()
        {
            return Player.Delete(this.Id);
        }

        /// <summary>
        /// Calls Delete method from PlayerDataService class.
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        public static bool Delete(int id)
        {
            return new PlayerDataService().Delete(id);
        }

        /// <summary>
        /// Overrides Equals() method in system.Object class.
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Player that))
            {
                return false;
            }
            else { /*doNothing()*/ }

            if (object.ReferenceEquals(this, that))
            {
                return true;
            }
            else { /*doNothing()*/ }

            return this.Id == that.Id;
        }

        /// <summary>
        /// Overrides GetHashCode() method in System.Object class.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => this.Id.GetHashCode();

        /// <summary>
        /// Overrides ToString() method in System.Object class.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{this.LastName}, {this.FirstName} => {this.Id}";

        /// <inheritdoc />
        /// <summary>
        /// Overrides MapData() method in BaseObject class.
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <returns>bool</returns>
        public override bool MapData(DataRow row)
        {
            this.Id = Convert.ToInt32(row["playerId"]);
            this.FirstName = row["firstName"].ToString();
            this.LastName = row["lastName"].ToString();
            this.TeamNumber = Convert.ToInt32(row["teamNumber"].ToString());
            return base.MapData(row);
        }
    }
}