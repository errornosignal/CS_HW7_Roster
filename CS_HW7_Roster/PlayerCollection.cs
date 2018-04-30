
namespace CS_HW7_Roster
{
    internal class PlayerCollection : BaseObjectCollection<Player>
    {
        public static PlayerCollection GetAll()
        {
            var collection = new PlayerCollection();
            var dataSet = new PlayerDataService().GetAll();
            collection.MapObjects(dataSet);
            return collection;
        }
    }
}