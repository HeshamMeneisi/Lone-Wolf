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
        const short cellspr = 30;
        public void OnActivated(params object[] args)
        {
            // Specifications of the world                        ;
            short cameradistance = 40;
            // Create world
            world.FloorWidth = cellspr;
            world.FloorHeight = cellspr;
            world.ActiveCam = new OrbitCamera(cameradistance);
            world.CreateTerrain();
            #region TestCode
            //world.Add(new StarBox(new Vector3(50, 0, 100)));
            //world.Add(new FirstAidBag(new Vector3(50, 0, 100)));
            //world.Add(new LandMine(new Vector3(50, 0, 100)));            
            //world.Add(new Dagger(player.Position, player.Rotation));            
            //world.Add(new BrickWall(new Vector3(-Wall.WallLowAnchor.X, 0, -Wall.WallLowAnchor.Z), 0));
            //world.Add(new BrickWall(new Vector3(-Wall.WallLowAnchor.Z, 0, -Wall.WallLowAnchor.X), 1));                
            #endregion
            int d = 6;
            currentmap = new Map(cellspr, cellspr, new Rectangle(cellspr / 2 - d / 2, cellspr / 2 - d / 2, d, d));
            currentmap.BuildMaze();
            Player player = new Player(new Vector3((cellspr / 2 + d / 4) * Map.Celld, 0, cellspr / 2 * Map.Celld), Vector3.Zero, 0.02f);
            player.Rotation = new Vector3(0, -MathHelper.PiOver2, 0);
            world.Add(player);
            /*
            Model testmodel = Manager.Game.Content.Load<Model>("Models\\Fence\\model");
            Object3D obj = new Object3D(testmodel, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, 2);
            obj.Position = player.Position + new Vector3(50,0,50);
            world.Add(obj);
            */
            new EnemyCoordinator(currentmap);
            var coord = EnemyCoordinator.GetInstance();
            PopulateMap();
            BuildGUI();
        }
        const int noenemies = 100;
        const int nocol = 75;

        private void PopulateMap()
        {
            int minp = 10, maxp = 20;
            Factory<Enemy> enfac = SuperFactory.GetFactory<Enemy>();
            Factory<Collectible> clfac = SuperFactory.GetFactory<Collectible>();
            var coord = EnemyCoordinator.GetInstance();
            for (int i = 0; i < noenemies; i++)
            {
                var e = enfac.CreateNew((byte)ran.Next(0, enfac.AvailableTypes), Vector3.Zero, coord.GenerateRandomPath(ran.Next(minp, maxp)));
                coord.Register(e);
                world.Add(e);
            }
            float offset = -Map.Celld / 4;
            byte[,] used = new byte[cellspr, cellspr];
            for (int i = 0; i < nocol; i++)
            {
                int x = ran.Next(0, cellspr);
                int z = ran.Next(0, cellspr);
                int yoff = used[x, z];
                float fx = x * Map.Celld + offset;
                float fz = z * Map.Celld + offset;
                var c = clfac.CreateNew((byte)ran.Next(0, clfac.AvailableTypes), new Vector3(fx, yoff, fz));
                used[x, z] += (byte)(c.HighAnchor.Y - c.LowAnchor.Y);
                world.Add(c);
                // Place mine
                x = ran.Next(0, cellspr);
                z = ran.Next(0, cellspr);
                yoff = used[x, z];
                if (yoff == 0)
                {
                    fx = x * Map.Celld + offset;
                    fz = z * Map.Celld + offset;
                    world.Add(new LandMine(new Vector3(fx, 0, fz)));
                }
            }
        }

        internal void ShowGameOver()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime time)
        {
            world.Update(time);
            scoretext.Text = Manager.UserData.GameState.Score.ToString();
            healthbar.Progress = (float)Manager.UserData.GameState.Health / Player.MaxHealth;
            scoretext.Position = new Vector2(Screen.Width - scoretext.Width, 0);
            EnemyCoordinator.GetInstance().UpdateEnemies(time);
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
