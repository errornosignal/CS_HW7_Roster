
using System.Data;

namespace CS_HW7_Roster
{
    /// <summary>
    /// BaseObject class.
    /// </summary>
    public abstract class BaseObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet">DataRow</param>
        /// <returns>bool</returns>
        public virtual bool MapData(DataSet dataSet)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table">DataRow</param>
        /// <returns>bool</returns>
        public virtual bool MapData(DataTable table)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <returns>bool</returns>
        public virtual bool MapData(DataRow row)
        {
            return false;
        }
    }
}