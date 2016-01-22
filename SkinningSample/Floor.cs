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

        //Constructor
        public Floor(int width, int height)
        {
            floorWidth = width;
            floorHeight = height;
            effect = new BasicEffect(Manager.Game.GraphicsDevice);
            effect.VertexColorEnabled = true;
            Tile.BuildTile();
        }
        float celld = Tile.CellDim;
        public void Draw(Camera cam)
        {
            effect.View = cam.View;
            effect.Projection = cam.Projection;            
            int ctd = (int)Math.Ceiling(cam.FarClip / celld);
            int ccx = (int)(cam.Position.X / celld);
            int ccz = (int)(cam.Position.Z / celld);
            for (int x = Math.Max(0, ccx - ctd); x < Math.Min(floorWidth, ccx + ctd); x++)
                for (int z = Math.Max(0, ccz - ctd); z < Math.Min(floorHeight, ccz + ctd); z++)
                {
                    effect.World = Matrix.CreateTranslation(x*celld, 0, z*celld);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        foreach (VertexBuffer buffer in Tile.Buffers)
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
        }
    }
}
