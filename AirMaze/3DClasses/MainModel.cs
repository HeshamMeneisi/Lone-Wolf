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
    class MainModel
    {

        Model model;
        public Matrix view, projection;
        Vector3 pos;
        Vector3 campos;
        Vector3 rotation;
        float speed;
        public MainModel(Model m, Vector3 positition, GraphicsDeviceManager graphics)
        {
            speed = 15f;
            model = m;
            pos = positition;
            rotation = Vector3.Zero;
            campos = new Vector3(pos.X, pos.Y - 25, pos.Z - 20);
            //view = Matrix.CreateLookAt(campos, Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                graphics.GraphicsDevice.Viewport.AspectRatio, 0.001f, 1000f);
        }
        public void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            bool change = false;
            rotation.Y += .05f;
            if (ks.IsKeyDown(Keys.A))
            {
                rotation.Y += .05f;
                change = true;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                rotation.Y -= .05f;
                change = true;
            }
            if (ks.IsKeyDown(Keys.W))
            {
                pos.X += speed * (float)Math.Sin(rotation.Y);
                pos.Z += speed * (float)Math.Cos(rotation.Y);
                change = true;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                pos.X -= speed * (float)Math.Sin(rotation.Y);
                pos.Z -= speed * (float)Math.Cos(rotation.Y);
                change = true;
            }
            campos.X = pos.X - 20 * (float)Math.Sin(rotation.Y);
            campos.Z = pos.Z - 20 * (float)Math.Cos(rotation.Y);
            view = Matrix.CreateLookAt(campos, pos, Vector3.Up);
        }
        public void Draw()
        {
            Update();
            Matrix[] trans = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(trans);
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect effect in m.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = trans[m.ParentBone.Index] * Matrix.CreateScale(0.2f) * Matrix.CreateRotationX(rotation.X) *
                        Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(pos);
                    effect.View = view;
                    effect.Projection = projection;
                }
                m.Draw();
            }
        }
    }
}
