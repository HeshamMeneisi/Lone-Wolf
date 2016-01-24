namespace LoneWolf
{
    internal class GameStateHolder
    {
        uint score;
        int health;

        public uint Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }
    }
}