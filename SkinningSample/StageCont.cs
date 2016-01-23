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
        static float celld = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;

        private World world;
        private StageCont()
        {
            //TODO: init
            world = World.GetInstance();
            instance = this;
        }
        public void Draw(SpriteBatch batch)
        {
            //Sky
            Sky.Draw(batch);
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
        public void OnActivated(params object[] args)
        {
            var player = new Player(new Vector3(50, 0, 50), Vector3.Zero, 0.5f);
            // Specifications of the world            
            short cellspr = 30;
            short cameradistance = 40;
            // Create world
            world.FloorWidth = 30;
            world.FloorHeight = 30;
            world.ActiveCam = new OrbitCamera(cameradistance);
            world.CreateTerrain();
            world.Add(player);
            #region TestCode
            //world.Add(new StarBox(new Vector3(50, 0, 100)));
            //world.Add(new FirstAidBag(new Vector3(50, 0, 100)));
            /*Model testmodel = Manager.Game.Content.Load<Model>("Models\\Test\\model");
            Model3D obj = new Model3D(testmodel, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, 0.1f);
            obj.Position = new Vector3(50, 0, 100);
            world.Add(obj);*/
            //world.Add(new BrickWall(new Vector3(-Wall.WallLowAnchor.X, 0, -Wall.WallLowAnchor.Z), 0));
            //world.Add(new BrickWall(new Vector3(-Wall.WallLowAnchor.Z, 0, -Wall.WallLowAnchor.X), 1));    
            #endregion
            byte[,,] walls = HelperClasses.OptimizedMazeGenerator.GenerateMaze(cellspr, cellspr);
            BuildMaze(walls);
        }

        private void BuildMaze(byte[,,] walls)
        {
            int cellspr = walls.GetLength(1);
            int cellspc = walls.GetLength(2);
            for (short x = 0; x < cellspr; x++)
                for (short z = 0; z < walls.GetLength(2); z++)
                {
                    if (x < cellspr && walls[0, x, z] < 0xFF)
                        world.Add(new BrickWall(new Vector3(x * celld - Wall.WallLowAnchor.Z, 0, z * celld - Wall.WallLowAnchor.X), 1));
                    if (z < cellspc && walls[1, x, z] < 0xFF)
                        world.Add(new BrickWall(new Vector3(x * celld - Wall.WallLowAnchor.X, 0, z * celld - Wall.WallLowAnchor.Z), 0));
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
