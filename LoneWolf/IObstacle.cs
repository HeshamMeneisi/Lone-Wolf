using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    interface IObstacle
    {
        void Collide(Object3D player);
    }
}
