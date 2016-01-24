using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    abstract class Collectible : Model3D, IInteractiveObject
    {
        public Collectible(Model m, Vector3 origin, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale, Vector3 position) : base(m, origin, baserot, lowanchor, highanchor, scale)
        {
            this.position = position;
        }        

        public abstract void Interact(Player player);
    }
}
