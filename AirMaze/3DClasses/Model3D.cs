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
        protected Vector3 rot;
        protected Matrix view,projection;
        public Model3D(Model m,Matrix proj,Vector3 pos)
        {
            model = m;
            projection = proj;
            position = pos;
            view = Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
            rot = Vector3.Zero;
        }
        public virtual void Update()
        {
            trans = Matrix.CreateRotationX(rot.X) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateRotationZ(rot.Z);
        }
        public virtual void Draw()
        {
            Update();
            Matrix[] transform = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transform);
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect effect in m.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transform[m.ParentBone.Index] * trans;
                    effect.View = view;
                    effect.Projection = projection;
                }
                m.Draw();
            }
        }
    }
}
