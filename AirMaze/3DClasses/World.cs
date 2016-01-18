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
        GraphicsDeviceManager graphics;
        BasicEffect e;
        public void AddModel(Model3D model)
        {
            obs.Add(model);
        }
        public void AddMain(MainModel m,Floor f)
        {
            model = m;
            floor = f;
        }
        public void Draw()
        {
            model.Draw();
            floor.Draw(e, model.view);
            foreach(Model3D m in obs)
            {
                m.Draw();
            }
        }
    }
}
