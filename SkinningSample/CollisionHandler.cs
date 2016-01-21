using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LoneWolf
{
    internal class CollisionHandler
    {
        internal static void Handle(List<Model3D> obs)
        {
            foreach (Model3D obj in obs)
            {
                if (obj is DynamicObject)
                    CheckDynamiObject(obj as DynamicObject, obs);
            }
        }

        private static void CheckDynamiObject(DynamicObject target, List<Model3D> obs)
        {
            DynamicObject dobj;
            foreach (Model3D obj in obs)
            {
                if (obj != target && obj.Intersects(target))
                {
                    if ((dobj = obj as DynamicObject) != null)
                    {
                        if (dobj.TimeStamp > target.TimeStamp)
                        { target.SeparateFrom(obj); break; }
                        else { dobj.SeparateFrom(target); break; }
                    }
                    else
                    {
                        target.SeparateFrom(obj);
                        break;
                    }
                }
            }
        }
    }
}