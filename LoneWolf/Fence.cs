using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Fence : Wall
    {
        public static Model Model = Manager.Game.Content.Load<Model>("Models\\Fence\\model");
        public static byte Code = 0x2;
        public Fence(Vector3 position, float orientation) : base(Model, Vector3.Zero, position, 3.4f, orientation)
        { }
    }
}
