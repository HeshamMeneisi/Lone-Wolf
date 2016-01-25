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
        public Map(short cellspr, short cellspc)
        {
            walls = HelperClasses.OptimizedMazeGenerator.GenerateMaze(cellspr, cellspr, ref pathroot);
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