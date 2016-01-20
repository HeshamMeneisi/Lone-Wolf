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
        private float nearclip = 0.5f;
        private float farclip = 1000;
        private float fov = MathHelper.PiOver4;
        private Matrix viewmat;
        private Matrix projmat;

        //Properties
        public Matrix Projection { get { return projmat; } }
        public Matrix View { get { return viewmat; } }
        public virtual Vector3 Position
        {
            get
            {
                return camPosition;
            }
            set
            {
                camPosition = value;
                UpdateViewMatrix();
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
                UpdateViewMatrix();
            }
        }

        public float NearClip
        {
            get
            {
                return nearclip;
            }

            set
            {
                nearclip = value;
                UpdateProjMatrix();
            }
        }

        public float FarClip
        {
            get
            {
                return farclip;
            }

            set
            {
                farclip = value;
                UpdateProjMatrix();
            }
        }

        public float Fov
        {
            get
            {
                return fov;
            }

            set
            {
                fov = value;
                UpdateProjMatrix();
            }
        }

        //Constructor
        public Camera(Vector3 position = default(Vector3), Vector3 rotation = default(Vector3))
        {
            //Create perspective projection cuz we live in a perspective world
            UpdateProjMatrix();
            //Set the camera position and Rotation
            MoveTo(position, rotation);
        }

        private void UpdateProjMatrix()
        {
            projmat = Matrix.CreatePerspectiveFieldOfView(fov,
                Manager.Game.GraphicsDevice.Viewport.AspectRatio,
                nearclip,
                farclip);
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
        //Update LookAt Matrix
        private void UpdateViewMatrix()
        {
            viewmat = Matrix.CreateLookAt(camPosition, camLookAt, Vector3.Up);
        }
    }
}
