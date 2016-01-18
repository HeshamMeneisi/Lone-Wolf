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
            var model = Manager.Parent.Content.Load<Model>("ManCatMotion");
            var wall = Manager.Parent.Content.Load<Model>("Wall");
            var cam = new Camera(Manager.Parent, new Vector3(10, 0, 10), Vector3.Zero, 10f);
            world = new World(model, Manager.Parent.Graphics, cam, new BasicEffect(Manager.Parent.GraphicsDevice));
            Model3D w = new Model3D(wall, new Vector3(20, 0, 20));
            world.Add(w);
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
