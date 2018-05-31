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
    class Water_Enemy1
    {
        public Vector2 position;
        public Texture2D texture;
        public float speedUp;
        public Vector2 velocity;
        static public Vector2 target;
        Rectangle sourceRect;
        float elapsed;
        float delay = 100f;
        float rotation;
        public static int border=10;
        float rotation_;
        int frames = 0;
        int maxFrames = 9; // tổng số frame
        int row = 3; // số hàng
        int frameWidth;
        int frameHeight;
        Vector2 spriteOrigin;
        public float timeSpan; // thời gian tồn tại của water1


        public Rectangle boundingBox;
        public bool isColliding;
        public Water_Enemy1(Texture2D newtexture,float time,int x=100,int y=100,float _speedUp=0.2f,float _rotation=0f,float startSpeed =0)
        {
            timeSpan = time;
            texture = null;
            speedUp = _speedUp;
            rotation_ = _rotation;
            rotation = 0f;
            position = new Vector2(x, y);
            velocity = new Vector2(0, 0);
            if (startSpeed != 0)
            {
                Vector2 direction = new Vector2((float)Math.Cos(rotation_), (float)Math.Sin(rotation_));
                velocity += direction * startSpeed;
            }
                     
            boundingBox = new Rectangle((int)position.X - 14, (int)position.Y - 12, 28, 24);
            isColliding = false;
            texture = newtexture;
            spriteOrigin = new Vector2(2 * texture.Width / 3 / row, texture.Height / (2 * row));
            frameWidth = texture.Width / row;
            frameHeight = texture.Height / row;
        }
       
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, rotation_, spriteOrigin, 1f, SpriteEffects.None, 0);
        }
        public void Update(GameTime gameTime)
        {
            timeSpan-= (float)gameTime.ElapsedGameTime.TotalMilliseconds; // giảm thời gian tồn tại của water1
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= maxFrames - 1)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }
            sourceRect = new Rectangle((frameWidth * frames) % (frameWidth * (maxFrames / row)), frameHeight * (frames / (maxFrames / row)), frameWidth, frameHeight);

            if (timeSpan > 0) //
            {
                // khi vẫn còn thời gian tồn tại
                // xử lý water1 di chuyển đuổi theo player
                rotation = (float)Math.Atan2(target.Y - position.Y, target.X - position.X);
                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                velocity += direction * speedUp;
                rotation_ = (float)Math.Atan2(velocity.Y, velocity.X);
                position += velocity;
                boundingBox = new Rectangle((int)position.X - 14, (int)position.Y - 12, 28, 24);
                //xử lý không cho water1 di chuyên ra ngoài viền
                if (position.X < border) position.X = border;
                if (position.X > 1000 - border) position.X = 1000 - border;
                if (position.Y < border) position.Y = border;
                if (position.Y > 600 - border) position.Y = 600 - border;
            }
            else
            {
                // khi hết thời gian tồn tại
                // xử lý di chuyển water1 ra khỏi màn hình 
                Vector2 direction = new Vector2((float)Math.Cos(rotation_), (float)Math.Sin(rotation_));
                velocity += direction * speedUp;
                position += velocity;
                boundingBox = new Rectangle((int)position.X - 14, (int)position.Y - 12, 28, 24);
            }
        }
        
    }
}
