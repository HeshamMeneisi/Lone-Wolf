using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Mutant : SkinnedModel3D, Enemy
    {
        public static Model WalkingModel = Manager.Game.Content.Load<Model>("Models\\Mutant\\walking");
        public static Model IdleModel = Manager.Game.Content.Load<Model>("Models\\Mutant\\idle");
        public static Vector3 BoxLowAnchor = new Vector3(-20, 40, -20);
        public static Vector3 BoxHighAnchor = new Vector3(20, 60, 20);
        static float DefaultVelocity = 0.3f;
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

        public Mutant(Vector3 position, NodedPath path) : base(WalkingModel, Vector3.Zero, Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 0.25f, "Take 001")
        {
            Position = position;
            Path = path;
            Velocity = DefaultVelocity;
        }

        public void StopWalking(GameTime time)
        {
            stoppedtime = time.TotalGameTime;
            IsIdle = true;
            Model = IdleModel;
        }

        public void StartWalking()
        {
            IsIdle = false;
            Model = WalkingModel;
        }

        public TimeSpan GetIdleTime(GameTime time)
        {
            return time.TotalGameTime.Subtract(stoppedtime);
        }
    }
}
