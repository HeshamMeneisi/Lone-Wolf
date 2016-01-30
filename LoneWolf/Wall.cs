using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoneWolf
{
    class Wall : Object3D
    {
        public static Vector3 WallLowAnchor = new Vector3(-10, 0, -50);
        public static Vector3 WallHighAnchor = new Vector3(10, 100, 50);
        public static float Tolerance = 6;
        public Wall(Model model, Vector3 origin, Vector3 position, float scale, float orientation = 0) : base(model, origin, Vector3.Zero, WallLowAnchor - new Vector3(Tolerance, 0, Tolerance), WallHighAnchor + new Vector3(Tolerance, 0, Tolerance), scale - (orientation == 0 ? 0.0006f : 0))
        {
            Position = position;
            Rotation = new Vector3(0, MathHelper.PiOver2 * orientation, 0);
        }
    }
}
