using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Dagger : Projectile
    {
        static Model DaggerModel = Manager.Game.Content.Load<Model>("Models\\Dagger\\model");
        static Vector3 BoxLowAnchor = new Vector3(-5, -1, -5);
        static Vector3 BoxHighAnchor = new Vector3(5, 1, 5);
        public Dagger(Vector3 position, Vector3 direction) : base(DaggerModel, position, direction, 4, new Vector3(0, -30, 0), Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 0.15f)
        {
        }
        public override void Collide(Object3D obj)
        {
            if (obj is INPC)
            {                
                ((INPC)obj).Kill();             
            }
            if (!(obj is Player))
                base.Collide(obj);
        }
        public override void Update(GameTime time)
        {
            Rotation += new Vector3(0, 0.2f, 0);
            base.Update(time);
        }
    }
}
