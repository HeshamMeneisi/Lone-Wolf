using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LoneWolf
{
    class Screen
    {
        private static GameWindow window;
        private static GraphicsDeviceManager device;
        private static bool isVirtual = false;
        private static Vector2 virtualbounds;
        // This is deprecated
        //public static int Width{get{return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;}}
        //public static int Height { get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; } }
        public static float ViewWidth
        {
            get
            {
                return isVirtual ? virtualbounds.X :
#if WP81
                window.ClientBounds.Width * ((window.CurrentOrientation == DisplayOrientation.LandscapeLeft || window.CurrentOrientation == DisplayOrientation.LandscapeRight) && !Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsFullScreen ? 0.94f : 1);
#else
                window.ClientBounds.Width;
#endif
            }
        }
        public static float ViewHeight
        {
            get
            {
                return isVirtual ? virtualbounds.Y :
#if WP81
                    window.ClientBounds.Height * ((window.CurrentOrientation == DisplayOrientation.Portrait || window.CurrentOrientation == DisplayOrientation.PortraitDown) && !Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsFullScreen ? 0.94f : 1);
#else
                    window.ClientBounds.Height;
#endif
            }
        }

        public static int ActualWidth
        {
            get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; }
        }

        public static int ActualHeight
        {
            get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; }
        }
        // window.CurrentOrientation also works, but this is more literal representation of the concept.
        public static Orientation Mode { get { return ViewWidth > ViewHeight ? Orientation.Landscape : Orientation.Portrait; } }

        public static float BigDim { get { return MathHelper.Max(ViewWidth, ViewHeight); } }
        public static float SmallDim { get { return MathHelper.Min(ViewWidth, ViewHeight); } }

        public static float YAdjustRatio
        {
            get
            {
                return
                    ViewHeight / window.ClientBounds.Height;
            }
        }
        public static float XAdjustRatio
        {
            get
            {
                return
                    ViewWidth / window.ClientBounds.Width;
            }
        }
        public static void SetUp(GameWindow gamewindow, GraphicsDeviceManager devicemanager)
        {
            device = devicemanager;
            window = gamewindow;
        }
        public static void SetFullScreen(bool state)
        {
            if (device.IsFullScreen != state)
            { device.ToggleFullScreen(); device.PreferredBackBufferWidth = ActualWidth; device.PreferredBackBufferHeight = ActualHeight; device.ApplyChanges(); }
        }

        public static void MakeVirtual(Vector2 virtualbounds)
        {
            Screen.virtualbounds = virtualbounds;
            isVirtual = true;
            Manager.HandleEvent(new DisplaySizeChangedEvent(virtualbounds));
        }

        public static void MakeReal()
        {
            isVirtual = false;
            Manager.HandleEvent(new DisplaySizeChangedEvent(new Vector2(ViewWidth, ViewHeight)));
        }
    }
}
