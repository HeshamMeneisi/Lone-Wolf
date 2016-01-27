using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace LoneWolf
{
    class MainMenuCont : IState
    {
        static MainMenuCont instance = null;

        UIMenu menu;
        UIButton playbtn;
        UIButton exitbtn;

        private MainMenuCont()
        {
            menu = new UIMenu();
            menu.Add(playbtn
            = new UIButton(DataHandler.UIObjectsTextureMap[UIObjectType.PlayBtn],
                (sender) => Manager.StartNewGame()));
            menu.Add(exitbtn = new UIButton(DataHandler.UIObjectsTextureMap[UIObjectType.ExitBtn],
                (sender) => Manager.Game.Exit()));
            instance = this;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            menu.Draw(batch);
            batch.End();
        }

        public void HandleEvent(WorldEvent e, bool forcehandle = false)
        {
            menu.HandleEvent(e);
        }

        public void OnActivated(params object[] args)
        {
            menu.setAllSizeRelative(0.2f, Orientation.Landscape);
            menu.ArrangeInForm(Orientation.Portrait);
            menu.Position = new Vector2((Screen.Width - menu.Width) / 2, (Screen.Height - menu.Height) / 2);
            Manager.GameSettings.MusicVolume = 0.8f;
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Loop], SoundCategory.Music, true);
        }

        public void Update(GameTime time)
        {
            menu.Update(time);
        }
        internal static MainMenuCont GetInstance()
        {
            if (instance != null) return instance;
            return new MainMenuCont();
        }
    }
}
