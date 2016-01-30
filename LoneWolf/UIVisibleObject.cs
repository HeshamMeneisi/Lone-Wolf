﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace LoneWolf
{
    class UIVisibleObject : UIObject
    {
        // First, you can switch to RectangleF for more accuracy.
        protected TextureID2D[] sprite;
        protected Vector2 size = Vector2.Zero; // I told you the size must be custom, texture size is irrelevant to actual size :P
        protected Vector2 origin = Vector2.Zero;
        protected int state = 0;
        protected object tag;
        internal virtual object Tag { get { return tag; } }


        internal virtual int State { get { return state; } set { state = value; } }
        internal UIVisibleObject(TextureID2D[] tid, string id = "", int layer = 0)
        : base(id, layer)
        {
            // Use TextureID to describe the texture.   new VisibleUIObject(new TextureID(
            // int groupIndex   // Filename index in the array Datahandler.TextureFiles
            // , int idx        // Index in the sheet
            //  , int wunits=1, int hunits=1  width and hight units, default 1*1))

            if (tid == null || tid.Length == 0 || !DataHandler.isValid(tid[0])) sprite = null;
            else
            {
                sprite = tid;
                size = new Vector2(tid[0].TotalWidth, tid[0].TotalHeight);
            }
        }

        internal virtual void Draw(SpriteBatch batch, Camera2D cam = null) // You don't need a camera, draw ontop of the stage
        {
            if (!visible || sprite == null)
                return;
            if (cam == null)
                batch.Draw(DataHandler.getTexture(sprite[state].RefKey)/*Texture2D from file*/, BoundingBox.ToRectangle()/*on-screen box*/, DataHandler.getTextureSource(sprite[state])/*Rectange on the sheet*/, Color.White/*white=no tint*/);
            else
            {
                if (cam.isInsideView(LocalBoundingBox))
                {
                    RectangleF nocrop;
                    RectangleF cropped = cam.TransformWithCropping(LocalBoundingBox, out nocrop);
                    RectangleF source = DataHandler.getTextureSource(sprite[state]);
                    // rect is an intersection of nocrop, thus contained by it
                    source = source.Mask(nocrop, cropped);
                    batch.Draw(DataHandler.getTexture(sprite[state].RefKey)/*Texture2D from file*/,
                        cropped.Offset(parent.GlobalPosition).ToRectangle()/*on-screen box*/,
                        source.ToRectangle()/*Rectange on the sheet*/,
                        Color.White/*white=no tint*/);
                }
            }
        }

        internal Vector2 GlobalCenter
        {
            // Real center is found this way (on screen)
            get { return BoundingBox.Center; }
        }
        internal Vector2 LocalCenter
        { get { return LocalBoundingBox.Center; } }
        internal virtual float Width { get { return size.X; } }
        internal virtual float Height { get { return size.Y; } }
        internal virtual Vector2 Size
        {
            get { return new Vector2(Width, Height); }
            set { size = value; }
        }

        internal Vector2 Origin
        {
            get { return this.origin; }
            set { this.origin = value; }
        }

        internal RectangleF LocalBoundingBox
        {
            get
            {
                return new RectangleF(Position.X - Origin.X, Position.Y - Origin.Y, Width, Height);
            }
        }
        internal override RectangleF BoundingBox
        {
            get
            {
                float left = (int)(GlobalPosition.X - origin.X);
                float top = (int)(GlobalPosition.Y - origin.Y);
                return new RectangleF(new Vector2(left, top), Size);
            }
        }

        internal void setSizeRelativeToWidth(float perc)
        {
            float w = Screen.ViewWidth * perc;
            float h = size.Y / size.X * w;

            Vector2 nsize = new Vector2(w, h);
            foreach (UIVisibleObject obj in children.Where(t => t is UIVisibleObject))
                obj.Size *= nsize / Size;
            Size = nsize;

        }
        internal void setSizeRelativeToHeight(float perc)
        {
            float h = Screen.ViewHeight * perc;
            float w = size.X / size.Y * h;
            Vector2 nsize = new Vector2(w, h);
            foreach (UIVisibleObject obj in children.Where(t => t is UIVisibleObject))
                obj.Size *= nsize / Size;
            Size = nsize;
        }

        internal virtual void setSizeRelative(float perc, Orientation mode)
        {
            if (mode == Orientation.Landscape)
                setSizeRelativeToHeight(perc);
            else
                setSizeRelativeToWidth(perc);
        }
        internal override void HandleEvent(WorldEvent e)
        {
        }

        internal void CentralizeSiblings()
        {
            foreach (UIVisibleObject obj in siblings.Where(t => t is UIVisibleObject))
                obj.Position = new Vector2(position.X + (Width - obj.Width) / 2, position.Y + (Height - obj.Height) / 2);
        }

        internal void FitSiblings()
        {
            foreach (UIVisibleObject obj in siblings.Where(t => t is UIVisibleObject))
                if (obj.Width > obj.Height)
                {
                    float h = Width * obj.Height / obj.Width,
                          w = Width;
                    obj.Size = new Vector2(w, h);
                }
                else
                {
                    float w = Height * obj.Width / obj.Height,
                          h = Height;
                    obj.Size = new Vector2(w, h);
                }
        }

        public override string ToString()
        {
            return (this.GetType() + ":" + id + "\nLocal:" + LocalBoundingBox.ToString() + "\nGlobal:" + BoundingBox.ToString());
        }
    }
}
