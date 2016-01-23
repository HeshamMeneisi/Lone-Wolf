using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LoneWolf
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Game instance = null;

        public delegate void OnDrawEventHandler(OnDrawEventArgs e);
        public event OnDrawEventHandler OnDraw;
        public delegate void OnUpdateEventHandler(OnUpdateEventArgs e);
        public event OnUpdateEventHandler OnUpdate;
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return graphics;
            }
            private set { graphics = value; }
        }

        public Game()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content = new SmartContentManager(Content.ServiceProvider);
            Content.RootDirectory = "Content";
            instance = this;
        }

        public static Game GetInstance()
        {
            return instance;
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Screen.SetUp(Window, Graphics);
#if !DEBUG
            Screen.SetFullScreen(true);
#endif
#if WINDOWS || WINDOWS_UAP
            IsMouseVisible = true;
#endif
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Manager.init();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            OnUpdate(new OnUpdateEventArgs(gameTime));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            OnDraw(new OnDrawEventArgs(spriteBatch));          
            base.Draw(gameTime);
        }
    }
}
