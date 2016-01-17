using LoneWolf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AirMaze
{
    class StageCont : IState
    {
        public void Draw(SpriteBatch batch)
        {
            // Draw world
            // Draw overlay GUI
            throw new NotImplementedException();
        }

        public void HandleEvent(WorldEvent e, bool forcehandle = false)
        {
            // Send to GUI
            // if not handled send to world
            throw new NotImplementedException();
        }

        public void OnActivated(params object[] args)
        {
            // On play
            throw new NotImplementedException();
        }

        public void Update(GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}
