using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoneWolf
{
    class BrickWall : Wall
    {
        public static byte Code = 0x0;

        public static Model Model = Manager.Game.Content.Load<Model>("Models\\Wall\\model");
        public BrickWall(Vector3 position, float orientation) : base(Model, new Vector3(0, -50, 0), position, 1, orientation)
        { }
    }
}
