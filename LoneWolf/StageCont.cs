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

        private Map currentmap;
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
            batch.Begin();
            // Draw overlay GUI            
            scoretext.Draw(batch);
            healthbar.Draw(batch);
            batch.End();
        }

        public void HandleEvent(WorldEvent e, bool forcehandle = false)
        {
            // Send to GUI
            // if not handled send to world

        }
        Random ran = new Random();
        public void OnActivated(params object[] args)
        {
            var player = new Player(new Vector3(50, 0, 50), Vector3.Zero, 0.5f);
            // Specifications of the world            
            short cellspr = 5;
            short cameradistance = 40;
            // Create world
            world.FloorWidth = 5;
            world.FloorHeight = 5;
            world.ActiveCam = new OrbitCamera(cameradistance);
            world.CreateTerrain();
            world.Add(player);
            #region TestCode
            //world.Add(new StarBox(new Vector3(50, 0, 100)));
            //world.Add(new FirstAidBag(new Vector3(50, 0, 100)));
            //world.Add(new LandMine(new Vector3(50, 0, 100)));            
            /*Model testmodel = Manager.Game.Content.Load<Model>("Models\\testmodel\\test");
            Model3D obj = new Model3D(testmodel, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, 1f);
            obj.Position = new Vector3(50, 0, 100);
            world.Add(obj);*/
            //world.Add(new BrickWall(new Vector3(-Wall.WallLowAnchor.X, 0, -Wall.WallLowAnchor.Z), 0));
            //world.Add(new BrickWall(new Vector3(-Wall.WallLowAnchor.Z, 0, -Wall.WallLowAnchor.X), 1));                
            #endregion
            currentmap = new Map(cellspr, cellspr);
            currentmap.BuildMaze();
            new EnemyCoordinator(currentmap);
            var coord = EnemyCoordinator.GetInstance();
            Drone temp;
            world.Add(temp = new Drone(new Vector3(50, 0, 80), coord.GenerateRandomPath(5)));
            coord.Register(temp);
            player.Position = temp.Position;
            BuildGUI();
        }

        public void Update(GameTime time)
        {
            world.Update(time);
            scoretext.Text = Manager.UserData.GameState.Score.ToString();
            healthbar.Progress = (float)Manager.UserData.GameState.Health / Player.MaxHealth;
            scoretext.Position = new Vector2(Screen.Width - scoretext.Width, 0);
            EnemyCoordinator.GetInstance().UpdateEnemies();
        }

        internal static IState GetInstance()
        {
            if (instance != null) return instance;
            return new StageCont();
        }
        #region GUI
        UITextField scoretext;
        UIBar healthbar;
        Texture2D hbbg = Manager.Game.Content.Load<Texture2D>("Textures\\Ancient\\HBBG");
        Texture2D hb = Manager.Game.Content.Load<Texture2D>("Textures\\Ancient\\HB");
        private void BuildGUI()
        {
            scoretext = new UITextField(16, Color.White, Color.Black);
            scoretext.Selectable = false;
            scoretext.Text = Manager.UserData.GameState.Score.ToString();
            scoretext.Size = new Vector2(200, 50);
            healthbar = new UIBar(200, 50, hbbg, hb);
        }
        #endregion

    }
}
