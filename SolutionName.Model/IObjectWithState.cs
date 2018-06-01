using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Model
{
    public interface IObjectWithState
    {
        //Add this to store whether the operation is an Add, Save, update or delete and apply this interface to 
        //SalesOrder class
        ObjectState ObjectState { get; set; }
    }

    public enum ObjectState
    {
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }
}
