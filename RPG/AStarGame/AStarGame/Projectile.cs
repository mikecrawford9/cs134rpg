using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG
{
    
    public class Projectile
    {
        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        private int xwall;

        // Get the width of the projectile ship
        public int Width
        {
            get { return Texture.Width; }
        }

        // Get the height of the projectile ship
        public int Height
        {
            get { return Texture.Height; }
        }

        public Projectile()
        {
        }

        // Determines how fast the projectile moves
        int projectileMoveSpeed;

        public bool isLeft;
        bool soundplay;
        Spell ba;
        BattleEntity ent;

        public void Initialize( Texture2D texture, Spell action, Vector2 position, bool isLeft, int xwall, BattleEntity ent)
        {

            Texture = texture;
            Position = position;
            Active = true;
            this.xwall = xwall;
            projectileMoveSpeed = 6;
            this.isLeft = isLeft;
            soundplay = false;
            ba = action;
            this.ent = ent;
        }

        public void Update(GameTime gameTime)
        {
            // Projectiles always move to the right
            
            if (isLeft)
            {
                Position.X -= projectileMoveSpeed;
                if (Position.X < xwall)
                {
                    if (Active == true && Texture != null)
                    {
                        if (ba.element.Equals(SpellElement.FIRE))
                        {
                            Game1.firesound.Play();
                        }
                        else if (ba.element.Equals(SpellElement.HOLY))
                        {
                            Game1.healSound.Play();
                        }
                        else if (ba.element.Equals(SpellElement.PHYSICAL))
                        {
                            Game1.swordSound.Play();
                        }
                        else
                        {
                            Game1.swordSound.Play();
                        }

                        ent.startFlashing(gameTime);
                    }
                    Active = false;
                }
            }
            else
            {
                Position.X += projectileMoveSpeed;
                
                if (Position.X > xwall)
                {
                    if (Active == true && Texture != null)
                    {
                        if (ba.element.Equals(SpellElement.FIRE))
                        {
                            Game1.firesound.Play();
                        }
                        else if (ba.element.Equals(SpellElement.HOLY))
                        {
                            Game1.healSound.Play();

                        }
                        else if (ba.element.Equals(SpellElement.PHYSICAL))
                        {
                            Game1.swordSound.Play();
                        }
                        else
                        {
                            Game1.swordSound.Play();
                        }
                            
                        ent.startFlashing(gameTime);
                    }
                    Active = false;
                }

            }

            // Deactivate the bullet if it goes out of screen
            if (Texture != null)
            {
                if (Position.X + Texture.Width / 2 > 520)
                    Active = false;
                if (Position.X <= 0)
                    Active = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active && Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
                new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
