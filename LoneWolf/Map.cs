using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoneWolf
{
    internal class Map
    {
        public static float Celld = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;
        byte[,,] walls;
        Node pathroot;
        public Map(short cellspr, short cellspc, Rectangle rect)
        {
            int x, z;
            walls = HelperClasses.OptimizedMazeGenerator.GenerateMaze(cellspr, cellspr, ref pathroot, new Rectangle[] { rect });
            Width = walls.GetLength(1) - 1;
            Height = walls.GetLength(2) - 1;
            for (x = rect.X; x < rect.X + rect.Width; x++)
                for (z = rect.Y + 1; z < rect.Y + rect.Height; z++)
                    walls[0, x, z] = 0xFF;
            for (x = rect.X + 1; x < rect.X + rect.Width; x++)
                for (z = rect.Y; z < rect.Y + rect.Height; z++)
                    walls[1, x, z] = 0xFF;
            x = rect.X + rect.Height / 2;
            for (z = rect.Y; z < rect.Y + rect.Height; z++)
                walls[1, x, z] = Fence.Code;
            var ran = new Random();
            walls[1, rect.X, rect.Y + ran.Next(0, rect.Width - 1)] = 0xFF;
            walls[1, rect.X + rect.Width, rect.Y + ran.Next(0, rect.Width - 1)] = 0xFF;
        }

        public byte[,,] Walls
        {
            get
            {
                return walls;
            }
        }

        public Node Path
        {
            get
            {
                return pathroot;
            }
        }

        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public void BuildMaze()
        {
            World world = World.GetInstance();
            int cellspr = walls.GetLength(1) - 1;
            int cellspc = walls.GetLength(2) - 1;
            WallFactory wallfactory = (WallFactory)SuperFactory.GetFactory<Wall>();
            for (short x = 0; x < Walls.GetLength(1); x++)
                for (short z = 0; z < walls.GetLength(2); z++)
                {
                    if (x < cellspr && walls[0, x, z] < 0xFF)
                        world.Add(wallfactory.CreateNew(walls[0, x, z], new Vector3(x * Celld - Wall.WallLowAnchor.Z, 0, z * Celld - Wall.WallLowAnchor.X), 1));
                    if (z < cellspc && walls[1, x, z] < 0xFF)
                        world.Add(wallfactory.CreateNew(walls[1, x, z], new Vector3(x * Celld - Wall.WallLowAnchor.X, 0, z * Celld - Wall.WallLowAnchor.Z), 0));
                }
        }
        Texture2D minmap;
        public Texture2D MiniMap
        {
            get
            {
                if (minmap != null)
                    return minmap;
                var dev = Manager.Game.GraphicsDevice;
                var spriteBatch = Manager.Game.SpriteBatch;
                Texture2D line = new Texture2D(dev, 1, 1);
                Color[] colors = new Color[] { Color.Brown };
                line.SetData<Color>(colors);
                RenderTarget2D target = new RenderTarget2D(dev, Width * 10 + 4, Height * 10 + 4);
                dev.SetRenderTarget(target);
                dev.Clear(Color.LightGoldenrodYellow);
                int cellspr = walls.GetLength(1) - 1;
                int cellspc = walls.GetLength(2) - 1;
                for (short x = 0; x < Walls.GetLength(1); x++)
                    for (short z = 0; z < walls.GetLength(2); z++)
                    {
                        if (x < cellspr && walls[0, x, z] < 0xFF)
                            spriteBatch.Draw(line, new Rectangle(x * 10, z * 10, 10, 3), Color.White);
                        if (z < cellspc && walls[1, x, z] < 0xFF)
                            spriteBatch.Draw(line, new Rectangle(x * 10, z * 10, 3, 10), Color.White);
                    }
                spriteBatch.End();
                dev.SetRenderTarget(null);
                minmap = target;                
                spriteBatch.Begin();
                return target;
            }
        }
    }
}