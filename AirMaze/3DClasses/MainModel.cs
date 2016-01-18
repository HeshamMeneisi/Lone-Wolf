using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace LoneWolf.Extra
{
    class Player : Model3D
    {

        Model model;
        public Matrix view, projection;
        float speed;
        public Player(Model m, Vector3 position) : base(m, position)
        {
            speed = 15f;
            model = m;
            this.position = position;
            rotation = Vector3.Zero;            
            //view = Matrix.CreateLookAt(campos, Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                Manager.Parent.GraphicsDevice.Viewport.AspectRatio, 0.001f, 1000f);
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
            World.GetInstance().ActiveCam.Position =
            new Vector3(position.X - 80 * (float)Math.Sin(rotation.Y)
            , position.Y, position.Z - 80 * (float)Math.Cos(rotation.Y));
            World.GetInstance().ActiveCam.LookAt = position;
            base.Update(time);
        }
    }
}
