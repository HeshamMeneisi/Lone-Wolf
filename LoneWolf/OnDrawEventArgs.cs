using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    public class OnDrawEventArgs
    {
        private SpriteBatch spritebatch;

        public OnDrawEventArgs(SpriteBatch batch)
        {
            spritebatch = batch;
        }

        public SpriteBatch Spritebatch
        {
            get
            {
                return spritebatch;
            }
        }
    }
}