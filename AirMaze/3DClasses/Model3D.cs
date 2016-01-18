using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Model3D
    {
        Model model;
        protected Vector3 position;
        protected Matrix trans;
        protected Vector3 rotation;
        public Model3D(Model m,Vector3 pos)
        {
            model = m;
            position = pos;            
            rotation = Vector3.Zero;
        }
        public virtual void Update(GameTime time)
        {
            trans = Matrix.CreateScale(0.2f) * Matrix.CreateRotationX(rotation.X) *
                        Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
        }
        public virtual void Draw(Camera cam)
        {
            Matrix[] transform = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transform);
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect effect in m.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transform[m.ParentBone.Index] * trans;
                    effect.View = cam.View;
                    effect.Projection = cam.Projection;
                }
                m.Draw();
            }
        }
    }
}
