using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{
    public class AttackButton : Button
    {
        public Player player;
        public List<Event> eventList;

        public AttackButton(Texture2D texture, SpriteFont font, String text, Player p, List<Event> events) : base(texture, font, text)
        {
            this.image = texture;
            this.font = font;
            this.location = new Rectangle(0, 0, image.Width, image.Height);
            this.Text = text;
            this.player = p;
            this.eventList = events;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (base.clicked)
            {
                foreach (Event e in eventList)
                {
                    Game1.addToEventQueue(e);
                }
            }

        }

    }
}
