
using System.Data;

namespace CS_HW7_Roster
{
    public class Player : BaseObject
    {
        private int id = 0;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private int teamNumber = 0;

        public int Id
        {
            get => this.id;
            set => this.id = value >= 0 ? value : 0;
        }

        public string FirstName
        {
            get => this.firstName;
            set => this.firstName = value ?? string.Empty;
        }

        public string LastName
        {
            get => this.lastName;
            set => this.lastName = value ?? string.Empty;
        }

        public int TeamNumber
        {
            get => this.teamNumber;
            set => this.teamNumber = value >= 0 ? value : 0;
        }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public bool Delete()
        {
            return Player.Delete(this.Id);
        }

        public static bool Delete(int id)
        {
            return new PlayerDataService().Delete(id);
        }

        public bool Insert()
        {
            var dataService = new PlayerDataService();
            return dataService.Insert(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Player that))
            {
                return false;
            }

            if (object.ReferenceEquals(this, that))
            {
                return true;
            }

            return this.Id == that.Id;
        }

        public override int GetHashCode() => this.Id.GetHashCode();

        public override string ToString() =>
            $"{this.LastName}, {this.FirstName} => {this.Id}";

        public override bool MapData(DataRow row)
        {
            return base.MapData(row);
        }
    }
}