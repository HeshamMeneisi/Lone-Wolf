using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace LoneWolf
{
    class Player : DynamicObject
    {
        public static Model Model = Manager.Game.Content.Load<Model>("Models\\Player\\model");
        public static Vector3 ModelLowAnchor = new Vector3(-5, 0, -10);
        public static Vector3 ModelHighAnchor = new Vector3(5, 25, 10);
        float speed;

        float faceheight = 30;
        Vector3 camoffset;
        public Player(Vector3 position, Vector3 rotation, float scale = 1) : base(Model, Vector3.Zero, new Vector3(0, MathHelper.Pi, 0), ModelLowAnchor, ModelHighAnchor, scale, "Take 001", true)
        {
            speed = 1f;
            camoffset = new Vector3(0, faceheight, 0);
            this.position = position;
            this.rotation = rotation;
        }
        bool ismoving = false, adjusted = false;
        public override void Update(GameTime time)
        {
            // Vector3 is passed on assignment as a clone not a reference
            Vector3 newpos = position;
            Vector3 newrot = rotation;
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
            Position = newpos;
            Rotation = newrot;
            World.GetInstance().ActiveCam.Position = Position + camoffset;
            base.Update(time);
        }

        private void StandStill()
        {
            StopAnimation(new TimeSpan(2800000));
        }

        private void StarWalking()
        {
            if (!PlayingAnimation)
                StartAnimation("Take 001");
        }
    }
}
