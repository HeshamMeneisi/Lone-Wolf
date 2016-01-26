using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using System;

namespace LoneWolf
{
    class SkinnedModel3D : Object3D
    {
        AnimationPlayer animationPlayer;
        SkinningData skinningData;
        bool playing = false;
        bool loop = true;
        private string defaultclip;

        public bool PlayingAnimation
        {
            get
            {
                return playing;
            }
        }
        public override Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                base.Model = value;
                SetupAnimationData();
            }
        }

        public string DefaultClip
        {
            get
            {
                return defaultclip;
            }

            set
            {
                defaultclip = value;
            }
        }

        public SkinnedModel3D(Model m, Vector3 origin, Vector3 baserot, Vector3 lowanchor, Vector3 highanchor, float scale = 1, string defaultclip = null, bool setup = true) : base(m, origin, baserot, lowanchor, highanchor, scale)
        {
            this.defaultclip = defaultclip;
            if (setup)
                SetupAnimationData();
        }

        private void SetupAnimationData()
        {
            skinningData = model.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag. Check if the model has the right processor.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);
            if (defaultclip != null)
            {
                AnimationClip clip = skinningData.AnimationClips[defaultclip];
                animationPlayer.StartClip(clip);
                playing = true;
            }
        }

        public void StartAnimation(string name, bool loop = true)
        {
            this.loop = loop;
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
                if (!animationPlayer.EndReached || loop)
                    animationPlayer.Update(time.ElapsedGameTime, true, Matrix.Identity);
                else OnFinishedPlayingAnimation();
            base.Update(time);
        }

        public virtual void OnFinishedPlayingAnimation()
        {
            var frames = animationPlayer.CurrentClip.Keyframes;
            StopAnimation(frames[frames.Count - 1].Time);
        }

        public override void Draw(Camera cam)
        {
            if (changed)
                UpdateTransformation();
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
