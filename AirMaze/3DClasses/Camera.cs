using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LoneWolf
{
    class Camera
    {
        //Attributes
        protected Vector3 camPosition;
        protected Vector3 camRotation;
        protected Vector3 camLookAt;
        private Matrix mat;

        //Properties
        public Matrix Projection { get; protected set; }
        public Matrix View { get { return mat; } }
        public virtual Vector3 Position
        {
            get
            {
                return camPosition;
            }
            set
            {
                camPosition = value;
                UpdateMatrix();
            }
        }
        public Vector3 Rotation
        {
            get { return camRotation; }
            set
            {
                camRotation = value;
                UpdateLookat();
            }
        }

        private void UpdateLookat()
        {
            //Build a rotation matrix
            //simply just adding camera's rotations to prevent the game from crashing
            Matrix roatationMatrix = Matrix.CreateRotationX(camRotation.X) + Matrix.CreateRotationY(camRotation.Y);
            //Build a lookAt Offset
            Vector3 LookAtOffset = Vector3.Transform(Vector3.UnitZ, roatationMatrix);
            //Update cameras
            LookAt = camPosition + LookAtOffset;
        }

        public Vector3 LookAt
        {
            get
            {
                return camLookAt;
            }

            set
            {
                camLookAt = value;
                UpdateMatrix();
            }
        }

        //Constructor
        public Camera(Vector3 position = default(Vector3), Vector3 rotation = default(Vector3))
        {
            //Create perspective projection cuz we live in a perspective world
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                Manager.Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f,
                1000f);
            //Set the camera position and Rotation
            MoveTo(position, rotation);
        }

        public virtual void Update(GameTime time)
        {

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
        private void UpdateMatrix()
        {
            mat = Matrix.CreateLookAt(camPosition, camLookAt, Vector3.Up);
        }
    }
}
