using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace RPG
{
    public class BattleEntity
    {
        const int FLASH_TIME = 400;
        const int FLASH_INTERVAL = 50;

        public Rectangle rec;
        bool isflashing;
        int flashstarttime;
        Texture2D normal;
        Texture2D flashing;
        Texture2D current;


        public BattleEntity(Rectangle rec, Texture2D n, Texture2D f)
        {
            this.rec = rec;
            this.normal = n;
            this.flashing = f;
            this.current = normal;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(current, rec, Color.AliceBlue);
        }

        public void Update(GameTime gameTime)
        {
            int t = (int)gameTime.TotalGameTime.TotalMilliseconds;

            if (isflashing && ((t - flashstarttime) > FLASH_TIME))
            {
                Console.WriteLine("Should stop flashing!, time is " + flashstarttime);
                isflashing = false;
                current = normal;
            }

            if (isflashing)
            {
                int cur = (int)((t - flashstarttime) / FLASH_INTERVAL) % 2;
                if (cur == 1)
                    current = normal;
                else
                    current = flashing;
            }
        }

        public void startFlashing(GameTime gameTime)
        {
           flashstarttime = (int)gameTime.TotalGameTime.TotalMilliseconds;
           Console.WriteLine("Should start flashing!, time is " + flashstarttime);
           isflashing = true;
        }
    }
}
