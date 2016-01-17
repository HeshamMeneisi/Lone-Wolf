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
    public class GameClass : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera cam;
        Floor floor;
        BasicEffect e;
        MainModel Mymodel;
        Model model;
        Texture2D tex;

        static GameClass instance = null;

        public GameClass()
        {            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;
        }

        public static GameClass GetInstance()
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
            cam = new Camera(this, new Vector3(10,1,5),Vector3.Zero,5f);
            Components.Add(cam);
            floor = new Floor(GraphicsDevice, 20, 20);
            e = new BasicEffect(GraphicsDevice);
           model = Content.Load<Model>("Used");
           tex = Content.Load<Texture2D>("tut4");
           Mymodel = new MainModel(model,tex);
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
            Manager.init(this);            
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
            Manager.Draw(spriteBatch);
            /*
            floor.Draw(cam, e);
            Mymodel.Draw(cam);
            base.Draw(gameTime);*/
        }
    }
}
