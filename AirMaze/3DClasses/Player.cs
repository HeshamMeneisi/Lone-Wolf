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
    class Player : Model3D
    {
        float speed;

        Vector3 camoffset = new Vector3(-50, 180, -50);
        float faceheight;

        public Player(Model m, Vector3 position, Vector3 rotation, float scale) : base(m, position, rotation, scale)
        {
            speed = 1f;
            faceheight = 25;
        }
        public override void Update(GameTime time)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                rotation.Y += .05f;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                rotation.Y -= .05f;
            }
            if (ks.IsKeyDown(Keys.W))
            {
                position.X += speed * (float)Math.Sin(rotation.Y);
                position.Z += speed * (float)Math.Cos(rotation.Y);
            }
            if (ks.IsKeyDown(Keys.S))
            {
                position.X -= speed * (float)Math.Sin(rotation.Y);
                position.Z -= speed * (float)Math.Cos(rotation.Y);
            }
            World.GetInstance().ActiveCam.Position = position + (camoffset * new Vector3((float)Math.Sin(rotation.Y), 1, (float)Math.Cos(rotation.Y)));
            World.GetInstance().ActiveCam.LookAt = position + new Vector3(0, faceheight, 0);
            base.Update(time);
        }
    }
}
