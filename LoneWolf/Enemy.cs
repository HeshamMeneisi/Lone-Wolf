using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    interface Enemy : WorldElement, IObstacle
    {
        float Velocity { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        NodedPath Path { get; set; }
        bool IsIdle { get; set; }

        void StopWalking(GameTime time);
        void StartWalking();
        TimeSpan GetIdleTime(GameTime time);
    }
}
