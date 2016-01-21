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
        private List<VertexBuffer> floorbuffers = new List<VertexBuffer>();
        private Color[] floorColors = new Color[2] { Color.White, Color.Black };

        //Constructor
        public Floor(int width, int height)
        {
            floorWidth = width;
            floorHeight = height;
            BuildFloor();
        }

        //Building floor
        private void BuildFloor()
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            int counter = 0;
            //Loop through to create floor
            for (int x = 0; x < floorWidth; x++)
            {
                counter++;
                for (int z = 0; z < floorHeight; z++)
                {
                    counter++;
                    //loop through each vertix
                    foreach (VertexPositionColor vertix in floortile(x, z, floorColors[counter % 2]))
                    {
                        vertexList.Add(vertix);
                    }
                }

            }
            //create buffer
            int start = 0;
            while (start < vertexList.Count)
            {
                ushort count = (ushort)Math.Min(short.MaxValue-1, vertexList.Count - start);
                var floorBuffer = new VertexBuffer(Manager.Game.GraphicsDevice, VertexPositionColor.VertexDeclaration, count, BufferUsage.None);
                floorBuffer.SetData<VertexPositionColor>(vertexList.GetRange(start, count).ToArray());
                start += count;
                floorbuffers.Add(floorBuffer);
            }
        }

        //Define a single tile in the floor
        private List<VertexPositionColor> floortile(int xOffset, int zOffset, Color tileColor)
        {
            List<VertexPositionColor> FT = new List<VertexPositionColor>();
            FT.Add(new VertexPositionColor(new Vector3(xOffset, 0, zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(xOffset, 0, 1 + zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 1 + zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(xOffset, 0, 1 + zOffset), tileColor));
            return FT;
        }
        public void Draw(BasicEffect effect, Camera cam)
        {
            effect.VertexColorEnabled = true;
            effect.View = cam.View;
            effect.Projection = cam.Projection;
            effect.World = Matrix.Identity;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (VertexBuffer buffer in floorbuffers)
                {
                    Manager.Game.GraphicsDevice.SetVertexBuffer(buffer);
                    Manager.Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, buffer.VertexCount / 3);
                }
            }
        }
    }
}
