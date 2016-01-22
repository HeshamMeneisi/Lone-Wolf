using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    static class Sky
    {
        private static Texture2D skytex = Manager.Game.Content.Load<Texture2D>("Textures\\Sky");
        public static void Draw(SpriteBatch batch)
        {
            batch.Begin();            
            batch.Draw(skytex, Manager.Game.GraphicsDevice.Viewport.Bounds, Color.White);
            batch.End();
        }
    }
}
