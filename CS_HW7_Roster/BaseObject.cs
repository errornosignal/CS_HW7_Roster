using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_HW7_Roster
{
    public abstract class BaseObject
    {
        public virtual bool MapData(DataSet dataSet)
        {
            return false;
        }

        public virtual bool MapData(DataTable table)
        {
            return false;
        }

        public virtual bool MapData(DataRow row)
        {
            return false;
        }
    }
}
