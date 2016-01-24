using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace LoneWolf
{
    internal class NodedPath
    {
        private Stack<Vector3> path;
        private Stack<Vector3> rpath = new Stack<Vector3>();
        bool isreverse = false;
        public NodedPath(IEnumerable<Vector3> path)
        {
            this.path = new Stack<Vector3>();
            if (path == null) this.path = null;
            else
            {
                foreach (Vector3 v in path)
                    this.path.Push(v);
                current = this.path.Peek();
            }
        }
        Vector3 current;

        public Vector3 Current
        {
            get
            {
                return current;
            }
        }

        public Vector3 NextNode()
        {
            if (isreverse)
            {
                if (rpath.Count == 0)
                {
                    rpath.Push(current = path.Pop());
                    isreverse = false;
                }
                else
                    path.Push(current = rpath.Pop());
            }
            else
            {
                if (path.Count == 0)
                {
                    path.Push(current = rpath.Pop());
                    isreverse = true;
                }
                else
                    rpath.Push(current = path.Pop());
            }
            return current;
        }

        public void EnforceCurrentNode(Func<Vector3, bool> predicate)
        {
            while (rpath.Count > 0)
            {
                path.Push(current = rpath.Pop());
                if (predicate(current)) return;
            }
            while (path.Count > 0)
            {
                rpath.Push(current = path.Pop());
                if (predicate(current)) return;
            }
        }
        public bool FindNode(Func<Vector3, bool> predicate, ref Vector3 node)
        {
            foreach (Vector3 v in path.ToArray())
                if (predicate(v)) { node = v; return true; }
            foreach (Vector3 v in rpath.ToArray())
                if (predicate(v)) { node = v; return true; }
            return false;
        }        
    }
}