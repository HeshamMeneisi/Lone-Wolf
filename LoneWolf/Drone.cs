using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Drone : Object3D, Enemy
    {
        public static Model DroneModel = Manager.Game.Content.Load<Model>("Models\\Drone\\model");
        public static Vector3 BoxLowAnchor = new Vector3(-20, 40, -20);
        public static Vector3 BoxHighAnchor = new Vector3(20, 60, 20);
        static float DefaultVelocity = 0.2f;
        private float velocity;
        private NodedPath path;
        private TimeSpan stoppedtime;

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

        public bool IsIdle
        {
            get; set;
        }
        bool attacking = false;
        public bool Attacking
        {
            get
            {
                return attacking;
            }
        }

        public bool Dying
        {
            get
            {
                return false;                
            }
        }

        public uint ScoreIncrement
        {
            get
            {
                return 1000;
            }
        }

        public Drone(Vector3 position, NodedPath path) : base(DroneModel, new Vector3(0, -60, 0), Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 1.5f)
        {
            Position = position;
            Path = path;
            Velocity = DefaultVelocity;
        }
        
        public void StopWalking(GameTime time)
        {
            stoppedtime = time.TotalGameTime;
            IsIdle = true;
        }

        public void StartWalking()
        {
            IsIdle = false;
        }

        public TimeSpan GetIdleTime(GameTime time)
        {
            return time.TotalGameTime.Subtract(stoppedtime);
        }

        public void Collide(Object3D player)
        {            
        }

        public void Kill()
        {            
        }
    }
}
