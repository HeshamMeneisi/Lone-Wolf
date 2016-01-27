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
            if (gameover || won)
            {
                main.Draw(batch);
                sign.Draw(batch);
            }
            Compass.Draw(batch, world.ActiveCam.Rotation.Y);
            batch.End();
        }

        public void HandleEvent(WorldEvent e, bool forcehandle = false)
        {
            // Send to GUI
            // if not handled send to world
            main.HandleEvent(e);
        }
        Random ran = new Random();
        const short cellspr = 30;
        Player player;
        EscapeDrone esc;
        public void OnActivated(params object[] args)
        {
            Manager.GameSettings.MusicVolume = 0.05f;
            SoundManager.StopAllLoops();
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Loop], SoundCategory.Music, true);
            gameover = won = false;
            // Specifications of the world
            short cameradistance = 40;
            // Create world
            world.FloorWidth = cellspr;
            world.FloorHeight = cellspr;
            world.Clear();
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
            player = new Player(new Vector3((cellspr / 2 + d / 4) * Map.Celld, 0, cellspr / 2 * Map.Celld), Vector3.Zero, 0.02f);
            player.Rotation = new Vector3(0, -MathHelper.PiOver2, 0);
            esc = new EscapeDrone(new Vector3((cellspr / 2 - d / 4) * Map.Celld, 0, cellspr / 2 * Map.Celld));
            world.Add(player);
            world.Add(esc);
            player.Position += new Vector3(-300, 0, 0);
            //player.Position = esc.Position;
            /*
            Model testmodel = Manager.Game.Content.Load<Model>("Models\\Fence\\model");
            Object3D obj = new Object3D(testmodel, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero, 2);
            obj.Position = player.Position + new Vector3(50,0,50);
            world.Add(obj);
            */
            new NPCCoordinator(currentmap);
            var coord = NPCCoordinator.GetInstance();
            PopulateMap();
            BuildGUI();
#if !DEBUG
            Manager.Game.IsMouseVisible = false;
            world.ActiveCam.LockMouse = true;
#endif
        }
        const int noenemies = 150;
        const int nocol = 75;

        private void PopulateMap()
        {
            int minp = 10, maxp = 20;
            Factory<INPC> enfac = SuperFactory.GetFactory<INPC>();
            Factory<Collectible> clfac = SuperFactory.GetFactory<Collectible>();
            var coord = NPCCoordinator.GetInstance();
            coord.Reset();
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
        bool gameover = false;
        public void ShowGameOver()
        {
            sign = new UICell(DataHandler.UIObjectsTextureMap[UIObjectType.GameOver], "");
            sign.setSizeRelative(0.4f, Orientation.Portrait);
            sign.Position = new Vector2((Screen.Width - sign.Width) / 2, Screen.Height / 2);
            gameover = true;
            Manager.Game.IsMouseVisible = true;
            world.ActiveCam.LockMouse = false;
        }
        public void ShowYouWon()
        {
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Flyaway], SoundCategory.SFX);
            sign = new UICell(DataHandler.UIObjectsTextureMap[UIObjectType.YouWon], "");
            sign.setSizeRelative(0.4f, Orientation.Portrait);
            sign.Position = new Vector2((Screen.Width - sign.Width) / 2, Screen.Height / 2);
            won = true;
            Manager.Game.IsMouseVisible = true;
            world.Destroy(player);
            esc.Path = new NodedPath(new Vector3[]
            {
                esc.Position+ new Vector3(10000,200,10000),
                esc.Position + new Vector3(0,200,0),
                esc.Position
            });
            NPCCoordinator.GetInstance().Register(esc);
            world.ActiveCam.LockMouse = false;
        }
        public void Update(GameTime time)
        {
            world.Update(time);
            scoretext.Text = Manager.UserData.GameState.Score.ToString();
            healthbar.Progress = (float)Manager.UserData.GameState.Health / Player.MaxHealth;
            scoretext.Position = new Vector2(Screen.Width - scoretext.Width, 0);
            NPCCoordinator.GetInstance().UpdateEnemies(time);
            main.Update(time);
        }

        internal static IState GetInstance()
        {
            if (instance != null) return instance;
            return new StageCont();
        }
        #region GUI
        UITextField scoretext;
        UIBar healthbar;
        UIButton main;
        UICell sign;
        Texture2D hbbg = Manager.Game.Content.Load<Texture2D>("Textures\\Ancient\\HBBG");
        Texture2D hb = Manager.Game.Content.Load<Texture2D>("Textures\\Ancient\\HB");
        private bool won;

        private void BuildGUI()
        {
            scoretext = new UITextField(16, Color.White, Color.Black);
            scoretext.Selectable = false;
            scoretext.Text = Manager.UserData.GameState.Score.ToString();
            scoretext.Size = new Vector2(200, 50);
            healthbar = new UIBar(200, 50, hbbg, hb);
            main = new UIButton(DataHandler.UIObjectsTextureMap[UIObjectType.Main], (s) => Manager.StateManager.SwitchTo(GameState.MainMenu));
            main.setSizeRelative(0.2f, Orientation.Portrait);
            main.Position = new Vector2((Screen.Width - main.Width) / 2, Screen.Height / 2 + Screen.Height / 4);
        }
        #endregion

    }
}
