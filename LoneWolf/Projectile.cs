using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LoneWolf
{
    class Projectile : Object3D, IObstacle
    {
        Vector3 direction;
        private float velocity;

        public Projectile(Model m, Vector3 initialposition, Vector3 direction, float velocity, Vector3 origin, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale) : base(m, origin, baserot, lowanchor, highanchor, scale)
        {
            Position = initialposition;
            this.direction = direction;            
            this.velocity = velocity;            
        }
        public virtual void Collide(Object3D obj)
        {
            World.GetInstance().Destroy(this);         
        }
        public override void Update(GameTime time)
        {
            Vector3 velvec = direction * velocity;
            Position += velvec;
            base.Update(time);
        }
    }
}
