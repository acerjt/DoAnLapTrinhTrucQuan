using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace TenGame
{
    class WarningLightning
    {
        public Vector2 position;
        public Texture2D texture;

        float elapsed;
        float delay = 100f;
        public int frames = 0;
        int maxFrames = 3; // tổng số frame
        int row = 3; // số hàng
        int textureWidth;
        int textureHeight;
        public bool isVisible;
        Rectangle sourceRect;
        Vector2 spriteOrigin;

        public WarningLightning(int x=500, int y=300)
        {
            texture = null;   
            position = new Vector2(x, y);
            isVisible = false;
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("warning");
            textureWidth = texture.Width / 3;
            textureHeight = texture.Height / 3;
            spriteOrigin = new Vector2(textureWidth / 6, textureHeight / 2);


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, spriteOrigin, 1f, SpriteEffects.None, 0);
        }
        public void Update(GameTime gameTime)
        {
            if (isVisible)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsed >= delay)
                {
                    if (frames >= maxFrames - 1)
                    {
                        frames = 0;
                        isVisible = false;
                    }
                    else
                        frames++;
                    elapsed = 0;
                }
                sourceRect = new Rectangle((textureWidth * frames) % (textureWidth * (maxFrames / row)), textureHeight * (frames / (maxFrames / row)), textureWidth, textureHeight);
            }
            

           

        }

    }
}
