using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LoneWolf
{
    internal class Map
    {
        public static float Celld = Wall.WallHighAnchor.Z - Wall.WallLowAnchor.Z;
        byte[,,] walls;
        Node pathroot;
        public Map(short cellspr, short cellspc, Rectangle rect)
        {
            walls = HelperClasses.OptimizedMazeGenerator.GenerateMaze(cellspr, cellspr, ref pathroot, new Rectangle[] { rect });
            for (int x = rect.X; x < rect.X + rect.Width; x++)
                for (int z = rect.Y + 1; z < rect.Y + rect.Height; z++)
                    walls[0, x, z] = 0xFF;
            for (int x = rect.X + 1; x < rect.X + rect.Width; x++)
                for (int z = rect.Y; z < rect.Y + rect.Height; z++)
                    walls[1, x, z] = 0xFF;
            walls[0, rect.X + (new Random()).Next(0, rect.Width - 1), rect.Y] = 0xFF;
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
    }
}