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
    class Player
    {
        public Vector2 position; // vị trí của player
        public Texture2D texture; // hình player 
        public Color p_color; // màu
        //ability
        public Texture2D manaShieldTexture; // hình của kĩ năng Q khiên
        public Vector2 manaShield_pos=new Vector2(10,530); // vị trí đặt hình
        public Vector2 manaShield_cdPos =new Vector2(10, 500); // vị trí đặt thời gian hồi 
        public Texture2D shieldTexture; // hình của khiên
        public float onCD_elapsed = 0; // biến đếm thời gian hồi
        public float shield_cooldown = 10000f; // thời gian hồi Q
        public float shield_elapsed = 0;
        public bool isShielded = false; // biến kiểm tra shield có đang bật hay không
        public bool shield_onCD = false; // biến kiểm tra skill có đang trong thời gian hồi hay không
        public float shield_range = 70; // tầm hoạt động của khiên
        public float shielded_time = 5000f; // thời gian khiên tồn tại
        public float shielded_elapsed = 0; // biến đếm thời gian khiên tồn tại
        
        // các biến xử lý đồ hoạ của khiên
        int shieldWidth;
        int shieldHeight;
        Vector2 shieldOrigin;
        Rectangle shieldSource;

        //Time Stop 
        public float timestop_CD = 40000f; // thời gian hồi của time stop
        public float timestop_time = 5000f; // thời gian hiệu quả
        public float timestop_CD_elapsed = 0;   // biến đếm
        public float timestop_time_elapsed = 0;// biến đếm
        public float timestop_still_CD_elapsed = 0;// biến đếm
        public bool timestop = false; // biến kiểm tra time stop có đang hiệu quả hay không
        public bool timestop_onCD = false; // biến kiểm tra time stop có đang trong thời gian hồi hay không
        public Vector2 timestop_pos = new Vector2(158, 530); // vị trí đặt hình
        public Vector2 timestop_cdPos = new Vector2(158, 500); // vị trí đặt thời gian hồi 
        public Texture2D timeStopTexture; // icon time stop


        public Texture2D blinkTexture; // hình của skill W tốc biến
        public Vector2 blink_pos=new Vector2(84,530); // vị trí đặt hình
        public Vector2 Blink_cdPos = new Vector2(84, 500); // vị trí đặt thời gian hồi
        public float blink_onCD_elapsed = 0; // biến đếm thời gian hồi
        public float blink_CD = 10000f; // thời gian hồi skill W
        public bool blink_onCD = false; // skill có đang trong thời gian hồi hay không?
        public float blink_still_on_cd_elapsed = 0;
       


        SoundManager sm=new SoundManager();
        public float speed; // tốc độ của player 
        public int border; // player không thể tiếp cận 1 khoảng cách có giá trị = border so với viền
        Rectangle sourceRect;
        float elapsed;
        float delay = 100f; // thời gian giữa 2 frame
        int frames = 0; // biến đếm frame
        int maxFrames = 10; // tổng số frame
        int row = 2; // số hàng
        int edge; // độ dài cạnh của texture
        float rotation; // 
        Vector2 spriteOrigin;
        Vector2 moveTo; // vị trí player di chuyển đến
        bool run;
        public SpriteFont Font;

        // các biến kiểm tra va chạm
        public Rectangle boundingBox;
        public bool isColliding;
       
        public Player()
        {
            texture = null;   
            speed = 9;
            rotation = 0f;
            border = 20; 
            run = false;
            position = new Vector2(500, 300); // vị trí giữa map
            boundingBox = new Rectangle((int)position.X - 15, (int)position.Y - 15, 25, 26);
            Water_Enemy1.target = position;
            isColliding = false;
            Font = null;
            //ability texture
            manaShieldTexture=null;
            blinkTexture=null;
            timeStopTexture = null;

    }
        public void reset() // hàm reset player về giá trị ban đầu
        {
            position.X = 500;
            position.Y = 300;
            run = false;
            boundingBox = new Rectangle((int)position.X - 15, (int)position.Y - 15, 25, 26);
            speed = 9;
            Water_Enemy1.target = position;
            rotation = 0f;
            p_color = Color.White;

            shield_elapsed = 0;
            isShielded = false;
            shield_onCD = false;
            shielded_elapsed = 0;
            onCD_elapsed = 0;

            blink_onCD_elapsed = 0;         
            blink_onCD = false;
            blink_still_on_cd_elapsed = 0;

            timestop_CD_elapsed = 0;
            timestop_time_elapsed = 0;
            timestop_still_CD_elapsed = 0;
            timestop = false;
            timestop_onCD = false;

    }
        public void LoadContent(ContentManager Content)
        {
            sm.LoadContent(Content);
            Font = Content.Load<SpriteFont>("myFont");
            texture = Content.Load<Texture2D>("fireorb");
            shieldTexture = Content.Load<Texture2D>("Shield_");
            manaShieldTexture = Content.Load<Texture2D>("ManaShield");
            timeStopTexture = Content.Load<Texture2D>("TimeStop");
            blinkTexture = Content.Load<Texture2D>("Blink");
            spriteOrigin = new Vector2(texture.Height/(2*row), texture.Height/(2*row));
            edge = texture.Height / row;
            shieldHeight = shieldTexture.Height;
            shieldWidth = shieldTexture.Width;
            shieldOrigin = new Vector2(shieldWidth, shieldHeight / 2);
            shieldSource = new Rectangle(0, 0, shieldWidth, shieldHeight);


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Vẽ hình shield khi shield bị phá vỡ
            if (!isShielded && shielded_elapsed != 0)
            {
                sm.SpellShieldImpact.Play(0.6f,0f,0f);
                shielded_elapsed = 0f;
                spriteBatch.Draw(shieldTexture, position + new Vector2(shield_range - 10,0), shieldSource, Color.White, 0f, shieldOrigin, 1f, SpriteEffects.None, 0);
            }
            //Vẽ Mana Shield Icon
            if (isShielded)
            {
                spriteBatch.Draw(manaShieldTexture, manaShield_pos, Color.Gray);
                spriteBatch.DrawString(Font, ((shielded_time - shielded_elapsed) / 1000f).ToString("0.0"), manaShield_cdPos, Color.Red);
            }
            else
                    if (shield_onCD)
                    {
                         spriteBatch.Draw(manaShieldTexture, manaShield_pos, Color.Gray);
                         spriteBatch.DrawString(Font, ((shield_cooldown - shield_elapsed) / 1000f).ToString("0.0"), manaShield_cdPos, Color.Teal);
                    }
            else spriteBatch.Draw(manaShieldTexture, manaShield_pos, Color.White);
            // Vẽ Blink Icon
            if (blink_onCD)
            {
                spriteBatch.Draw(blinkTexture, blink_pos, Color.Gray);
                spriteBatch.DrawString(Font, ((blink_CD - blink_onCD_elapsed) / 1000f).ToString("0.0"), Blink_cdPos, Color.Teal);
            }
            else spriteBatch.Draw(blinkTexture, blink_pos, Color.White);
            // Vẽ Time Stop Icon
            if (timestop)
            {
                spriteBatch.Draw(timeStopTexture, timestop_pos, Color.Gray);
                spriteBatch.DrawString(Font, ((timestop_time - timestop_time_elapsed) / 1000f).ToString("0.0"), timestop_cdPos, Color.Red);
            }
            else
                    if (timestop_onCD)
            {
                spriteBatch.Draw(timeStopTexture, timestop_pos, Color.Gray);
                spriteBatch.DrawString(Font, ((timestop_CD - timestop_CD_elapsed) / 1000f).ToString("0.0"), timestop_cdPos, Color.Teal);
            }
            else spriteBatch.Draw(timeStopTexture, timestop_pos, Color.White);
            //  VẼ Player
            spriteBatch.Draw(texture, position, sourceRect, p_color, rotation, spriteOrigin, 1f, SpriteEffects.None, 0);
        }
        public void Update(GameTime gameTime)
        {
            // xử lý frame hiện tại của player
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= maxFrames-1)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }
            sourceRect = new Rectangle((edge * frames)%(edge * (maxFrames/row)), edge * (frames/(maxFrames/row)), edge, edge);



            // Set moveTo = vị trí click chuột trái
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                
                    if (Vector2.Distance(position, mousePosition) > 5)
                    {
                        moveTo = mousePosition;
                        rotation = (float)Math.Atan2(moveTo.Y - position.Y, moveTo.X - position.X);
                        delay = 20f;
                        run = true;
                    }
                            
            }
            //Xử lý khi player nhấn Q sử dụng shield
            var  keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                if (!shield_onCD)
                {
                    sm.Shield.Play(0.2f, 0f, 0f);
                    shield_onCD = true;
                    isShielded = true;
                    shielded_elapsed = 0;
                    onCD_elapsed = 0;
                }
                else
                {
                    if (onCD_elapsed > sm.onCD.Duration.TotalMilliseconds)
                    {
                        onCD_elapsed = 0;
                        sm.onCD.Play();
                    }
                }
            }
            // Xử lý khi player nhấn W sử dụng tốc biến
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (!blink_onCD)
                {
                    var blink_pos = new Vector2(mouseState.X, mouseState.Y);

                    if (blink_pos.X < border) blink_pos.X = border;
                    if (blink_pos.X > 1000 - border) blink_pos.X = 1000 - border;
                    if (blink_pos.Y < border) blink_pos.Y = border;
                    if (blink_pos.Y > 600 - border) blink_pos.Y = 600 - border;

                    blink_still_on_cd_elapsed = 0;
                    blink_onCD = true;
                    blink_onCD_elapsed = 0f;
                    position = blink_pos;
                    boundingBox = new Rectangle((int)position.X - 15, (int)position.Y - 15, 25, 26);
                    sm.Blink.Play();
                    
                }
                else
                {
                    if (blink_still_on_cd_elapsed > sm.onCD.Duration.TotalMilliseconds)
                    {
                        blink_still_on_cd_elapsed = 0;
                        sm.onCD.Play();
                    }
                }
            }
            //Xử lý khi sử dụng timestop
            if (keyboardState.IsKeyDown(Keys.E))
            {
                if (!timestop_onCD)
                {
                    timestop_onCD = true;
                    timestop = true;
                    timestop_CD_elapsed = 0f;
                    timestop_time_elapsed = 0f;
                    sm.TimeStop.Play();
                }
                else
                {
                    if (timestop_still_CD_elapsed > sm.onCD.Duration.TotalMilliseconds)
                    {
                        timestop_still_CD_elapsed = 0;
                        sm.onCD.Play();
                    }
                }
            }

            //đếm thời gian timestop hồi
            if (timestop_onCD)
            {
                timestop_still_CD_elapsed+= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (!timestop)
                {
                    timestop_CD_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timestop_CD_elapsed > timestop_CD)
                    {
                        timestop_onCD = false;
                        timestop_CD_elapsed = 0;
                    }
                }
            }
            //Đếm thời gian timestop hết tác dụng
            if (timestop)
            {
                timestop_time_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timestop_time_elapsed > timestop_time)
                {
                    timestop_time_elapsed = 0;
                    timestop = false;
                }
            }
            //đếm thời gian skill W hồi
            if (blink_onCD)
            {
                blink_still_on_cd_elapsed+= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                blink_onCD_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (blink_onCD_elapsed > blink_CD)
                {
                    blink_onCD_elapsed=0;
                    blink_onCD = false;
                }
            }
            //đếm thời gian khi skill Q hết tác dụng
            if (isShielded)
            {
                shielded_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (shielded_elapsed > shielded_time)
                {
                    shielded_elapsed=0;
                    isShielded = false;
                }
            }
            // đém thời gian skill Q hồi
            if (shield_onCD)
            {
                onCD_elapsed+= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (!isShielded)
                {
                    shield_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (shield_elapsed > shield_cooldown)
                    {
                        shield_elapsed = 0;
                        shield_onCD = false;
                    }
                }
            }

            //Di chuyển nhân vật đến moveTo      
            if (run)
            {
                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                position += direction * speed;
                if (position.X < border) position.X = border;
                if (position.X > 1000 - border) position.X =1000 - border;
                if (position.Y < border) position.Y = border;
                if (position.Y > 600 - border) position.Y = 600 - border;
                boundingBox = new Rectangle((int)position.X - 15, (int)position.Y - 15, 25, 26);
                Water_Enemy1.target = position;
                if (Vector2.Distance(position, moveTo) < 5)
                {
                    run = false;
                    delay = 100f;
                }
                    
            }


        }

    }
}
