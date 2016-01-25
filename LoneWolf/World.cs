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
        LinkedList<WorldElement> obs = new LinkedList<WorldElement>();
        LinkedList<WorldElement> todestroy = new LinkedList<WorldElement>();
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

        public void Add(WorldElement model)
        {
            obs.AddFirst(model);
        }

        internal void Destroy(WorldElement target)
        {
            target.Destroyed = true;
            todestroy.AddFirst(target);
        }

        public void Draw()
        {
            //BoundingFrustum frustum = new BoundingFrustum()
            foreach (WorldElement m in obs.Where(obj => obj.DistanceTo(cam.Position) < cam.FarClip))            
            {
                m.Draw(cam);
            }
            floor.Draw(cam);
        }

        internal void Update(GameTime time)
        {
            foreach (WorldElement tdm in todestroy)
                obs.Remove(tdm);
            todestroy.Clear();
            foreach (WorldElement m in obs)
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
            foreach (WorldElement obj in obs)
                if (obj.Contains(pos))
                    return obj;
            return null;
        }
    }
}
