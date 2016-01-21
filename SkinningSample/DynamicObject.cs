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
        private Vector3 lastpos, lastrot;
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
                lastpos = Position;
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
                lastrot = rotation;
                TimeStamp = DateTime.Now;
                base.Rotation = value;
            }
        }

        public DateTime TimeStamp { get; private set; }

        public void UndoLastTranslation()
        {
            Position = lastpos;
            Rotation = lastrot;
        }
    }
}
