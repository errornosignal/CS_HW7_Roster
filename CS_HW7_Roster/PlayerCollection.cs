
namespace CS_HW7_Roster
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    internal class PlayerCollection : BaseObjectCollection<Player>
    {
        /// <summary>
        /// Gets entire collection from PlayerCollection.
        /// </summary>
        /// <returns>PlayerCollection</returns>
        public static PlayerCollection GetAll()
        {
            var collection = new PlayerCollection();
            var dataSet = new PlayerDataService().GetAll();
            collection.MapObjects(dataSet);
            return collection;
        }
    }
}