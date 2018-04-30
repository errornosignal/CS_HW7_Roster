
using System.Collections.Generic;
using System.Data;

namespace CS_HW7_Roster
{
    public abstract class BaseObjectCollection<T> :
        List<T> where T : BaseObject, new()
    {
        public bool MapObjects(DataSet dataSet)
        {
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return MapObjects(dataSet.Tables[0]);
            }
            else
            {
                return false;
            }
        }

        public bool MapObjects(DataTable table)
        {
            this.Clear();
            for (var index = 0; index < table.Rows.Count; index++)
            {
                var obj = new T();
                obj.MapData(table.Rows[index]);
                this.Add(obj);
            }

            return true;
        }
    }
}