﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    class Object3D : WorldElement
    {
        protected Model model;
        protected Vector3 origin;
        protected Vector3 modelbaserot;
        protected Vector3 lowanchor;
        protected Vector3 highanchor;
        private Vector3 position;
        private Vector3 rotation;
        private float scale = 1f;
        protected Matrix trans;
        private Vector3 lacache;
        private Vector3 hacache;

        public bool Destroyed { get; set; }
        protected bool changed = true;
        public Vector3 AbsoluteLowAnchor
        {
            get { return LowAnchor + position; }
        }
        public Vector3 AbsoluteHighAnchor
        {
            get { return HighAnchor + position; }
        }
        public virtual Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                changed = true;
            }
        }
        public virtual Vector3 Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
                changed = true;
            }
        }
        public virtual float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
                changed = true;
            }
        }

        public Vector3 LowAnchor
        {
            get
            {
                return changed ? lacache = new Vector3(lowanchor.X * (float)Math.Cos(Rotation.Y) + lowanchor.Z * (float)Math.Sin(Rotation.Y),
                lowanchor.Y,
                lowanchor.Z * (float)Math.Cos(Rotation.Y) + lowanchor.X * (float)Math.Sin(Rotation.Y))
                : lacache;
            }
        }
        public Vector3 HighAnchor
        {
            get
            {
                return changed ? hacache = new Vector3(highanchor.X * (float)Math.Cos(Rotation.Y) + highanchor.Z * (float)Math.Sin(Rotation.Y),
                highanchor.Y,
                highanchor.Z * (float)Math.Cos(Rotation.Y) + highanchor.X * (float)Math.Sin(Rotation.Y))
                : hacache;
            }
        }

        public virtual Model Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        public Object3D(Model m, Vector3 origin, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale = 1)
        {
            Destroyed = false;
            model = m;
            this.origin = origin;
            modelbaserot = baserot;
            this.scale = scale;
            this.lowanchor = lowanchor;
            this.highanchor = highanchor;
        }
        public float DistanceTo(Vector3 position)
        {
            return (position - Position).Length();
        }
        public bool Intersects(Object3D target)
        {
            Vector3 ta1 = target.AbsoluteLowAnchor;
            Vector3 ta2 = target.AbsoluteHighAnchor;
            Vector3 a1 = AbsoluteLowAnchor;
            Vector3 a2 = AbsoluteHighAnchor;
            bool xzinter = new RectangleF(ta1.X, ta1.Z, ta2.X, ta2.Z, true).Intersects(new RectangleF(a1.X, a1.Z, a2.X, a2.Z, true));
            bool xyinter = new RectangleF(ta1.X, ta1.Y, ta2.X, ta2.Y, true).Intersects(new RectangleF(a1.X, a1.Y, a2.X, a2.Y, true));
            bool zyinter = new RectangleF(ta1.Z, ta1.Y, ta2.Z, ta2.Y, true).Intersects(new RectangleF(a1.Z, a1.Y, a2.Z, a2.Y, true));
            return xzinter && xyinter && zyinter;
        }
        public bool Contains(Vector3 point)
        {
            Vector3 a1 = AbsoluteLowAnchor;
            Vector3 a2 = AbsoluteHighAnchor;
            bool xzinter = new RectangleF(a1.X, a1.Z, a2.X, a2.Z, true).ContainsPoint(point.X, point.Z);
            bool xyinter = new RectangleF(a1.X, a1.Y, a2.X, a2.Y, true).ContainsPoint(point.X, point.Y);
            bool zyinter = new RectangleF(a1.Z, a1.Y, a2.Z, a2.Y, true).ContainsPoint(point.Z, point.Y);
            return xzinter && xyinter && zyinter;
        }
        public virtual void Update(GameTime time)
        {
        }

        public void UpdateTransformation()
        {
            trans = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y + modelbaserot.Y, rotation.X + modelbaserot.X, rotation.Z + modelbaserot.Z) * Matrix.CreateTranslation(position - origin);
            changed = false;
        }

        public virtual void Draw(Camera cam)
        {
            if (changed)
                UpdateTransformation();
            //model.Draw(trans, cam.View, cam.Projection);
            #region AdvancedDraw 
            if (model != null)
            {
                Matrix[] transform = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transform);
                foreach (ModelMesh m in model.Meshes)
                {
                    foreach (BasicEffect effect in m.Effects)
                    {
                        effect.TextureEnabled = true;
                        effect.EnableDefaultLighting();
                        effect.World = transform[m.ParentBone.Index] * trans;
                        effect.View = cam.View;
                        effect.Projection = cam.Projection;
                    }
                    m.Draw();
                }
            }
            #endregion
        }
    }
}
