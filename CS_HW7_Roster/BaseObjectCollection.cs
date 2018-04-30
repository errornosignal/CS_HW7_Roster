using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_HW7_Roster
{
    public abstract class BaseObjectCollection<T> :
        List<T> where T : BaseObject, new()
    {
        public bool MapObjects(DataSet dataSet)
        {
            return true;
        }

        public bool MapObjects(DataTable table)
        {
            var obj = new T();
            this.Add(obj);
            //return obj.MapData(table);
            return false;
        }
    }
}
