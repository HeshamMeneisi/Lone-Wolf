using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    interface WorldElement
    {
        bool Destroyed { get; set; }

        void Draw(Camera cam);
        void Update(GameTime time);
        float DistanceTo(Vector3 position);
        bool Contains(Vector3 v);
    }
}
