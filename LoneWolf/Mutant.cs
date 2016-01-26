using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Mutant : GroundMonster
    {
        public override Model WalkingModel { get { return Manager.Game.Content.Load<Model>("Models\\Mutant\\walking"); } }
        public override Model IdleModel { get { return Manager.Game.Content.Load<Model>("Models\\Mutant\\idle"); } }
        public override Model AttackModel { get { return Manager.Game.Content.Load<Model>("Models\\Mutant\\attack"); } }
        public override Model DeathModel { get { return Manager.Game.Content.Load<Model>("Models\\Mutant\\death"); } }
        public static Vector3 BoxLowAnchor { get { return new Vector3(-15, 0, -15); } }
        public static Vector3 BoxHighAnchor { get { return new Vector3(15, 60, 15); } }
        public override float DefaultVelocity { get { return 0.4f; } }

        public override uint ScoreIncrement
        {
            get
            {
                return 100;
            }
        }

        public Mutant(Vector3 position, NodedPath path) : base(position, path, BoxLowAnchor, BoxHighAnchor)
        {

        }
    }
}
