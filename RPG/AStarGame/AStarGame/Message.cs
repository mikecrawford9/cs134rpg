using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{
    class Message
    {
        Rectangle rec;
        String text;
        Texture2D pixel;
        SpriteFont font;

        public Message(String text, Texture2D pixel, SpriteFont font)
        {
            rec = new Rectangle(30, 450, 480, 60);
            this.text = text;
            this.pixel = pixel;
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, rec, Color.Black);
            spriteBatch.DrawString(font, text, new Vector2(rec.X + 20, rec.Y - 6), Color.Yellow);
        }
    }
}
