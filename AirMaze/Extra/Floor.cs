using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AirMaze.Extra
{
    class Floor
    {
        //Attributes
        private int floorWidth;
        private int floorHeight;
        private VertexBuffer floorBuffer;
        private GraphicsDevice device;
        private Color[] floorColors= new Color[2]{Color.White,Color.Black};

        //Constructor
        public Floor(GraphicsDevice GD,int width,int height) {
            this.device = GD;
            this.floorWidth = width;
            this.floorHeight = height;
            BuildFloor();
        }

        //Building floor
        private void BuildFloor()
        {
            List<VertexPositionColor> vertixList = new List<VertexPositionColor>();
            int counter = 0;
            //Loop through to create floor
            for(int x=0; x<floorWidth;x++){
                counter++;
                for (int z = 0; z < floorHeight; z++)
                {
                    counter++;
                    //loop through each vertix
                    foreach (VertexPositionColor vertix in floortile(x,z,floorColors[counter%2]))
                    {
                        vertixList.Add(vertix);
                    }
                }

            }
            //create buffer
            floorBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, vertixList.Count, BufferUsage.None);
            floorBuffer.SetData<VertexPositionColor>(vertixList.ToArray());
        }
     
        //Define a single tile in the floor
        private List<VertexPositionColor> floortile(int xOffset, int zOffset, Color tileColor)
        {
            List<VertexPositionColor> FT = new List<VertexPositionColor>();
            FT.Add(new VertexPositionColor (new Vector3(xOffset, 0, zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(1+xOffset, 0,zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(xOffset, 0,1+ zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(1+xOffset, 0, zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(1+xOffset, 0, 1+zOffset), tileColor));
            FT.Add(new VertexPositionColor(new Vector3(xOffset, 0, 1 + zOffset), tileColor));


            return FT;
        }
        public void Draw(Camera cam, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            effect.View = cam.View;
            effect.Projection = cam.Projection;
            effect.World = Matrix.Identity;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, floorBuffer.VertexCount / 3);
            }
        }
    }
}
