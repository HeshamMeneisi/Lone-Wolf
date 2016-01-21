using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoneWolf
{
    class BrickWall : Wall
    {
        public BrickWall(Vector3 position, float orientation) : base(Manager.Game.Content.Load<Model>("Models\\wall\\model"), new Vector3(0, 40, 0), position, 80, orientation)
        { }
    }
}
