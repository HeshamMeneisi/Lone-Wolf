using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    static class MiniMap
    {
        static int dim = 160;
        static int diag = (int)(dim * Math.Sqrt(2));
        public static void Draw(SpriteBatch batch, Map map, float rot)
        {
            Texture2D minmap = map.MiniMap;
            batch.Draw(minmap, new Rectangle(diag / 2, (int)(Screen.ViewHeight - diag / 2), dim, dim), null, Color.White, rot, new Vector2(minmap.Width / 2, minmap.Height / 2), SpriteEffects.None, 0);
        }
    }
}
