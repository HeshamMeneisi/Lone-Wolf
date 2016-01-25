using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class StarBox : Collectible
    {
        static Model Model = Manager.Game.Content.Load<Model>("Models\\StarBox\\model");
        static Vector3 BoxLowAnchor = new Vector3(-10, 0, -10);
        static Vector3 BoxHighAnchor = new Vector3(10, 10, 10);
        static uint scoreincrement = 100;
        public StarBox(Vector3 position) : base(Model, Vector3.Zero, Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 0.001f, position)
        {
            // Randomize rotation
            rotation = new Vector3(0, (float)((new Random()).NextDouble() * Math.PI), 0);
        }

        public override void Interact(Player player)
        {
            Manager.UserData.GameState.Score += scoreincrement;
        }
    }
}
