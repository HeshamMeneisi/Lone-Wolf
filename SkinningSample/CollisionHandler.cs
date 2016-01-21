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
                if (obj!= target && obj.Intersects(target))
                {
                    if ((dobj = obj as DynamicObject) != null)
                    {
                        if (dobj.TimeStamp > target.TimeStamp)
                        { target.UndoLastTranslation(); break; }
                        else { dobj.UndoLastTranslation(); break; }
                    }
                    else
                    {
                        target.UndoLastTranslation();
                        break;
                    }
                }
            }
            // TODO: Handle badly intialized dynammic objects by pushing them out
        }
    }
}