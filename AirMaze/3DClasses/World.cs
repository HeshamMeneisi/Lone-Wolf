using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using LoneWolf.Extra;
namespace LoneWolf
{
    class World
    {
        static World instance;
        List<Model3D> obs = new List<Model3D>();
        Player model;
        Floor floor;
        BasicEffect e;
        Camera cam;

        public Camera ActiveCam
        {
            get { return cam; }
            set { cam = value; }
        }

        public World(Model m, GraphicsDeviceManager g, Camera c, BasicEffect eff)
        {
            model = new Player(m, new Vector3(-29, 0, 37));
            cam = c;
            e = eff;
            floor = new Floor(g.GraphicsDevice, 1000, 1000);
            instance = this;
        }
        public void Add(Model3D model)
        {
            obs.Add(model);
        }

        public void Draw()
        {
            model.Draw(cam);
            floor.Draw(e, cam);
            foreach (Model3D m in obs)
            {
                m.Draw(cam);
            }
        }

        internal void Update(GameTime time)
        {            
            model.Update(time);
            foreach (Model3D m in obs)
            {
                m.Update(time);
            }
            cam.Update(time);
        }

        public static World GetInstance()
        {
            return instance;
        }
    }
}
