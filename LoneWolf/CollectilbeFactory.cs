using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class CollectibleFactory : Factory<Collectible>
    {
        static Dictionary<byte, Type> CollectibleTypes = new Dictionary<byte, Type>
        {
            {0,typeof(StarBox)},
            {1,typeof(FirstAidBag) }
        };
        public int AvailableTypes { get { return CollectibleTypes.Count; } }
        public Collectible CreateNew(object keyname, params Object[] arguments)
        {
            if (CollectibleTypes.ContainsKey((byte)keyname))
            {
                var constructor = CollectibleTypes[(byte)keyname].GetConstructor(arguments.Select(arg => arg.GetType()).ToArray());
                if (constructor == null)
                    throw new Exception("Could not find an appropriate constructor.");
                return (Collectible)constructor.Invoke(arguments);
            }
            throw new Exception("Undefined type.");
        }
    }
}
