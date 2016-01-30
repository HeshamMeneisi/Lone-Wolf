using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LoneWolf
{
    class Player : DynamicObject
    {
        public static Model IdleModel = Manager.Game.Content.Load<Model>("Models\\Player\\idle");
        public static Model WalkingModel = Manager.Game.Content.Load<Model>("Models\\Player\\walking");
        public static Model DeathModel = Manager.Game.Content.Load<Model>("Models\\Player\\death");
        public static Model FireModel = Manager.Game.Content.Load<Model>("Models\\Player\\fire");
        public static Vector3 ModelLowAnchor = new Vector3(-10, 0, -10);
        public static Vector3 ModelHighAnchor = new Vector3(10, 25, 10);
        static SoundEffect[] StepSounds = new SoundEffect[] { DataHandler.Sounds[SoundType.Step], DataHandler.Sounds[SoundType.Step2], DataHandler.Sounds[SoundType.Step3], DataHandler.Sounds[SoundType.Step4] };
        float speed;

        float faceheight = 30;
        Vector3 camoffset;
        public Player(Vector3 position, Vector3 rotation, float scale = 1) : base(IdleModel, Vector3.Zero, new Vector3(0, 0, 0), ModelLowAnchor, ModelHighAnchor, scale, "Take 001", true)
        {
            speed = 1f;
            camoffset = new Vector3(0, faceheight, 0);
            Position = position;
            Rotation = rotation;
            Alive = true;
            InputManager.MouseDown += mousedonw;
        }

        private void mousedonw(InputManager.MouseKey k, Vector2 position)
        {
            if (!Destroyed && Alive && !Attacking && k == InputManager.MouseKey.LeftKey)
            {
                Attacking = true;
                Rotation = new Vector3(0, World.GetInstance().ActiveCam.Rotation.Y + (float)Math.PI, 0);
                SoundManager.PlaySound(DataHandler.Sounds[SoundType.Throw], SoundCategory.SFX);
                Model = FireModel;
                StartAnimation(DefaultClip, false);
                float dist = 10;
                var daggerpos = Position + new Vector3((float)Math.Sin(Rotation.Y), 0, (float)Math.Cos(Rotation.Y)) * dist;
                var dir = (daggerpos - Position);
                dir.Normalize();
                var dagger = new Dagger(daggerpos, dir);
                World.GetInstance().Add(dagger);
            }
        }

        internal void TakeDamage(int damage)
        {
            if (Alive)
            {
                int health = Manager.UserData.GameState.Health;
                health -= damage;
                if (health <= 0)
                {
                    health = 0;
                    Death();
                }
                Manager.UserData.GameState.Health = health;
            }
        }

        private void Death()
        {
            if (Alive)
            {
                SoundManager.PlaySound(DataHandler.Sounds[SoundType.Playerdeath], SoundCategory.SFX);
                Alive = false;
                Model = DeathModel;
                StartAnimation(DefaultClip, false);
                Manager.GameOver();
            }
        }

        internal void Heal(int healamout)
        {
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Score], SoundCategory.SFX);
            if (Manager.UserData.GameState.Health + healamout > MaxHealth)
                Manager.UserData.GameState.Health = MaxHealth;
            else Manager.UserData.GameState.Health += healamout;
        }

        bool ismoving = false, adjusted = false;

        public static int MaxHealth = 100;

        public bool Alive { get; private set; }
        public bool Attacking { get; private set; }
        public override void OnFinishedPlayingAnimation()
        {
            base.OnFinishedPlayingAnimation();
            if (Alive)
            {
                Attacking = false;
                Model = IdleModel;
                StartAnimation(DefaultClip);
            }
        }
        static Random ran = new Random();
        public override void Update(GameTime time)
        {
            Vector3 newpos = Position;
            Vector3 newrot = Rotation;
            if (!Attacking && Alive)
            {
                // Vector3 is passed on assignment as a clone not a reference
                if (InputManager.IsKeyDown(Keys.W))
                {
                    if (InputManager.IsKeyDown(Keys.A))
                    {
                        newrot.Y += 0.05f;
                        adjusted = true;
                    }
                    else if (InputManager.IsKeyDown(Keys.D))
                    {
                        newrot.Y -= 0.05f;
                        adjusted = true;
                    }
                    else if (!ismoving || !adjusted)
                        newrot = new Vector3(0, World.GetInstance().ActiveCam.Rotation.Y + (float)Math.PI, 0);
                    newpos.X += speed * (float)Math.Sin(newrot.Y);
                    newpos.Z += speed * (float)Math.Cos(newrot.Y);
                    ismoving = true;
                }
                else ismoving = adjusted = false;
                var test = new TimeSpan(0, 0, 1);
                if (ismoving)
                    StarWalking();
                else
                    StandStill();
            }
            float prog = AnimationProgress;
            if (ismoving && (int)(prog * 100) % 50 == 0)
                SoundManager.PlaySound(StepSounds[ran.Next(StepSounds.Length)], SoundCategory.SFX);
            if (Attacking && ((int)(prog * 100) > 50))
                    OnFinishedPlayingAnimation();
            Position = newpos;
            Rotation = newrot;
            World.GetInstance().ActiveCam.Position = Position + camoffset;
            base.Update(time);
        }

        private void StandStill()
        {
            if (Alive && Model != IdleModel)
                Model = IdleModel;
        }

        private void StarWalking()
        {
            if (Alive && Model != WalkingModel)
                Model = WalkingModel;
        }

        public override void SeparateFrom(Object3D stc)
        {
            if (!Alive) return;
            base.SeparateFrom(stc);
            if (stc is IInteractiveObject && InputManager.IsKeyDown(Keys.E))
                ((IInteractiveObject)stc).Interact(this);
            if (stc is IObstacle)
                ((IObstacle)stc).Collide(this);
        }
    }
}
