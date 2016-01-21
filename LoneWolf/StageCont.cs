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
            var model = Manager.Game.Content.Load<Model>("Models\\beanbag\\model");
            var player = new Player(model, Vector3.Zero, new Vector3(20, 0, 20), Vector3.Zero, 20);
            var cam = new OrbitCamera(80);
            float celld = 80, wallw = 16; short cellspr = 10;
            world = new World(cam, new BasicEffect(Manager.Game.GraphicsDevice), new Floor(Manager.Game.GraphicsDevice, (int)(celld * cellspr), (int)(celld * cellspr)));
            world.Add(player);
            //world.Add(new Model3D(wall,new Vector3(celld/2,0,wallw/2)));            
            byte[,,] walls = HelperClasses.OptimizedMazeGenerator.GenerateMaze(cellspr, cellspr);
            for (short x = 0; x <= cellspr; x++)
                for (short z = 0; z <= cellspr; z++)
                {
                    if (x < cellspr && walls[0, x, z] < 0xFF)
                        world.Add(new BrickWall(new Vector3(x * celld + celld / 2, 0, z * celld + wallw / 2), 1));
                    if (z < cellspr && walls[1, x, z] < 0xFF)
                        world.Add(new BrickWall(new Vector3(x * celld + wallw / 2, 0, z * celld + celld / 2), 0));
                }
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
