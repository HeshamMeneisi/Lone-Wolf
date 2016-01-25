using LoneWolf;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class DynamicObject : SkinnedModel3D
    {
        public DynamicObject(Model m, Vector3 origin, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale, string defaultclip, bool cangooffpath) : base(m, origin, baserot, lowanchor, highanchor, scale, defaultclip)
        {
            CanGoOffPath = cangooffpath;
            TimeStamp = DateTime.Now;
        }
        public bool CanGoOffPath { get; private set; }
        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                TimeStamp = DateTime.Now;
                base.Position = value;
            }
        }
        public override Vector3 Rotation
        {
            get
            {
                return base.Rotation;
            }

            set
            {
                TimeStamp = DateTime.Now;
                base.Rotation = value;
            }
        }
        public DateTime TimeStamp { get; private set; }
        public virtual void SeparateFrom(Model3D stc)
        {
            Vector3 ta1 = stc.AbsoluteLowAnchor;
            Vector3 ta2 = stc.AbsoluteHighAnchor;
            Vector3 a1 = AbsoluteLowAnchor;
            Vector3 a2 = AbsoluteHighAnchor;
            RectangleF xzinter =  new RectangleF(ta1.X, ta1.Z, ta2.X, ta2.Z, true).Intersection(new RectangleF(a1.X, a1.Z, a2.X, a2.Z, true));
            RectangleF zyinter = new RectangleF(ta1.Z, ta1.Y, ta2.Z, ta2.Y, true).Intersection(new RectangleF(a1.Z, a1.Y, a2.Z, a2.Y, true));            
            Vector3 lv = Position - stc.Position;
            float xdif = xzinter.Width;
            float zdif = xzinter.Height;
            float ydif = zyinter.Height;
            Vector3 position = Position;
            if (xdif < zdif && xdif < ydif)
                position.X += xdif * lv.X > 0 ? 1 : -1;
            else if (zdif < ydif)
                position.Z += zdif * lv.Z > 0 ? 1 : -1;
            else
                position.Y += ydif;/*Should not go underground*/
            Position = position;
        }
    }
}
