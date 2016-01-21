using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    class Model3D
    {
        protected Model model;
        protected Vector3 modeloffset;
        protected Vector3 modelbaserot;
        protected Vector3 lowanchor;
        protected Vector3 highanchor;
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

        public Model3D(Model m, Vector3 offset, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale = 1)
        {
            model = m;
            modeloffset = offset;
            modelbaserot = baserot;
            this.scale = scale;
            this.lowanchor = lowanchor;
            this.highanchor = highanchor;
        }

        internal float DistanceTo(Vector3 position)
        {
            return (position - Position).Length();
        }

        public virtual void Update(GameTime time)
        {
            trans = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y + modelbaserot.Y, rotation.X + modelbaserot.X, rotation.Z + modelbaserot.Z) * Matrix.CreateTranslation(position + modeloffset);
        }
        public virtual void Draw(Camera cam)
        {
            model.Draw(trans, cam.View, cam.Projection);
            /*
            if (model != null)
            {
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
            }*/
        }
    }
}
