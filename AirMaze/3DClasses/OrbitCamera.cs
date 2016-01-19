using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace LoneWolf
{
    class OrbitCamera : Camera
    {
        float r, lon = MathHelper.Pi, lat = MathHelper.PiOver4;
        float mousescale = 0.02f;
        float topcone = 0.2f;
        float bottomcone = 0.5f;
        public override Vector3 Position
        {
            get { return LookAt; }
            set { camLookAt = value; UpdatePosToOrbit(); }
        }
        public float Distance { get { return r; } set { r = value; } }

        public float Longitude
        {
            get
            {
                return lon;
            }

            set
            {
                lon = value;
            }
        }

        public float Latitude
        {
            get
            {
                return lat;
            }

            set
            {
                lat = value;
            }
        }

        public OrbitCamera(float distance)
        {
            r = distance;
            InputManager.MouseMoved += mmoved;
        }

        private void mmoved(Vector2 position, Vector2 offset)
        {
            float llat = lat;
            lon += offset.X * mousescale;
            lat += offset.Y * mousescale;
            Debug.WriteLine(lon + "," + lat);
            if (lat > MathHelper.PiOver2 - topcone || lat < -MathHelper.PiOver2 + bottomcone)
                lat = llat;
            UpdatePosToOrbit();
        }

        public override void Update(GameTime time)
        {
        }

        public void UpdatePosToOrbit()
        {
            float z = (float)(r * Math.Cos(lat) * Math.Cos(lon));
            float x = (float)(r * Math.Cos(lat) * Math.Sin(lon));
            float y = (float)(r * Math.Sin(lat));
            base.Position = LookAt + new Vector3(x, y, z);
        }
    }
}