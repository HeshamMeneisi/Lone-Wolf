using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Drone : Enemy
    {
        public static Model Model = Manager.Game.Content.Load<Model>("Models\\Drone\\model");
        public static Vector3 BoxLowAnchor = new Vector3(-20, 40, -20);
        public static Vector3 BoxHighAnchor = new Vector3(20, 60, 20);
        public Drone(Vector3 position, NodedPath path) : base(Model, position, new Vector3(0, -40, 0), Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 1, path, 0.2f)
        {

        }
    }
}
