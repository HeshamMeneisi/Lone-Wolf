﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace LoneWolf
{
    class Player : Model3D
    {
        float speed;
        
        float faceheight = 150;
        Vector3 camoffset;
        float wheelrot = 0.1f;
        public Player(Model m, Vector3 position, Vector3 rotation, float scale) : base(m, position, rotation, scale)
        {
            speed = 1f;
            camoffset = new Vector3(0, faceheight, 0);
        }
        /*2,8,9,10*/
        public void RotateWheels() {
            if (base.model != null)
            {
                base.model.Bones[2].Transform = Matrix.CreateRotationX(wheelrot)
                    * Matrix.CreateTranslation(base.model.Bones[2].Transform.Translation);
                base.model.Bones[8].Transform = Matrix.CreateRotationX(wheelrot)
                    * Matrix.CreateTranslation(base.model.Bones[8].Transform.Translation);
                base.model.Bones[9].Transform = Matrix.CreateRotationX(wheelrot)
                    * Matrix.CreateTranslation(base.model.Bones[9].Transform.Translation);
                base.model.Bones[10].Transform = Matrix.CreateRotationX(wheelrot)
                    * Matrix.CreateTranslation(base.model.Bones[10].Transform.Translation);
            }
        }
        public override void Update(GameTime time)
        {
            RotateWheels();
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                rotation.Y += .05f;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                rotation.Y -= .05f;
            }
            if (ks.IsKeyDown(Keys.W))
            {
                position.X += speed * (float)Math.Sin(rotation.Y);
                position.Z += speed * (float)Math.Cos(rotation.Y);
                wheelrot = 0.5f;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                position.X -= speed * (float)Math.Sin(rotation.Y);
                position.Z -= speed * (float)Math.Cos(rotation.Y);
                wheelrot = -0.5f;
            }
            World.GetInstance().ActiveCam.Position = position+camoffset;
            base.Update(time);
        }
    }
}
