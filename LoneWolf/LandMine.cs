using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class LandMine : Object3D, IInteractiveObject, IObstacle
    {
        static Model Model = Manager.Game.Content.Load<Model>("Models\\LandMine\\model");
        static Vector3 BoxLowAnchor = new Vector3(-5, 0, -5);
        static Vector3 BoxHighAnchor = new Vector3(5, 4, 5);
        static int damage = 25;
        static uint DefuseScore = 50;
        public LandMine(Vector3 position) : base(Model, Vector3.Zero, Vector3.Zero, BoxLowAnchor, BoxHighAnchor, 1)
        {
            Position = position;
        }
        public void Collide(Object3D player)
        {
            if (Destroyed) return;
            World.GetInstance().Destroy(this);
            Triggered(player as Player);
        }

        private void Triggered(Player player)
        {
            if (player != null)
                player.TakeDamage(damage);
        }

        public void Interact(Player player)
        {
            if (Destroyed) return;
            World.GetInstance().Destroy(this);
            Defused(player);
        }

        private void Defused(Player player)
        {
            Manager.UserData.GameState.Score += DefuseScore;
        }
    }
}
