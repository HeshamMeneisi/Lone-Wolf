﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utilities
{
    internal interface IState
    {
        void Update(GameTime time);

        void Draw(SpriteBatch batch);

        void HandleEvent(WorldEvent e, bool forcehandle = false);

        void OnActivated(params object[] args);
    }
}
