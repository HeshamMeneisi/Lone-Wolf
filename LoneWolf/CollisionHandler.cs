using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LoneWolf
{
    internal class CollisionHandler
    {
        internal static void Handle(IEnumerable<Model3D> obs)
        {
            foreach (Model3D obj in obs)
            {
                if (obj is DynamicObject)
                    CheckDynamiObject(obj as DynamicObject, obs);
            }
        }

        private static void CheckDynamiObject(DynamicObject target, IEnumerable<Model3D> obs)
        {
            DynamicObject dobj;
            foreach (Model3D obj in obs)
            {
                if (obj != target && obj.Intersects(target))
                {
                    if ((dobj = obj as DynamicObject) != null)
                    {
                        if (dobj.TimeStamp > target.TimeStamp)
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