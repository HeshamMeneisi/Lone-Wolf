using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LoneWolf
{
    internal class CollisionHandler
    {
        internal static void Handle(IEnumerable<WorldElement> obs)
        {
            foreach (Object3D obj in obs)
            {
                if (obj is DynamicObject)
                    CheckDynamiObject(obj as DynamicObject, obs);
                else if (obj is Projectile)
                    CheckProjectile(obj as Projectile, obs);
            }
        }

        private static void CheckProjectile(Projectile target, IEnumerable<WorldElement> obs)
        {
            foreach (Object3D obj in obs)
            {
                if (obj != target && obj.Intersects(target))
                {
                    target.Collide(obj);
                    break;
                }
            }
        }

        private static void CheckDynamiObject(DynamicObject target, IEnumerable<WorldElement> obs)
        {
            DynamicObject dobj;
            foreach (Object3D obj in obs)
            {
                if (obj != target && obj.Intersects(target))
                {
                    if ((dobj = obj as DynamicObject) != null)
                    {
                        if (dobj is Player || dobj.TimeStamp > target.TimeStamp)
                            target.SeparateFrom(obj);
                        else dobj.SeparateFrom(target);
                    }
                    else
                        target.SeparateFrom(obj);
                }
            }
        }
    }
}