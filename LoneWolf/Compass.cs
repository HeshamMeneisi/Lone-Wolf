using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    internal class Compass
    {
        private static Texture2D comptex = Manager.Game.Content.Load<Texture2D>("Textures\\Compass");
        static float dim = 100;
        static Vector2 center = new Vector2(comptex.Width / 2, comptex.Height / 2);
        public static void Draw(SpriteBatch batch, float rot)
        {
            RectangleF rect = new RectangleF(Screen.ViewWidth - dim, Screen.ViewHeight - dim, dim, dim);
            batch.Draw(comptex, rect.ToRectangle(), null, Color.White, rot, center, SpriteEffects.None, 0);
        }
    }
}