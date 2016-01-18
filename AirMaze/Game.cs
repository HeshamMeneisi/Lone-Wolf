using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LoneWolf.Extra;
using LoneWolf;

namespace LoneWolf
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera cam;
        Model model;
        Model wall;
        World world;
        static Game instance = null;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
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
            cam = new Camera(this, new Vector3(20, 0, 20), Vector3.Zero, 5f);
            Components.Add(cam);
            Screen.SetUp(Window, graphics);
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
            model = Content.Load<Model>("ManCatMotion");
            wall= Content.Load<Model>("Wall");
            cam = new Camera(this, new Vector3(10, 0, 10), Vector3.Zero, 10f);
            world = new World(model, graphics, cam, new BasicEffect(GraphicsDevice));
            Model3D w = new Model3D(wall, Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                graphics.GraphicsDevice.Viewport.AspectRatio, 0.001f, 1000f), new Vector3(20, 0, 20));
            world.Add(w);
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
            Manager.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Manager.Draw(spriteBatch);
            spriteBatch.End();
            world.Draw();
            base.Draw(gameTime);
        }
    }
}
