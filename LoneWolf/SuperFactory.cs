using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class SuperFactory
    {
        static Dictionary<Type, Factory> Factories = new Dictionary<Type, Factory>
        {
            {typeof(Wall),new WallFactory() },
            {typeof(INPC),new NPCFactory() },
            {typeof(Collectible),new CollectibleFactory() }
        };
        public static Factory<T> GetFactory<T>()
        {
            return (Factory<T>)Factories[typeof(T)];
        }
    }
}
