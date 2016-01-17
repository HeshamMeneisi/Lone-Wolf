using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace LoneWolf.Extra
{
    class MainModel
    {
        Model model;
        Texture2D tex;
        public MainModel(Model m,Texture2D t)
        {
            model = m;
            tex = t;
        }
        public void Draw(Camera cam)
        {
            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.View = cam.View;
                    effect.World = transforms[mesh.ParentBone.Index] *
                    Matrix.CreateScale(1.0f, 1.0f, 1.0f);

                    effect.Projection = cam.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
