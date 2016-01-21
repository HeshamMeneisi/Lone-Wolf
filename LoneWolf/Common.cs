using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
#if ANDROID
using Android.OS;
using Java.Util;
using Xamarin.Facebook;
#else
#endif

namespace LoneWolf
{
    // Common gampelay code affecting multiple classes is kept here
    static class Common
    {
        internal static bool isSameAngle(double a, double b, double tolerance = 1e-9)
        {
            a = NormalizeAngle(a);
            b = NormalizeAngle(b);
            if (Math.Abs(a - b) <= tolerance) return true;
            return false;
        }

        private static double NormalizeAngle(double a)
        {
            while (a >= Math.PI * 2)
                a -= Math.PI * 2;
            while (a < 0)
                a += Math.PI * 2;
            return a;
        }

        internal static Orientation ReverseOrientation(Orientation mode)
        {
            return mode == Orientation.Landscape ? Orientation.Portrait : Orientation.Landscape;
        }

        internal static Direction NextDirCW(Direction dir, int count = 1)
        { return count >= 0 ? (Direction)((int)(dir + count) % 4) : NextDirCCW(dir, -count); }

        internal static byte[] Texture2DToBytes(Texture2D thumb)
        {
            MemoryStream s = new MemoryStream();
            thumb.SaveAsJpeg(s, thumb.Width, thumb.Height);
            byte[] data = new byte[s.Length];
            s.Position = 0;
            s.Read(data, 0, data.Length);
            s.Dispose();
            return data;
        }
        internal static Texture2D Texture2DFromBytes(byte[] data)
        {
            MemoryStream s = new MemoryStream();
            s.Write(data, 0, data.Length);
            s.Position = 0;
            Texture2D ret = Texture2D.FromStream(Manager.Game.GraphicsDevice, s);
            s.Dispose();
            return ret;
        }
        internal static Direction NextDirCCW(Direction dir, int count = 1)
        { return count >= 0 ? (Direction)((int)(dir + 3 * count) % 4) : NextDirCW(dir, -count); }
        internal static Direction RelativeDir(Direction dir, Direction neworigin, Direction origin = Direction.North)
        { return NextDirCW(dir, origin - neworigin); }

        internal static bool isDirVertical(Direction dir)
        { return dir == Direction.North || dir == Direction.South; }

        internal static bool isDirHorizontal(Direction dir)
        { return dir == Direction.East || dir == Direction.West; }
    }
    public enum PackageType
    {
        None = -2, User = 0, Beach = 1, Space = 2,
        Online = 3
    }

    static class CommonData
    {
        internal static string[] SPH = new string[] { "8PvAJQUx6J33knnHVaSecA==", "GK+t2sBr4zNwtCUgffFXCw==", "x8tdDAc3r5uCdzp18/EMrQ==", "9QZditpqmYdWC5ChozR5uQ==", "tiifEN0A+FZATZTLdzvRsA==", "0455fMlYN5hUkKZsr1etLQ==", "0ujO099GHYURU2Hgll6i/A==", "ndGBHDg7tu16LuG/FTQqVw==", "2eUQpjOQTCT//PN3qJJPmg==", "36K5WA+iX8gByEJzouQ/jw==", "rSDd5miYZKuhpZV1Kw1Oqw==", "kbb1nQ/loM1ENPzw0QZWXQ==", "Sm0IDwyVkStm0Mj9+IUqkA==", "8xqQRMlwfUi0QVCG7MV+1Q==", "YJvu3yJaJWrTBAgMtx4AmA==", "9oGHYqXnEVCRrKouIu4bVg==", "rJuTocMMrRUOfPPyn5asAA==", "MlWIf6XJNNvODAgqHjvSiQ==", "WR3dIKFly9xCEMk84k5rxQ==", "7Uf8no0Fsk/ElZeane4wgg==", "vOC1zuCcQ0TmahtB45vX0g==", "SSijqUSUSjVkCjzZu1rhKw==", "BaCX9XH+G4HXc5WFX+wGNw==", "xPLOWRZ3YCbIfN8rQDYjBw==", "XYzHNSVulvO1lCYgHIVVaQ==" };
        internal static string[] BPH = new string[] { "svFMgqgbsNqpGmptucZvew==", "h0vHbNd044njuTV3PCMBrw==", "64FNLHlarDpooGG3PC6NGQ==", "BTA+PIdwsxdqQXxcWirXOQ==", "Pzkb2fBXj1zXcZWaVyf8vw==", "s6arXO5YJ8QEa+fVFjc33A==", "9/osIu3FoERT4+CQFlCjIg==", "wOr16mVtYHGJ0tBcL+y/qw==", "DAHVHCPlaCOXcBPQpSKUQw==", "34Tc5uL85VXCGrxm+il7Vg==", "/jFLCmDBUfWFFH6Lx11sMQ==", "OlYdAkhOPH7Lq4GOQkJ58Q==", "j4ChfnCyvAXdgUkpEO+xKg==", "VxPUKRby+dyB8v26Em3yRA==", "j4ChfnCyvAXdgUkpEO+xKg==", "HvsoXD5/oo+/UlcjdkBUBg==", "PFpSoDfo3BvG2b7ERREijQ==", "hg73AWXT3pVaLCxMKNI1Rw==", "yIU7DX60kP66VAl4lMfBkw==", "RxuHT/3wz4UHuWGFnd92/g==", "PW+XsrsHChhozYFarhaegw==", "myUaVuZXtRGXZ/eNTecSIA==", "G5mD1gQCs2kcLzRYIFM5jg==", "WbMkRKeSRezg789uB0mGnA==", "Veyyb+Jmle6bMn7Q0OhIZA==" };

        public static Dictionary<Keys, char[]> KeyCharMap = new Dictionary<Keys, char[]>()
        {
            { Keys.OemTilde,new char[] {'`','~' } },

            { Keys.OemMinus,new char[] {'-','_' } },

            { Keys.OemPlus,new char[] {'=','+' } },

            { Keys.Divide,new char[] {'/','\0' } },

            { Keys.Multiply,new char[] {'*','\0' } },

            { Keys.Subtract,new char[] {'-','\0' } },

            { Keys.Add,new char[] {'+','\0' } },

            { Keys.OemSemicolon,new char[] {';',':' } },

            { Keys.OemQuotes,new char[] {'\'','\"' } },

            { Keys.OemPipe,new char[] {'\\','|' } },

            { Keys.OemBackslash,new char[] {'\\','|' } },

            { Keys.OemComma,new char[] {',','<' } },

            { Keys.OemPeriod,new char[] {'.','>' } },

            { Keys.OemQuestion,new char[] {'/','?' } },

            { Keys.Space,new char[] {' ','\0' } },

            { Keys.Decimal,new char[] {'.','\0' } },

            { Keys.D0, new char[] { '0', ')' } },

            { Keys.D1, new char[] { '1', '!' } },

            { Keys.D2, new char[] { '2', '@' } },

            { Keys.D3, new char[] { '3', '#' } },

            { Keys.D4, new char[] { '4', '$' } },

            { Keys.D5, new char[] { '5', '%' } },

            { Keys.D6, new char[] { '6', '^' } },

            { Keys.D7, new char[] { '7', '&' } },

            { Keys.D8, new char[] { '8', '*' } },

            { Keys.D9, new char[] { '9', '(' } },

            { Keys.A, new char[] { 'a', 'A' } },

            { Keys.B, new char[] { 'b', 'B' } },

            { Keys.C, new char[] { 'c', 'C' } },

            { Keys.D, new char[] { 'd', 'D' } },

            { Keys.E, new char[] { 'e', 'E' } },

            { Keys.F, new char[] { 'f', 'F' } },

            { Keys.G, new char[] { 'g', 'G' } },

            { Keys.H, new char[] { 'h', 'H' } },

            { Keys.I, new char[] { 'i', 'I' } },

            { Keys.J, new char[] { 'j', 'J' } },

            { Keys.K, new char[] { 'k', 'K' } },

            { Keys.L, new char[] { 'l', 'L' } },

            { Keys.M, new char[] { 'm', 'M' } },

            { Keys.N, new char[] { 'n', 'N' } },

            { Keys.O, new char[] { 'o', 'O' } },

            { Keys.P, new char[] { 'p', 'P' } },

            { Keys.Q, new char[] { 'q', 'Q' } },

            { Keys.R, new char[] { 'r', 'R' } },

            { Keys.S, new char[] { 's', 'S' } },

            { Keys.T, new char[] { 't', 'T' } },

            { Keys.U, new char[] { 'u', 'U' } },

            { Keys.V, new char[] { 'v', 'V' } },

            { Keys.W, new char[] { 'w', 'W' } },

            { Keys.X, new char[] { 'x', 'X' } },

            { Keys.Y, new char[] { 'y', 'Y' } },

            { Keys.Z, new char[] { 'z', 'Z' } },

            { Keys.NumPad0, new char[] { '0', '\0' } },

            { Keys.NumPad1, new char[] { '1', '\0' } },

            { Keys.NumPad2, new char[] { '2', '\0' } },

            { Keys.NumPad3, new char[] { '3', '\0' } },

            { Keys.NumPad4, new char[] { '4', '\0' } },

            { Keys.NumPad5, new char[] { '5', '\0' } },

            { Keys.NumPad6, new char[] { '6', '\0' } },

            { Keys.NumPad7, new char[] { '7', '\0' } },

            { Keys.NumPad8, new char[] { '8', '\0' } },

            { Keys.NumPad9, new char[] { '9', '\0' } }
        };
    }
}
enum Direction { North = 0, East = 1, South = 2, West = 3 }