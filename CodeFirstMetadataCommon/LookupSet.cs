using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    public class LookupSet<T>
    {
        private string _key;
        private Func<T, object> _lookupDeleg;

        public LookupSet(string key, Func<T, object> lookupDeleg)
        {
            _key = key;
            _lookupDeleg = lookupDeleg;
        }
    }
}
