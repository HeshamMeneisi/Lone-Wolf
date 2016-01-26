using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class EnemyFactory : Factory<Enemy>
    {
        static Dictionary<byte, Type> EnemyTypes = new Dictionary<byte, Type>
        {
            {0,typeof(Drone)},
            {1,typeof(Mutant) }
        };
        public int AvailableTypes { get { return EnemyTypes.Count; } }
        public Enemy CreateNew(object keyname, params Object[] arguments)
        {
            if (EnemyTypes.ContainsKey((byte)keyname))
            {
                var constructor = EnemyTypes[(byte)keyname].GetConstructor(arguments.Select(arg => arg.GetType()).ToArray());
                if (constructor == null)
                    throw new Exception("Could not find an appropriate constructor.");
                return (Enemy)constructor.Invoke(arguments);
            }
            throw new Exception("Undefined type.");
        }
    }
}
