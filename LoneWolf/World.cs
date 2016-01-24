using System;
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
        LinkedList<Model3D> obs = new LinkedList<Model3D>();
        LinkedList<Model3D> todestroy = new LinkedList<Model3D>();
        Terrain floor;
        Camera cam;

        public Camera ActiveCam
        {
            get { return cam; }
            set { cam = value; }
        }

        public int FloorWidth { get; internal set; }
        public int FloorHeight { get; internal set; }

        private World()
        {
            instance = this;
        }

        public void CreateTerrain()
        {
            floor = new Terrain(FloorWidth, FloorHeight);
        }

        public void Add(Model3D model)
        {
            obs.AddFirst(model);
        }

        internal void Destroy(Model3D target)
        {
            target.Destroyed = true;
            todestroy.AddFirst(target);
        }

        public void Draw()
        {
            //BoundingFrustum frustum = new BoundingFrustum()
            foreach (Model3D m in obs.Where(obj => obj.DistanceTo(cam.Position) < cam.FarClip))            
            {
                m.Draw(cam);
            }
            floor.Draw(cam);
        }

        internal void Update(GameTime time)
        {
            foreach (Model3D tdm in todestroy)
                obs.Remove(tdm);
            todestroy.Clear();
            foreach (Model3D m in obs)
            {
                m.Update(time);
            }
            cam.Update(time);
            CollisionHandler.Handle(obs);
        }

        public static World GetInstance()
        {
            if (instance == null)
                return new World();
            return instance;
        }

        internal object GetObjectAt(Vector3 pos)
        {
            foreach (Model3D obj in obs)
                if (obj.Contains(pos))
                    return obj;
            return null;
        }
    }
}
