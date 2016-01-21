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
            // 3D Rendering
            Manager.Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Manager.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Manager.Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            world.Draw();        
            
            // 2D Rendering
            Manager.Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //batch.begin
            // Draw overlay GUI            
            //bath.end
        }

        public void HandleEvent(WorldEvent e, bool forcehandle = false)
        {
            // Send to GUI
            // if not handled send to world

        }
        World world;
        public void OnActivated(params object[] args)
        {            
            var player = new Player(new Vector3(50, 0, 50), Vector3.Zero, 0.5f);
            var cam = new OrbitCamera(50);
            float celld = 100, wallw = 20; short cellspr = 10;
            world = new World(cam, new BasicEffect(Manager.Game.GraphicsDevice), new Floor((int)(celld * cellspr), (int)(celld * cellspr)));
            world.Add(player);
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
