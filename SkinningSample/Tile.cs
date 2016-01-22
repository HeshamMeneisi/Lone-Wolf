using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Tile
    {
        public static LinkedList<Tile> BuiltTiles = new LinkedList<Tile>();
        static LinkedList<Tile> todestroy = new LinkedList<Tile>();
        static Texture2D tex = Manager.Game.Content.Load<Texture2D>("Textures\\Floor");
        static Texture2D bumptex = Manager.Game.Content.Load<Texture2D>("Textures\\FloorBump");
        static bool initialized = false;
        static float CellDim = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;
        static float bumpfactor = 0.005f;
        static Color[] tcolors;
        static float[,] bumpmap;
        List<VertexBuffer> tilebuffers = new List<VertexBuffer>();
        float tx, tz;
        int x, z;
        bool built = false;
        public List<VertexBuffer> Buffers
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

        public bool Built
        {
            get
            {
                return built;
            }
        }

        public float X
        {
            get
            {
                return tx;
            }

            set
            {
                tx = value;
            }
        }

        public float Z
        {
            get
            {
                return tz;
            }

            set
            {
                tz = value;
            }
        }

        public int CX
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int CZ
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        public Tile(int x, int z)
        {
            tx = x * CellDim; tz = z * CellDim;
            this.x = x; this.z = z;
            if (!initialized)
            {
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
                initialized = true;
            }
        }
        bool marked = false;
        internal void MarkForDestruction()
        {
            if (marked) return;
            marked = true;
            todestroy.AddFirst(this);
        }

        public void BuildTile()
        {
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
            if (marked)
            {
                marked = false;
                todestroy.Remove(this);
            }
            BuiltTiles.AddFirst(this);
        }

        internal static void DestroyBending()
        {
            foreach (Tile t in todestroy)
                t.Destory();
            todestroy.Clear();
        }

        public void Destory()
        {
            marked = false;
            if (!built) return;
            built = false;
            tilebuffers.Clear();
            BuiltTiles.Remove(this);
        }
        //Define a single tile in the floor
        private List<VertexPositionColor> CreateTileCell(int x, int z)
        {
            float scale = (CellDim + 1) / tex.Width;
            List<VertexPositionColor> FT = new List<VertexPositionColor>();
            FT.Add(new VertexPositionColor(new Vector3(x * scale + tx, bumpmap[x, z], z * scale + tz), tcolors[x + z * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3((x + 1) * scale + tx, bumpmap[x + 1, z], z * scale + tz), tcolors[x + 1 + z * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3(x * scale + tx, bumpmap[x, z + 1], (z + 1) * scale + tz), tcolors[x + (z + 1) * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3((x + 1) * scale + tx, bumpmap[x + 1, z], z * scale + tz), tcolors[x + 1 + z * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3((x + 1) * scale + tx, bumpmap[x + 1, z + 1], (z + 1) * scale + tz), tcolors[x + 1 + (z + 1) * tex.Width]));
            FT.Add(new VertexPositionColor(new Vector3(x * scale + tx, bumpmap[x, z + 1], (z + 1) * scale + tz), tcolors[x + (z + 1) * tex.Width]));
            return FT;
        }
    }
}
