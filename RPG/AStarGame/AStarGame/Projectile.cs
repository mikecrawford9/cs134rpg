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

        bool isLeft;

        public void Initialize( Texture2D texture, Vector2 position, bool isLeft)
        {
            Texture = texture;
            Position = position;

            Active = true;

            projectileMoveSpeed = 32;
            this.isLeft = isLeft;
        }

        public void Update()
        {
            // Projectiles always move to the right
            if (isLeft)
            {
                Position.X -= projectileMoveSpeed;
            }
            else
            {
                Position.X += projectileMoveSpeed;
            }

            // Deactivate the bullet if it goes out of screen
            if (Position.X + Texture.Width / 2 > 520)
                Active = false;
            if (Position.X <= 0)
                Active = false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
            new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
