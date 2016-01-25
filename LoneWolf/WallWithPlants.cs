using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class WallWithPlants:Wall
    {
        public static Model Model = Manager.Game.Content.Load<Model>("Models\\WallWithPlants\\model");
        public WallWithPlants(Vector3 position, float orientation) : base(Model, new Vector3(0, -50, 0), position, 1, orientation)
        { }
    }
}
