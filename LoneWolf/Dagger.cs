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
        static Vector3 LowAnchor = new Vector3(-5, -1, -5);
        static Vector3 HighAnchor = new Vector3(5, 1, 5);
        public Dagger(Vector3 position, Vector3 direction) : base(DaggerModel, position, direction, 4, new Vector3(0, -30, 0), Vector3.Zero, LowAnchor, HighAnchor, 0.15f)
        {
        }
        public override void Collide(Object3D obj)
        {
            if (obj is Enemy)
            {
                ((Enemy)obj).Kill();
                Manager.UserData.GameState.Score += ((Enemy)obj).ScoreIncrement;
            }
            if (!(obj is Player))
                base.Collide(obj);
        }
        public override void Update(GameTime time)
        {
            Rotation += new Vector3(0, 0.1f, 0);
            base.Update(time);
        }
    }
}
