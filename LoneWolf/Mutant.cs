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
        public static Model AttackModel = Manager.Game.Content.Load<Model>("Models\\Mutant\\attack");
        public static Model DeathModel = Manager.Game.Content.Load<Model>("Models\\Mutant\\death");
        public static Vector3 BoxLowAnchor = new Vector3(-15, 0, -15);
        public static Vector3 BoxHighAnchor = new Vector3(15, 60, 15);
        static float DefaultVelocity = 0.4f;
        private float velocity;
        private NodedPath path;
        private TimeSpan stoppedtime;
        private bool attacking;

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
        Vector3 lrot;
        public void Collide(Player player)
        {
            if (!attacking)
            {
                lrot = Rotation;
                Vector3 v = player.Position - Position;
                v.Normalize();
                float angle = (float)Math.Atan(v.X / v.Z);
                if (v.X == 0 && v.Z < 0) angle += MathHelper.Pi;
                Rotation = new Vector3(0, angle, 0);                
                Rotation = new Vector3(0, angle, 0);
                attacking = true;
                Model = AttackModel;
                StartAnimation(DefaultClip, false);
                player.TakeDamage(100);
            }
        }
        protected override void OnFinishedPlayingAnimation()
        {
            base.OnFinishedPlayingAnimation();
            if (IsIdle) Model = IdleModel;
            else Model = WalkingModel;
            Rotation = lrot;
            StartAnimation(DefaultClip);
        }
    }
}
