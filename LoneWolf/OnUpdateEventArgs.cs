using Microsoft.Xna.Framework;

namespace LoneWolf
{
    public class OnUpdateEventArgs
    {
        private GameTime gametime;

        public GameTime Gametime
        {
            get
            {
                return gametime;
            }
        }

        public OnUpdateEventArgs(GameTime time)
        {
            gametime = time;
        }
    }
}