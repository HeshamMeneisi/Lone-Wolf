using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Drone : Model3D, Enemy
    {
        public static Model DroneModel = Manager.Game.Content.Load<Model>("Models\\Drone\\model");
        public static Vector3 BoxLowAnchor = new Vector3(-20, 40, -20);
        public static Vector3 BoxHighAnchor = new Vector3(20, 60, 20);
        static float DefaultVelocity = 0.2f;
        private float velocity;
        private NodedPath path;

        public float Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public NodedPath Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }

        public Drone(Vector3 position, NodedPath path) : base(DroneModel, new Vector3(0, -40, 0), Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 1)
        {
            Position = position;
            Path = path;
            Velocity = DefaultVelocity;
        }
    }
}
