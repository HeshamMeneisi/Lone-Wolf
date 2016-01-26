using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LoneWolf
{
    class WallFactory : Factory<Wall>
    {
        static Dictionary<byte, Type> WallTypes = new Dictionary<byte, Type>
        {
            {0,typeof(BrickWall)},
            {1,typeof(WallWithPlants) },
            {2,typeof(Fence) }
        };
        public int AvailableTypes { get { return WallTypes.Count; } }
        public Wall CreateNew(object keyname, params Object[] arguments)
        {
            if (WallTypes.ContainsKey((byte)keyname))
            {
                var constructor = WallTypes[(byte)keyname].GetConstructor(arguments.Select(arg => arg.GetType()).ToArray());
                if (constructor == null)
                    throw new Exception("Could not find an appropriate constructor.");
                return (Wall)constructor.Invoke(arguments);
            }
            throw new Exception("Undefined type.");
        }
    }
}