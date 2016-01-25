using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    interface Enemy
    {
        float Velocity { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        NodedPath Path { get; set; }
    }
}
