using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    public interface IMetadataLoader<T>
    {
        T LoadFromFile(string fileName, string AttributeIdentifier);
        T LoadFromString(string input, string AttributeIdentifier);
    }
}
