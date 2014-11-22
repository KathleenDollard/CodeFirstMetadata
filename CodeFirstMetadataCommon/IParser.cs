﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    public interface IParser<T>
    {
        IEnumerable<T> Parse(string input);
    }
}
