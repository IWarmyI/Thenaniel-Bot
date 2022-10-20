using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenaniel
{
    public class DictionaryStorage
    {
        public Dictionary<string, string> osuUsers { get; set; }

        public DictionaryStorage(Dictionary<string, string> users)
        {
            osuUsers = users;
        }
    }
}
