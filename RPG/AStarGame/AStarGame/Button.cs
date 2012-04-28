using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{
    public class Button
    {
        protected Texture2D image;
        protected SpriteFont font;
        protected Rectangle location;
        protected string text;
        protected Vector2 textLocation;
        protected SpriteBatch spriteBatch;
        protected MouseState mouse;
        protected MouseState oldMouse;
        protected bool clicked = false;

        public Button(Texture2D texture, SpriteFont font, SpriteBatch sBatch, String text)
        {
            image = texture;
            this.font = font;
            location = new Rectangle(0, 0, image.Width, image.Height);
            spriteBatch = sBatch;
            Text = text;
        }

        public string Text
        {
            get { return text; }
            set
            { 
                text = value;
                Vector2 size = font.MeasureString(text);
                textLocation = new Vector2();
                textLocation.Y = location.Y + ((image.Height / 2) - (size.Y / 2));
                textLocation.X = location.X + ((image.Width / 2) - (size.X / 2));
            }
        }

        public void Location(int x, int y)
        {
            location.X = x;
            location.Y = y;
        }

        public virtual void Update()
        {
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Released &&
                oldMouse.LeftButton == ButtonState.Pressed)
            {
                if (location.Contains(new Point(mouse.X, mouse.Y)))
                {
                    clicked = true;
                }
            }

            Text = text;

            oldMouse = mouse;
        }

        public virtual void Draw()
        {
            spriteBatch.Begin();

            if (location.Contains(new Point(mouse.X, mouse.Y)))
            {
                spriteBatch.Draw(image,
                    location,
                    Color.Silver);
            }
            else
            {
                spriteBatch.Draw(image,
                    location,
                    Color.White);
            }


            spriteBatch.DrawString(font, text, textLocation, Color.Black);
            spriteBatch.End();
        }
    }
}
