using E_BangDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.StaticData
{
    /// <summary>
    /// Represents static data for a collection of actions.
    /// Provides functionality to load and expose actions as a read-only list.
    /// </summary>
    public class ActionStaticData
    {
        public IReadOnlyList<Actions> Actions;

        public ActionStaticData()
        {
            Actions = [];
        }
        public void LoadData(IEnumerable<Actions> actions)
        {
            Actions = actions.ToList().AsReadOnly();
        }
    }
}
