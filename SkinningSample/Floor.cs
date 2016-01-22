using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LoneWolf
{
    class Floor
    {
        private int floorWidth;
        private int floorHeight;
        private BasicEffect effect;

        Tile[,] tiles;

        //Constructor
        public Floor(int width, int height)
        {
            floorWidth = width;
            floorHeight = height;
            effect = new BasicEffect(Manager.Game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.VertexColorEnabled = true;
            tiles = new Tile[10, 10];
            for (int x = 0; x < floorWidth; x++)
                for (int z = 0; z < floorHeight; z++)
                    tiles[x, z] = new Tile(x, z);

        }
        float celld = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;
        public void Draw(Camera cam)
        {
            effect.View = cam.View;
            effect.Projection = cam.Projection;
            int fx, lx, fz = 0, lz = 0;
            int ctd = (int)Math.Ceiling(cam.FarClip / celld);
            int ccx = (int)(cam.Position.X / celld);
            int ccz = (int)(cam.Position.Z / celld);
            for (int x = fx = Math.Max(0, ccx - ctd); x < (lx = Math.Min(floorWidth, ccx + ctd)); x++)
                for (int z = fz = Math.Max(0, ccz - ctd); z < (lz = Math.Min(floorHeight, ccz + ctd)); z++)
                {
                    if (!tiles[x, z].Built) tiles[x, z].BuildTile();
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        foreach (VertexBuffer buffer in tiles[x, z].Buffers)
                        {
                            Manager.Game.GraphicsDevice.SetVertexBuffer(buffer);
                            Manager.Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, buffer.VertexCount / 3);
                        }
                    }
                }
        }
        TimeSpan lgameitme = new TimeSpan(0);
        public void Update(GameTime time)
        {
            if (time.TotalGameTime.Subtract(lgameitme).TotalSeconds > 4)
            {
                int fx, lx, fz = 0, lz = 0;
                int ctd = 5;//(int)Math.Ceiling(cam.FarClip / celld);
                int ccx = (int)(World.GetInstance().ActiveCam.Position.X / celld);
                int ccz = (int)(World.GetInstance().ActiveCam.Position.Z / celld);
                /*fx = Math.Max(0, ccx - ctd); lx = Math.Min(floorWidth, ccx + ctd);
                fz = Math.Max(0, ccz - ctd); lz = Math.Min(floorHeight, ccz + ctd);*/
                for (int x = fx = Math.Max(0, ccx - ctd); x < (lx = Math.Min(floorWidth, ccx + ctd)); x++)
                    for (int z = fz = Math.Max(0, ccz - ctd); z < (lz = Math.Min(floorHeight, ccz + ctd)); z++)
                        if (!tiles[x, z].Built) tiles[x, z].BuildTile();
                foreach (Tile t in Tile.BuiltTiles)
                {
                    if (t.CX < fx || t.CZ < fz || t.CX > lx || t.CZ > lz)
                        t.Destory();
                }
            }
        }
    }
}
