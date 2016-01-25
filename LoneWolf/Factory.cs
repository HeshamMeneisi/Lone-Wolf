using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    interface Factory
    {        
        int AvailableTypes { get; }
    }
    interface Factory<T>:Factory
    {                
        T CreateNew(object name, params object[] arguments);
    }
}
