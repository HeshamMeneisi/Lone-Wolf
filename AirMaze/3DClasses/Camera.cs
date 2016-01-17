using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LoneWolf.Extra
{
    class Camera:GameComponent
    {
        //Attributes
        private Vector3 camPosition;
        private Vector3 camRotation;
        private Vector3 camLookAt;
        private float camSpeed;
        private Vector3 mouseRotationBuffer;
        private MouseState mouseState;
        private MouseState previousState;
        //Properties
        public Matrix Projection { get; protected set; }
        public Matrix View { get { return Matrix.CreateLookAt(camPosition, camLookAt, Vector3.Up); } }
        public Vector3 Position
        {
            get
            {
                return camPosition;
            }
            set
            {
                camPosition = value;
                UpdateLookAt();
            }
        }
        public Vector3 Rotation
        {
            get { return camRotation; }
            set
            {
                camRotation = value;
                UpdateLookAt();
            }
        }
        //Constructor
        public Camera(Game game,Vector3 position,Vector3 rotation,float speed)
            : base(game)
        {
            camSpeed = speed;
            //Create perspective projection cuz we live in a perspective world
            Projection= Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f,
                1000f);
            //Set the camera position and Rotation
            MoveTo(position, rotation);
            // Setting previous mouse state
            previousState = Mouse.GetState();
        }

        //Setting camera position and rotation
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }
        //Motion simulation
        private Vector3 PreviewMove(Vector3 amount)
        {
            Matrix rotate = Matrix.CreateRotationY(camRotation.Y);
            //movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            return camPosition + movement;
        }
        //movin the camera
        private void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);

        }
        //Update LookAt Matrix
        private void UpdateLookAt()
        {
            //Build a rotation matrix
            //simply just adding camera's rotations to prevent the game from crashing
            Matrix roatationMatrix = Matrix.CreateRotationX(camRotation.X) + Matrix.CreateRotationY(camRotation.Y);
            //Build a lookAt Offset
            Vector3 LookAtOffset = Vector3.Transform(Vector3.UnitZ,roatationMatrix);
            //Update cameras
            camLookAt = camPosition + LookAtOffset;
        }
        public override void Update(GameTime gameTime)
        {
            //For smooth mouse movement and kinda stuff xD
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            mouseState = Mouse.GetState();
            //key movement
            KeyboardState ks = Keyboard.GetState();
            Vector3 moveVector = Vector3.Zero;
            if (ks.IsKeyDown(Keys.W))
                moveVector.Z = 1;
            if (ks.IsKeyDown(Keys.S))
                moveVector.Z = -1;
            if (ks.IsKeyDown(Keys.D))
                moveVector.X = -1;
            if (ks.IsKeyDown(Keys.A))
                moveVector.X = 1;
            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= dt * camSpeed;
                Move(moveVector);
            }
            float deltaX, deltaY;
            if (mouseState != previousState)
            {
                //Cache mouse movement
                deltaX = mouseState.X - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaY = mouseState.Y - (Game.GraphicsDevice.Viewport.Height / 2);
                mouseRotationBuffer.X -= 0.01f * deltaX *dt;
                mouseRotationBuffer.Y -= 0.0f * deltaY *dt;
                if (mouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                Rotation = new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f))
                    , MathHelper.WrapAngle(mouseRotationBuffer.X),0);
                deltaX = 0;
                deltaY = 0;
            }
            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            previousState = mouseState;
            base.Update(gameTime);
        }
    }
}
