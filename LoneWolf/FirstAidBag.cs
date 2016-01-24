using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class FirstAidBag : Collectible
    {
        static Model Model = Manager.Game.Content.Load<Model>("Models\\FirstAid\\model");
        static Vector3 BoxLowAnchor = new Vector3(-10, 0, -8);
        static Vector3 BoxHighAnchor = new Vector3(10, 8, 8);
        static int healamout = 25;
        public FirstAidBag(Vector3 position) : base(Model, Vector3.Zero, Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 0.1f, position)
        {
            // Randomize rotation
            rotation = new Vector3(0, (float)((new Random()).NextDouble() * Math.PI), 0);
        }

        public override void Interact(Player player)
        {
            player.Heal(healamout);
            World.GetInstance().Destroy(this);
        }
    }
}
