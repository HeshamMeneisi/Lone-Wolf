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
    class Player : SkinnedModel3D
    {
        public static Model Model = Manager.Game.Content.Load<Model>("Models\\Player\\model");
        float speed;

        float faceheight = 30;
        Vector3 camoffset;
        public Player(Vector3 position, Vector3 rotation, float scale = 1) : base(Model, Vector3.Zero, new Vector3(0, MathHelper.Pi, 0), position, rotation, scale, "Take 001")
        {
            speed = 1f;
            camoffset = new Vector3(0, faceheight, 0);
            this.position = position;
            this.rotation = rotation;
        }
        public override void Update(GameTime time)
        {
            bool ismoving = false;
            if (InputManager.IsKeyDown(Keys.A))
            {
                ismoving = true;
                rotation.Y += .05f;
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                ismoving = true;
                rotation.Y -= .05f;
            }
            if (InputManager.IsKeyDown(Keys.W))
            {
                ismoving = true;
                position.X += speed * (float)Math.Sin(rotation.Y);
                position.Z += speed * (float)Math.Cos(rotation.Y);
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                ismoving = true;
                position.X -= speed * (float)Math.Sin(rotation.Y);
                position.Z -= speed * (float)Math.Cos(rotation.Y);
            }
            var test = new TimeSpan(0, 0, 1);
            if (ismoving)
                StarWalking();
            else
                StandStill();
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
