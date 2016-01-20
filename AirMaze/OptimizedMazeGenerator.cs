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
        public static byte[,,] GenerateMaze(short width, short height, short startx = -1, short starty = -1, Rectangle[] ignore = null)
        {
            ran = new Random();
            if (startx < 0) startx = (short)ran.Next(0, width);
            if (starty < 0) starty = (short)ran.Next(0, height);
            walls = new byte[2, width + 1 /*for extra vertical wall column*/, height + 1 /*for extra horizontal wall row*/];
            visited = new bool[width, height];
            DFSDestroyWS(startx, starty, ignore);
            return walls;
        }
        private static void DFSDestroyWS(short x, short y, Rectangle[] ignore = null)
        {
            Stack<short[]> stack = new Stack<short[]>();
            stack.Push(new short[] { x, y, -1, -1 });
            while (stack.Count > 0)
            {
                short[] data = stack.Pop();
                // data contains x1,y1,x2,y2 of cell1 (the current target) and cell2 (the parent).
                x = data[0]; y = data[1];
                if (visited[x, y]) continue; // Redundant (Precautionary to edits to bellow code)
                visited[x, y] = true;
                if (x == data[2]) // Vertically aligned cells
                    walls[0, x, Math.Max(y, data[3])] = 0xFF; // Remove horizontal wall between them
                else if (y == data[3]) // Horizontally aligend cells
                    walls[1, Math.Max(x, data[2]), y] = 0xFF; // Remove vertical wall between them
                short startdir = (short)ran.Next(0, 3);
                short sign = (short)(ran.Next(99) > 49 ? 1 : -1);
                for (short i = 0; i < 4; i++)
                {
                    // +4 to compensate for counter clock wise rotation. %4 to stay in range
                    short dir = (short)((4 + startdir + sign * i) % 4);
                    short tx = (short)(x + (dir == 1 ? 1 : dir == 3 ? -1 : 0));
                    short ty = (short)(y + (dir == 2 ? 1 : dir == 0 ? -1 : 0));
                    // More overhead checking here means less memory usage
                    if (tx >= 0 && ty >= 0 && tx < visited.GetLength(0) && ty < visited.GetLength(1) // Check whether target is inside workarea                        
                        && !visited[tx, ty] // Check if already visited
                        && (ignore == null || Array.TrueForAll(ignore, r => !r.Contains(tx, ty)))) // Finally, make sure it's not in an ignored area
                        stack.Push(new short[] { tx, ty, x, y });
                }
            }
        }
    }
}
