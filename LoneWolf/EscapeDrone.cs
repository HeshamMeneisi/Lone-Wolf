using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class EscapeDrone : Object3D,INPC
    {
        public static Model DroneModel = Manager.Game.Content.Load<Model>("Models\\Drone\\model");
        public static Vector3 BoxLowAnchor = new Vector3(-50, 0, -50);
        public static Vector3 BoxHighAnchor = new Vector3(50, 60, 50);
        private float vel;

        public float Velocity
        {
            get
            {
                return vel;
            }

            set
            {
                vel = value;
            }
        }
        NodedPath path;
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

        public bool Attacking
        {
            get
            {
                return false;
            }
        }

        public bool Dying
        {
            get
            {
                return false;
            }
        }
        bool isidle = true;
        public bool IsIdle
        {
            get
            {
                return isidle;
            }

            set
            {
                isidle = value;
            }
        }

        public uint ScoreIncrement
        {
            get
            {
                return 0;
            }
        }

        public EscapeDrone(Vector3 position): base(DroneModel, Vector3.Zero, Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 9)
        {
            Position = position;
            Velocity = 1f;
        }

        public void StopWalking(GameTime time)
        {
            isidle = true;
            lm = time.TotalGameTime;        
        }
        TimeSpan lm = new TimeSpan();
        public void StartWalking()
        {
            isidle = false;
        }

        public TimeSpan GetIdleTime(GameTime time)
        {
            return time.TotalGameTime.Subtract(lm);
        }

        public void Kill()
        {            
        }

        public void Collide(Object3D player)
        {
            Manager.PlayerWon();            
        }
    }
}
