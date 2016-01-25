using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LoneWolf
{
    class Terrain
    {
        static Model plate = Manager.Game.Content.Load<Model>("Models\\Plate\\model");        
        private int floorWidth;
        private int floorHeight;

        //Constructor
        public Terrain(int width, int height)
        {
            floorWidth = width;
            floorHeight = height;
        }
        float celld = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;
        public void Draw(Camera cam)
        {
            int ctd = (int)Math.Ceiling(cam.FarClip / celld);
            int ccx = (int)(cam.Position.X / celld);
            int ccz = (int)(cam.Position.Z / celld);
            var pool = Model3DPool.GetInstance();
            for (int x = Math.Max(0, ccx - ctd); x < Math.Min(floorWidth, ccx + ctd); x++)
                for (int z = Math.Max(0, ccz - ctd); z < Math.Min(floorHeight, ccz + ctd); z++)
                {
                    Model3D model = pool.Acquire();
                    model.Model = plate; model.Position = new Vector3(x * celld, 0, z * celld);
                    model.UpdateTransformation();
                    model.Draw(cam);
                    pool.Release(model);
                }
        }
        TimeSpan lgameitme = new TimeSpan(0);
    }
}
