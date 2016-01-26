using LoneWolf;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace HelperClasses
{
    static class OptimizedMazeGenerator
    {
        static Random ran;
        static bool[,] visited;
        static byte[,,] walls;
        /// <summary>
        /// Generate a random maze using the given width and height cell count and the starting position cell and ignored rectangles.
        /// </summary>
        /// <param name="width">The maze width in cells.</param>
        /// <param name="height">The maze height in cells.</param>
        /// <param name="startx">X coordinate of first cell. -1 for random.</param>
        /// <param name="starty">Y coordinate of first cell. -1 for random.</param>  
        ///       
        /// <returns>Returns a wall matrix of dimensions 2*(width+1)*(height+1) where layer 0 represents horizontal walls and layer 1 represents vertical walls.
        ///          0xFF means no wall and 0x0 means a regular wall.
        ///          The extra unused row/column of vertical/horizontal walls is left 0.</returns>
        public static byte[,,] GenerateMaze(short width, short height, ref Node pathroot, Rectangle[] ignore = null, short startx = -1, short starty = -1)
        {
            ran = new Random();
            ReRandomize:
            if (startx < 0) startx = (short)ran.Next(0, width);
            if (starty < 0) starty = (short)ran.Next(0, height);
            if (Array.FindIndex(ignore, r => r.Contains(startx, starty)) > -1)
                goto ReRandomize;
            walls = new byte[2, width + 1 /*for extra vertical wall column*/, height + 1 /*for extra horizontal wall row*/];
            visited = new bool[width, height];
            DFSDestroyWS(startx, starty, ref pathroot, ignore);
            return walls;
        }
        private static void DFSDestroyWS(short x, short z, ref Node pathroot, Rectangle[] ignore = null)
        {
            Stack<Node[]> stack = new Stack<Node[]>();
            stack.Push(new Node[] { pathroot = new Node(new Vector3(x, 0, z)), null });
            while (stack.Count > 0)
            {
                Node[] data = stack.Pop();
                int validwallcodes = SuperFactory.GetFactory<Wall>().AvailableTypes-1;                
                x = (short)data[0].Value.X; z = (short)data[0].Value.Z;
                if (visited[x, z])
                    continue;
                visited[x, z] = true;
                if (data[1] != null) // For first time
                {
                    data[0].Neighbours.Add(data[1]);
                    data[1].Neighbours.Add(data[0]);
                    if (x == data[1].Value.X) // Vertically aligned cells
                    {
                        walls[0, x, Math.Max(z, (short)data[1].Value.Z)] = 0xFF; // Remove horizontal wall between them
                        walls[1, x, z] = (byte)ran.Next(0, validwallcodes); // Randomize left wall type
                    }
                    else if (z == data[1].Value.Z) // Horizontally aligend cells
                    {
                        walls[1, Math.Max(x, (short)data[1].Value.X), z] = 0xFF; // Remove vertical wall between them                
                        walls[0, x, z] = (byte)ran.Next(0, validwallcodes); // Randomize top wall type
                    }
                }
                short startdir = (short)ran.Next(0, 4);
                short sign = (short)(ran.Next(99) > 49 ? 1 : -1);
                for (short i = 0; i < 4; i++)
                {
                    // +4 to compensate for counter clock wise rotation. %4 to stay in range
                    short dir = (short)((4 + startdir + sign * i) % 4);
                    short tx = (short)(x + (dir == 1 ? 1 : dir == 3 ? -1 : 0));
                    short tz = (short)(z + (dir == 2 ? 1 : dir == 0 ? -1 : 0));
                    // More overhead checking here means less memory usage
                    if (tx >= 0 && tz >= 0 && tx < visited.GetLength(0) && tz < visited.GetLength(1) // Check whether target is inside workarea                        
                        && !visited[tx, tz] // Check if already visited
                        && (ignore == null || Array.TrueForAll(ignore, r => !r.Contains(tx, tz)))) // Finally, make sure it's not in an ignored area
                        stack.Push(new Node[] { new Node(new Vector3(tx, 0, tz)), data[0] });
                }
            }
        }
    }
}
