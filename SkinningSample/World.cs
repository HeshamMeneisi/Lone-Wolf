﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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

        public World(Camera c, BasicEffect feff, Floor floor)
        {
            cam = c;
            e = feff;
            this.floor = floor;
            instance = this;
        }
        public void Add(Model3D model)
        {
            obs.Add(model);
        }

        public void Draw()
        {
            floor.Draw(e, cam);
            foreach (Model3D m in obs.Where(obj => obj.DistanceTo(cam.Position) < cam.FarClip)
                .OrderByDescending(obj => obj.DistanceTo(cam.Position)))
            {
                m.Draw(cam);
            }
        }

        internal void Update(GameTime time)
        {
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