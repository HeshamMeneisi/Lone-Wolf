using LoneWolf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    class StageCont : IState
    {
        static StageCont instance = null;

        public StageCont()
        {
            //TODO: init
            instance = this;
        }
        public void Draw(SpriteBatch batch)
        {
            world.Draw();
            // Draw overlay GUI            
        }

        public void HandleEvent(WorldEvent e, bool forcehandle = false)
        {
            // Send to GUI
            // if not handled send to world

        }
        World world;
        public void OnActivated(params object[] args)
        {
            var model = Manager.Game.Content.Load<Model>("ManCatMotion");
            var wall = Manager.Game.Content.Load<Model>("Wall");
            var player = new Player(model, new Vector3(1, 0, 1), Vector3.Zero, 1f);
            var wallobj = new Model3D(wall, new Vector3(10, 0, 10));
            var cam = new OrbitCamera(80);
            world = new World(Manager.Game.Graphics, cam, new BasicEffect(Manager.Game.GraphicsDevice));
            world.Add(player);
        }

        public void Update(GameTime time)
        {
            world.Update(time);
        }

        internal static IState GetInstance()
        {
            if (instance != null) return instance;
            return new StageCont();
        }
    }
}
