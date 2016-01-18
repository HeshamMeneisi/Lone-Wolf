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
        List<Model3D> obs;
        MainModel model;
        Floor floor;
        BasicEffect e;
        Camera cam;
        public World(Model m,GraphicsDeviceManager g,Camera c,BasicEffect eff)
        {
            model = new MainModel(m, new Vector3(-29, 0, 37), g);
            cam = c;
            e = eff;
        }
        public void Add(Model3D model)
        {
            obs.Add(model);
        }
        
        public void Draw()
        {
            model.Draw();
            floor.Draw(e,cam);
            foreach(Model3D m in obs)
            {
                m.Draw(model.view);
            }
        }
    }
}
