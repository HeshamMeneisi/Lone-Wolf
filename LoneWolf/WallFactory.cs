using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LoneWolf
{
    internal class WallFactory
    {
        public static int AvailableTypes = 2;
        static Dictionary<byte, Type> WallTypes = new Dictionary<byte, Type>
        {
            {0,typeof(BrickWall)},
            {1,typeof(WallWithPlants) }
        };
        internal static Wall CreateNew(byte name, params Object[] arguments)
        {
            if (WallTypes.ContainsKey(name))
            {
                var constructor = WallTypes[name].GetConstructor(arguments.Select(arg => arg.GetType()).ToArray());
                if (constructor == null)
                    throw new Exception("Could not find an appropriate constructor.");
                return (Wall)constructor.Invoke(arguments);
            }
            throw new Exception("Undefined type.");
        }
    }
}