using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    class Model3D
    {
        Model model;
        protected Vector3 position;
        protected Vector3 rotation;
        protected float scale = 1f;
        protected Matrix trans;

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public Model3D(Model m, Vector3 pos = default(Vector3), Vector3 rot = default(Vector3), float scale = 1)
        {
            model = m;
            position = pos;
            rotation = rot;
            this.scale = scale;
        }

        internal float DistanceTo(Vector3 position)
        {
            return (position - Position).Length();
        }

        public virtual void Update(GameTime time)
        {
            trans = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);
        }
        public virtual void Draw(Camera cam)
        {
            //model.Draw(trans, cam.View, cam.Projection);            
            Matrix[] transform = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transform);                        
            foreach (ModelMesh m in model.Meshes)
            {                
                foreach (BasicEffect effect in m.Effects)
                {
                    //effect.VertexColorEnabled = true;
                    //effect.TextureEnabled = true;
                    effect.EnableDefaultLighting();
                    //effect.PreferPerPixelLighting = true;
                    effect.World = transform[m.ParentBone.Index] * trans;
                    effect.View = cam.View;
                    effect.Projection = cam.Projection;
                }
                m.Draw();
            }
        }
    }
}
