using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Model3DPool
    {
        static Model3DPool instance;
        Stack<Object3D> available = new Stack<Object3D>();
        HashSet<Object3D> taken = new HashSet<Object3D>();
        int maxcapacity = 100;
        bool hardlimit = false;
        public int MaxCapacity
        {
            get
            {
                return maxcapacity;
            }

            set
            {
                maxcapacity = value;
            }
        }

        public bool HardLimit
        {
            get
            {
                return hardlimit;
            }

            set
            {
                hardlimit = value;
            }
        }

        internal static Model3DPool GetInstance()
        {
            if (instance != null) return instance;
            return new Model3DPool();
        }

        private Model3DPool()
        {
            instance = this;
        }
        public Object3D Acquire()
        {
            if (available.Count > 0)
            {
                Object3D ret;
                taken.Add(ret = available.Pop());
                return ret;
            }
            else
            {
                if (available.Count + taken.Count < maxcapacity || !hardlimit)
                    return new Object3D(null, Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero);
            }
            throw new Exception("Model3DPool out of objects. Please raise the capcity or turn off hard limit mode.");
        }
        public void Release(Object3D model)
        {
            taken.Remove(model);
            if (taken.Count + available.Count < maxcapacity)
                available.Push(model);
        }
    }
}
