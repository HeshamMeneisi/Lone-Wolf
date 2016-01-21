using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class SkinnedModel3D : Model3D
    {
        AnimationPlayer animationPlayer;
        SkinningData skinningData;
        bool playing = false;

        public bool PlayingAnimation
        {
            get
            {
                return playing;
            }
        }

        public SkinnedModel3D(Model m, Vector3 offset, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale = 1, string defaultclip = null) : base(m, offset, baserot, lowanchor, highanchor, scale)
        {
            skinningData = model.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            if (defaultclip != null)
            {
                AnimationClip clip = skinningData.AnimationClips[defaultclip];
                animationPlayer.StartClip(clip);
                playing = true;
            }
        }
        public void StartAnimation(string name)
        {
            AnimationClip clip = skinningData.AnimationClips[name];
            animationPlayer.StartClip(clip);
            playing = true;
        }
        public void StopAnimation(TimeSpan stopingpoint)
        {
            if (!playing) return;
            animationPlayer.Update(stopingpoint, false, Matrix.Identity);
            playing = false;
        }
        public override void Update(GameTime time)
        {
            if (playing)
                animationPlayer.Update(time.ElapsedGameTime, true, Matrix.Identity);
            Debug.WriteLine(time.TotalGameTime.Ticks);
            base.Update(time);
        }
        public override void Draw(Camera cam)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    effect.World = trans;
                    effect.View = cam.View;
                    effect.Projection = cam.Projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }
        }
    }
}
