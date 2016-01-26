using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LoneWolf
{
    internal static class Manager
    {
        static StateManager stateManager;
        static SmartContentManager contentManager;
        static Settings settings;
        static UserData userdata;
        const int timeout = 5000;

        internal static void StartNewGame()
        {
            userdata.GameState = new GameStateHolder();
            Manager.UserData.GameState.Health = Player.MaxHealth;
            stateManager.SwitchTo(GameState.OnStage);
#if !DEBUG
            Game.IsMouseVisible = false;
#endif
        }

        static bool initd = false;
        const string settingsfile = "GameSettings.xml";
        const string datafile = "UserData.xml";
        internal static void SaveSettings()
        {
            DataHandler.SaveData<Settings>(settings, settingsfile);
        }
        internal static void LoadSettings()
        {
            Settings temp = DataHandler.LoadData<Settings>(settingsfile);
            if (temp != null) GameSettings = temp;
            else settings = new Settings();
        }
        static bool syncingdata = false, connected = false;
        internal static bool IsIdle { get { return !syncingdata; } }

        internal static void SaveUserDataLocal()
        {
            userdata.UpdateRawData();
            userdata.EncryptStrings();
            DataHandler.SaveData<UserData>(userdata, datafile);
        }

        internal static Task<Exception> LoadUserDataLocal()
        {
            if (!IsIdle) return null;
            UserData temp = DataHandler.LoadData<UserData>(datafile);
            if (temp != null) userdata = temp;
            else userdata = new UserData();
            userdata.LoadRawData();
            return null;
        }

        internal static void GameOver()
        {
            ((StageCont)StageCont.GetInstance()).ShowGameOver();
        }

        internal static StateManager StateManager { get { return stateManager; } }
        internal static SmartContentManager RandomAccessContentManager { get { return contentManager; } }
        internal static Settings GameSettings { get { return settings; } set { settings = value; } }
        //internal static EncryptionProvider Cipher { get { return crypto; } set { crypto = value; } }
        internal static Game Game { get { return Game.GetInstance(); } }
        internal static UserData UserData { get { return userdata; } set { userdata = value; } }
        internal static bool Connected { get { return connected; } }
        internal static void init()
        {            
            LoadUserDataLocal();            
            contentManager = Game.Content as SmartContentManager;
            LoadSettings();
            DataHandler.LoadCurrentTheme();
            stateManager = new StateManager();

            stateManager.AddGameState(GameState.MainMenu, MainMenuCont.GetInstance());
            stateManager.AddGameState(GameState.OnStage, StageCont.GetInstance());

            initInput();

            stateManager.SwitchTo(GameState.MainMenu);

            Game.OnUpdate += Update;
            Game.OnDraw += Draw;
            initd = true;
        }

        internal static void HandleEvent(WorldEvent e)
        {
            if (!initd) return;
            // Debugging commands
            if (Debugger.IsAttached && e is KeyDownEvent)
            {
                if ((e as KeyDownEvent).Key == Keys.OemTilde)
                {
                    // Could open a console here later
                }
            }            
            stateManager.CurrentGameState.HandleEvent(e);
        }
        internal static void Draw(OnDrawEventArgs e)
        {
            if (!initd) return;
            stateManager.Draw(e.Spritebatch);            
        }
        internal static void Update(OnUpdateEventArgs e)
        {
            if (!initd) return;
            stateManager.Update(e.Gametime);
            SoundManager.Update(e.Gametime);            
            InputManager.Update(e.Gametime);
        }

        private static void initInput()
        {
            InputManager.init();
            InputManager.KeyDown += keydown;
            InputManager.KeyUp += keyup;
            InputManager.MouseDown += mdown;
            InputManager.MouseUp += mup;
            InputManager.Scrolled += mscrolled;
            InputManager.MouseMoved += mmoved;
            InputManager.Dragged += dragged;
            InputManager.Tapped += tapped;
            InputManager.Pinched += pinched;
            InputManager.DragComplete += drcomplete;
            InputManager.AllFingersOff += afo;
        }

        private static void afo()
        {
            HandleEvent(new TouchAllFingersOffEvent());
        }

        private static void drcomplete(Vector2 position)
        {
            HandleEvent(new TouchDragCompleteEvent(position));
        }

        private static void keyup(Keys k)
        {
            HandleEvent(new KeyUpEvent(k));
        }

        private static void mdown(InputManager.MouseKey k, Vector2 position)
        {
            HandleEvent(new MouseDownEvent(k, position));
        }

        private static void pinched(float delta)
        {
            HandleEvent(new TouchPinchEvent(delta));
        }

        private static void tapped(Vector2 position)
        {
            HandleEvent(new TouchTapEvent(position));
        }

        private static void dragged(Vector2 delta, Vector2 pos)
        {
            HandleEvent(new TouchFreeDragEvent(delta, pos));
        }

        private static void mmoved(Vector2 position, Vector2 offset)
        {
            HandleEvent(new MouseMovedEvent(position, offset));
        }

        private static void mscrolled(int value)
        {
            HandleEvent(new MouseScrollEvent(value));
        }

        private static void mup(InputManager.MouseKey k, Vector2 pos)
        {
            HandleEvent(new MouseUpEvent(k, pos));
        }

        private static void keydown(Keys k)
        {
            HandleEvent(new KeyDownEvent(k));
        }
    }
}
