using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    abstract class GroundMonster : SkinnedModel3D, INPC
    {
        public abstract Model WalkingModel { get; }
        public abstract Model IdleModel { get; }
        public abstract Model AttackModel { get; }
        public abstract Model DeathModel { get; }
        public abstract float DefaultVelocity { get; }

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

        public bool Attacking
        {
            get
            {
                return attacking;
            }
        }

        public bool Dying { get; private set; }

        public abstract uint ScoreIncrement { get; }

        public GroundMonster(Vector3 position, NodedPath path, Vector3 lowanchor, Vector3 highanchor) : base(null, Vector3.Zero, Vector3.Zero, lowanchor, highanchor, 0.25f, "Take 001", false)
        {
            Position = position;
            Model = WalkingModel;
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
        public void Collide(Object3D player)
        {
            if (!Dying && !Attacking && player is Player)
            {
                lrot = Rotation;
                Rotation = player.Rotation + new Vector3(0, MathHelper.Pi, 0);
                attacking = true;
                Model = AttackModel;
                StartAnimation(DefaultClip, false);
                DamagePlayer(((Player)player));
            }
        }

        abstract protected void DamagePlayer(Player player);

        public override void OnFinishedPlayingAnimation()
        {
            if (Dying)
            {
                NPCCoordinator.GetInstance().UnRegister(this);
                World.GetInstance().Destroy(this);
                return;
            }
            base.OnFinishedPlayingAnimation();
            if (IsIdle) Model = IdleModel;
            else Model = WalkingModel;
            Rotation = lrot;
            StartAnimation(DefaultClip);
            attacking = false;
        }

        public void Kill()
        {
            if (Dying) return;            
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Kill], SoundCategory.SFX);
            Dying = true;
            Model = DeathModel;
            StartAnimation(DefaultClip, false);
            Manager.IncrementScore(ScoreIncrement);
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Monsterdeath], SoundCategory.SFX);
        }
    }
}
