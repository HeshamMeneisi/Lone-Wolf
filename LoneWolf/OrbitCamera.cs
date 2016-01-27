using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace LoneWolf
{
    class OrbitCamera : Camera
    {
        float r, lon = MathHelper.Pi, lat = MathHelper.PiOver4;
        float mousescale = 0.02f;
        float topcone = 0.5f;
        float bottomcone = 1;
        public override Vector3 Rotation
        {
            get
            {
                return new Vector3(lat * (float)Math.Sin(lon), lon, lat * (float)Math.Cos(lon));
            }
        }
        public override Vector3 Position
        {
            get { return camPosition; }
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
        bool supressmouse = false;
        private void mmoved(Vector2 position, Vector2 offset)
        {
            if (supressmouse) { supressmouse = false; return; }
            float llat = lat;
            lon += offset.X * mousescale;
            lat += offset.Y * mousescale;            
            if (lat > MathHelper.PiOver2 - topcone || lat < -MathHelper.PiOver2 + bottomcone)
                lat = llat;
            UpdatePosToOrbit();
            supressmouse = true;
        }

        public override void Update(GameTime time)
        {
        }

        public void UpdatePosToOrbit()
        {
            float z = (float)(r * Math.Cos(lat) * Math.Cos(lon));
            float x = (float)(r * Math.Cos(lat) * Math.Sin(lon));
            float y = (float)(r * Math.Sin(lat));
            Vector3 newpos = LookAt + new Vector3(x, y, z);
            base.Position = newpos;
        }
    }
}