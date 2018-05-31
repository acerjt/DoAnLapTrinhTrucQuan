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
    class Water_Orb
    {
        public static int c_range = 300;
        public static float speed = 2f;
        public static float rotationSpeed = 0.01f;
        public static int endPhase = 5;

        public Vector2 position;
        public Texture2D texture;
        
        Rectangle sourceRect;
        float elapsed;
        float delay = 100f;
        int frames = 0;
        int maxFrames = 8; // tổng số frame
        int row = 2; // số hàng
        int edge;
        float angle;
        Vector2 spriteOrigin;
        public int phase;
        public Vector2 source;
        float sourceRotation;

        float phase_elapsed = 0;
        float phase0_waiting_time = 2000f;
        float phase1_waiting_time = 2000f;
        float phase2_waiting_time = 6000f;
        float phase3_waiting_time = 3000f;

        public Rectangle boundingBox;
        public bool isColliding;

        public Water_Orb(Texture2D newtexture, float x = 100, float y = 100,float _angle=0, int a = 500, int b = 300)
        {
            texture = null;
            position = new Vector2(x, y);
            boundingBox = new Rectangle((int)position.X - 23, (int)position.Y - 23, 40, 43);
            source = new Vector2(a, b);
            angle = _angle;
            sourceRotation = _angle;//(float)Math.Atan2(position.Y- source.Y, position.X- source.X);
            phase = 0;
            isColliding = false;
            texture = newtexture;
            spriteOrigin = new Vector2(38,50);
            edge = texture.Height / row;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0, spriteOrigin, 1f, SpriteEffects.None, 0);
        }
        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= maxFrames - 1)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }
            sourceRect = new Rectangle((edge * frames) % (edge * (maxFrames / row)), edge * (frames / (maxFrames / row)), edge, edge);
            Vector2 direction;
            float dist;
            switch (phase)
            {
                case 0: // khi ở phase 0, các water_orb di chuyển vào tâm
                    
                    if (Vector2.Distance(position, source) < 5)
                    {
                        position = source;
                        phase_elapsed += (float)gameTime.ElapsedGameTime.Milliseconds;
                        if (phase_elapsed>phase0_waiting_time)
                        {
                            phase_elapsed = 0;
                            phase = 1;
                        }
                    }
                    else
                    {
                        float rotation = (float)Math.Atan2(source.Y - position.Y, source.X - position.X);
                        direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                        position += direction * speed;
                    }
                    break;
                case 1: // khi ở phase 1, các water_orb di chuyển ra bên ngoài theo hình xoắn ốc
                    
                    if (Vector2.Distance(position, source) >=c_range)
                    {
                        direction = new Vector2((float)Math.Cos(sourceRotation), (float)Math.Sin(sourceRotation));
                        position = source + c_range * direction;
                        phase_elapsed += (float)gameTime.ElapsedGameTime.Milliseconds;
                        if (phase_elapsed > phase1_waiting_time)
                        {
                            phase_elapsed = 0;
                            phase = 2;
                        }
                    }
                    else
                    {
                        sourceRotation += rotationSpeed;
                        dist = Vector2.Distance(position, source);
                        dist += speed;
                        direction = new Vector2((float)Math.Cos(sourceRotation), (float)Math.Sin(sourceRotation));
                        position = source + direction * dist;
                    }
                    break;
                case 2: // khi ở phase 2, các water_orb di chuyển xung quanh tâm
                    sourceRotation += rotationSpeed;
                    direction = new Vector2((float)Math.Cos(sourceRotation), (float)Math.Sin(sourceRotation));
                    position = source + direction * c_range;
                    phase_elapsed += (float)gameTime.ElapsedGameTime.Milliseconds;
                    if (phase_elapsed > phase2_waiting_time)
                    {
                        phase_elapsed = 0;
                        phase = 3;
                    }
                    break;
                case 3: // khi ở phase 3, các water_orb di chuyển vào tâm theo hình xoắn ốc
                  
                    dist = Vector2.Distance(position, source);
                    dist -= speed;
                    sourceRotation +=rotationSpeed;
                    direction = new Vector2((float)Math.Cos(sourceRotation), (float)Math.Sin(sourceRotation));
                    position = source+direction * dist;
                    if (Vector2.Distance(position, source) < 6)
                    {
                        position = source;
                        phase_elapsed += (float)gameTime.ElapsedGameTime.Milliseconds;
                        if (phase_elapsed > phase3_waiting_time)
                        {
                            phase_elapsed = 0;
                            phase = 4;// end
                        }                       
                    }
                    break;
                case 4: // khi ở phase 4, các orb di chuyển ra ngoài với tốc độ nhanh
                    direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    position+= direction * speed*3;
                    if (position.X < -50 || position.X > 1050 || position.Y < -50 || position.Y > 650)
                        phase = endPhase;
                    break;
            }
            boundingBox = new Rectangle((int)position.X - 23, (int)position.Y - 23, 40, 43);

        }

    }
}
