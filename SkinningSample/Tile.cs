using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    static class Tile
    {
        static Texture2D tex = Manager.Game.Content.Load<Texture2D>("Textures\\Floor");
        static Texture2D bumptex = Manager.Game.Content.Load<Texture2D>("Textures\\FloorBump");        
        static public float CellDim = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;
        static float bumpfactor = 0.005f;
        static Color[] tcolors;
        static float[,] bumpmap;
        static List<VertexBuffer> tilebuffers = new List<VertexBuffer>();
        static bool built = false;
        static public List<VertexBuffer> Buffers
        {
            get
            {
                return tilebuffers;
            }

            set
            {
                tilebuffers = value;
            }
        }

        static public void BuildTile()
        {
            if (built) return;
            tcolors = new Color[tex.Width * tex.Height];
            bumpmap = new float[tex.Width, tex.Height];
            bumptex.GetData(tcolors);
            for (int i = 0; i < tex.Width; i++)
                for (int j = 0; j < tex.Height; j++)
                {
                    var temp = tcolors[i + j * tex.Width];
                    bumpmap[i, j] = bumpfactor * (0.299f * temp.R + 0.587f * temp.G + 0.114f * temp.B);
                }
            tex.GetData(tcolors);            
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            float tw = tex.Width, th = tex.Height;
            //Loop through to create one tile
            for (int x = 0; x < tex.Width - 1; x++)
            {
                for (int z = 0; z < tex.Height - 1; z++)
                {
                    vertexList.AddRange(CreateTileCell(x, z));
                }

            }
            //create buffers
            int start = 0;
            while (start < vertexList.Count)
            {
                ushort count = (ushort)Math.Min(short.MaxValue - 1, vertexList.Count - start);
                var floorBuffer = new VertexBuffer(Manager.Game.GraphicsDevice, VertexPositionColor.VertexDeclaration, count, BufferUsage.None);
                floorBuffer.SetData<VertexPositionColor>(vertexList.GetRange(start, count).ToArray());
                start += count;
                tilebuffers.Add(floorBuffer);
            }
            built = true;
        }

        static public void Destory()
        {
            if (!built) return;
            built = false;
            tilebuffers.Clear();
        }
        //Define a single tile in the floor
        static private List<VertexPositionColor> CreateTileCell(int x, int z)
        {
            float scale = (CellDim + 1) / tex.Width;
            List<VertexPositionColor> FT = new List<VertexPositionColor>();
            FT.Add(new VertexPositionColor(new Vector3(x * scale, bumpmap[x, z], z * scale), tcolors[x + z * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3((x + 1) * scale, bumpmap[x + 1, z], z * scale), tcolors[x + 1 + z * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3(x * scale, bumpmap[x, z + 1], (z + 1) * scale), tcolors[x + (z + 1) * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3((x + 1) * scale, bumpmap[x + 1, z], z * scale), tcolors[x + 1 + z * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3((x + 1) * scale, bumpmap[x + 1, z + 1], (z + 1) * scale), tcolors[x + 1 + (z + 1) * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3(x * scale, bumpmap[x, z + 1], (z + 1) * scale), tcolors[x + (z + 1) * tex.Width]));
            return FT;
        }
    }
}
