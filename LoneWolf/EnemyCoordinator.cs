﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class EnemyCoordinator
    {
        static EnemyCoordinator instance;
        private Map map;
        private LinkedList<Enemy> enemies = new LinkedList<Enemy>();

        public static EnemyCoordinator GetInstance()
        {
            if (instance != null) return instance;
            throw new Exception("EnemyCoordinator must be initially instantiated using the public constructor.");
        }
        public EnemyCoordinator(Map map)
        {
            this.map = map;
            instance = this;
        }
        Random ran = new Random();

        public NodedPath GenerateRandomPath(int length)
        {
            List<Vector3> path = new List<Vector3>();
            int rantrav = ran.Next(0, length * length);
            Node current = map.Path;
            Node last = null;
            Vector3 scale = new Vector3(Map.Celld, 0, Map.Celld);
            Vector3 shift = scale / 2;
            for (int i = 0; i <= length + rantrav; i++)
            {
                if (i > rantrav)
                    path.Add(current.Value * scale + shift);
                int r;
                if (current.Neighbours.Count == 0) throw new Exception("Invalid path graph. Isolated node encountered.");
                if (current.Neighbours.Count == 1)
                    r = 0;
                else
                {
                    r = ran.Next(current.Neighbours.Count - 1);
                    if (current.Neighbours[r] == last)
                        r = r > 0 ? r - 1 : r + 1;
                }
                last = current;
                current = current.Neighbours[r];
            }
            return new NodedPath(path);
        }
        public void Register(Enemy enemy)
        {
            enemies.AddFirst(enemy);
            enemy.Position = enemy.Path.Current;
        }
        public void UpdateEnemies()
        {
            foreach (Enemy e in enemies)
            {
                Vector3 velvec = e.Path.Current - e.Position;
                if (velvec.Length() > 0)
                    velvec.Normalize();
                velvec *= e.Velocity;
                e.Position += velvec;
                if ((e.Position - e.Path.Current).Length() < e.Velocity)
                {
                    Vector3 v1 = e.Path.Current;
                    e.Path.NextNode();
                    Vector3 v2 = e.Path.Current;
                    Vector3 v = (v2 - v1);
                    v.Normalize();
                    e.Rotation = new Vector3(0, (float)Math.Atan(v.X / v.Z), 0);
                }
            }
        }
    }
}
