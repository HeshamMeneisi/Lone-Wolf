using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoneWolf
{
    internal class UIBar : UIVisibleObject
    {
        private Texture2D background;
        private Texture2D fill;
        float progress;

        public UIBar(int w, int h, Texture2D background, Texture2D fill) : base(null)
        {
            size = new Vector2(w, h);
            this.background = background;
            this.fill = fill;
        }

        public float Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value == 1 ? 1 : value % 1;
            }
        }

        internal override void Draw(SpriteBatch batch, Camera2D cam = null)
        {
            batch.Draw(background, BoundingBox.ToRectangle(), Color.White);
            batch.Draw(fill, BoundingBox.ScaleWidth(progress).ToRectangle(), Color.White);
            base.Draw(batch, cam);
        }
    }
}