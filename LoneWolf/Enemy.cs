using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Enemy : Model3D
    {
        NodedPath path;
        public Enemy(Model model, Vector3 position, Vector3 origin, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale, NodedPath path, float velocity) : base(model, origin, baserot, lowanchor, highanchor, scale)
        {
            this.position = position;
            this.path = path;
            Velocity = velocity;
        }

        public float Velocity { get; internal set; }

        internal NodedPath Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
    }
}
