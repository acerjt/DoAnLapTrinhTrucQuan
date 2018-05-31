using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TenGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum State
        {
            Menu,
            Playing,
            Gameover
        }
        bool f_menu=true;
        bool f_gameover = true;

        float waterloop_elapsed = 0;
        float waterloop_dur;
        //Aoe Slow
        float water_slow_elapsed;
        float water_slow_spawn_time;
        float water_slow_time;
        float water_slow_time_elapsed;
        float water_slow_waiting_elapsed;
        float water_slow_speed;
        int water_slow_score;
        
        //
        Texture2D shieldTexture;
        int shieldWidth;
        int shieldHeight;
        Vector2 shieldOrigin;
        Rectangle shieldSource;
        Texture2D menuImage;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player p;
        List<Water_Enemy1> list_water1 = new List<Water_Enemy1>();
        List<Water_Orb> list_water_orb = new List<Water_Orb>();
        Texture2D water1_texture;
        Texture2D water_orb_texture;
        WarningLightning lightning;
        SoundManager sm = new SoundManager();
        float game_elapsed_time = 0;
        float _game = 40000f;
        float water1_spawn_time = 3000f;
        float water1_elapsed = 0;
        float water1_timeSpan = 7000f;
        float water1_speed = 0.15f;

        float water_orb_spawn_time = 25000f;
        float water_orb_elapsed = 0;
        int n_orb = 10;// số lượng orb 1 wave
        int max_range = 550;

        float water_ball_wave_rotation_speed;
        float water_ball_wave_speed;
        float water_ball_wave = 10000f;
        float water_ball_wave_elapsed = 0;
        float n_water_ball = 50;
        float water_ball_fire_rotation = 0f;
        float water_ball_delay = 100f;
        float water_ball_delay_elapse;
        float water_ball_num = 0;
        float warning_waiting = 4000f;
        float warning_elapsed = 0f;
        Vector2 water_ball_source;
        HUD hud = new HUD();

        State gameState = State.Menu;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            menuImage = null;


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {   
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            Window.Title = "Keep da Fire";
            p = new Player();
            lightning = new WarningLightning();
            reset();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p.LoadContent(Content);
            lightning.LoadContent(Content);
            hud.LoadContent(Content);
            sm.LoadContent(Content);
            waterloop_dur = (float)sm.waterloop.Duration.TotalMilliseconds;
            //shield
            shieldTexture = Content.Load<Texture2D>("Shield_");
            shieldHeight = shieldTexture.Height;
            shieldWidth = shieldTexture.Width;
            shieldOrigin = new Vector2(shieldWidth, shieldHeight / 2);
            shieldSource = new Rectangle(0, 0, shieldWidth, shieldHeight);
            //--
            water1_texture = Content.Load<Texture2D>("water_enemy1");
            water_orb_texture = Content.Load<Texture2D>("water_orb");
            menuImage = Content.Load<Texture2D>("StartMenu");
            MediaPlayer.Volume = 0.07f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(sm.bgMusic);
            
            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (gameState)
            {
                case State.Playing:
                    {
                        
                        int i;
                        // tạo ra hiệu ứng các water_orb xoay vòng mỗi water_orb_spawn_time
                        water_orb_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (water_orb_elapsed > water_orb_spawn_time)
                        {
                            sm.Play(sm.OrbStart);
                            //sm.OrbStart.Play();
                            hud.playerScore += 10;
                            water_orb_elapsed = 0f;
                            for (i = 0; i < n_orb; i++)
                            {
                                float orb_rotation = i * ((float)Math.PI * 2 / n_orb);
                                Vector2 direction = new Vector2((float)Math.Cos(orb_rotation), (float)Math.Sin(orb_rotation));
                                Vector2 spawn_point = new Vector2(500, 300);
                                spawn_point += direction * max_range;
                                Water_Orb w_orb = new Water_Orb(water_orb_texture, spawn_point.X, spawn_point.Y, orb_rotation);
                                list_water_orb.Add(w_orb);
                            }
                        }

                        // tăng độ khó của game mỗi _game/1000 giây
                        game_elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (game_elapsed_time > _game) 
                        {
                            sm.Play(sm._game_increased);
                            //sm._game_increased.Play(); 
                            hud.playerScore += 10; // 
                            game_elapsed_time = 0; 
                            water1_speed += 0.02f; //
                            
                            water_ball_wave_rotation_speed = 2 * (float)Math.PI / (40-n_orb); 
                            water1_spawn_time -= 100f; // 
                            water_ball_delay -= 3f; 
                            water_ball_wave = -500f;
                            water_ball_wave_speed += 0.2f;
                            warning_waiting -= 500f;
                            //water orb
                            Water_Orb.speed += 0.2f;
                            Water_Orb.rotationSpeed +=0.002f;
                            water_orb_spawn_time -= 1000f;
                            n_orb++; // 
                            //hiệu ứng làm chậm
                            water_slow_spawn_time -= 500f;
                            water_slow_speed += 0.1f;
                            
                        }
                        Random rnd = new Random();



                        // TODO: Add your update logic here
                        p.Update(gameTime);

                        // tạo ra các water1
                        water1_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (water1_elapsed >= water1_spawn_time)
                        {
                            hud.playerScore += 2;
                            int x;
                            if (rnd.Next(2) == 0)
                                x = -50;
                            else
                                x = 650;
                            Water_Enemy1 w = new Water_Enemy1(water1_texture, water1_timeSpan, 1000 / rnd.Next(1, 10), x, water1_speed);

                            list_water1.Add(w);
                            water1_elapsed = 0;
                        }

                        for (i = 0; i < list_water1.Count(); i++)
                        {
                            if (p.boundingBox.Intersects(list_water1[i].boundingBox))     // kiểm tra va chạm của water1 so với player                           
                                    gameState = State.Gameover;
                            else
                            if (list_water1[i].timeSpan > 0) // kiểm tra thời gian tồn tại của các water1
                                list_water1[i].Update(gameTime);

                            else
                            {
                                list_water1[i].Update(gameTime);
                                if (list_water1[i].position.X < -50 || list_water1[i].position.Y < -50)
                                {
                                    list_water1.Remove(list_water1[i--]);
                                    
                                }

                            }
                        }
                        // kiểm tra va chạm của các water_orb
                        for (i = 0; i < list_water_orb.Count(); i++)
                        {
                            list_water_orb[i].Update(gameTime);
                            if (p.boundingBox.Intersects(list_water_orb[i].boundingBox))
                            {
                                if (p.isShielded) // kiểm tra nếu player có khiên hay không
                                {
                                    list_water_orb.RemoveAt(i--); // làm mất water orb
                                    p.isShielded = false; // phá vỡ khiên
                                }
                                else
                                gameState = State.Gameover; // nếu không thì game over
                            }
                            else
                            if (list_water_orb[i].phase == Water_Orb.endPhase) // xoá water_orb khi vào phase cuối
                            {
                                list_water_orb.Remove(list_water_orb[i]);
                                i--;
                            }
                        }
                        waterloop_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


                        if (waterloop_elapsed > waterloop_dur)
                        {
                            sm.waterloop.Play(0.1f,0f,0f);
                            waterloop_elapsed = 0;
                        }

                        sm.Update(gameTime);
                        WaterBallWave(gameTime);
                        lightning.Update(gameTime);
                        WaterSlow(gameTime);
                        //hud.Update(gameTime);
                       
                        break;
                    }
                case State.Menu:
                    {
                        if (f_menu)
                        {
                            sm.MenuSound.Play();
                            
                            f_menu = false;
                        }                 
                        KeyboardState keyState = Keyboard.GetState();
                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            sm.Play(sm.gameStart);
                            //sm.gameStart.Play();
                            gameState = State.Playing;
                            f_menu = true;
                        }
                        break;
                    }
                case State.Gameover:
                    {
                        KeyboardState keyState = Keyboard.GetState();
                        if (f_gameover) sm.Done.Play();
                        f_gameover = false;
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            reset();
                            f_gameover = true;
                            gameState = State.Menu;    
                        }
                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            reset();
                            f_gameover = true;
                            sm.gameStart.Play();
                            gameState = State.Playing;
                        }

                        break;
                    }
            }

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch (gameState)
            {
                case State.Playing:
                   
                    p.Draw(spriteBatch);
                    foreach (Water_Enemy1 w in list_water1)
                        w.Draw(spriteBatch);
                    foreach (Water_Orb w_orb in list_water_orb)
                        w_orb.Draw(spriteBatch);
                    if (p.isShielded)
                    {
                        foreach (Water_Enemy1 w in list_water1)
                        {
                            if (Vector2.Distance(w.position, p.position) <= p.shield_range) // kiểm tra nếu water1 trong tầm hoạt động của khiên
                            {
                                float rotation = (float)Math.Atan2(w.position.Y - p.position.Y, w.position.X - p.position.X);
                                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                                w.position = p.position + direction * (p.shield_range + 1); // đẩy water1 ra khỏi tầm hoạt động của khiên
                                spriteBatch.Draw(shieldTexture, p.position + direction * (p.shield_range-10), shieldSource, Color.White, rotation, shieldOrigin, 1f, SpriteEffects.None, 0);
                                sm.AntiMagic.Play(0.02f,0f,0f);
                            }
                        }
                    }
                    

                    lightning.Draw(spriteBatch);
                    hud.Draw(spriteBatch);
                    break;
                case State.Menu:
                    spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                    break;
                case State.Gameover:
                    spriteBatch.DrawString(hud.playerScoreFont, "Your Score: " + hud.playerScore, new Vector2(400, 250), Color.Red);
                    spriteBatch.DrawString(hud.playerScoreFont, "Menu: Escape", new Vector2(400, 300), Color.Red);
                    spriteBatch.DrawString(hud.playerScoreFont, "Play Again: Enter", new Vector2(400, 350), Color.Red);
                    break;
            }
            
            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
        protected void WaterBallWave(GameTime gameTime) // hiệu ứng bắn water1 xoay vòng
        {
            if (water_ball_num != 0)
            {
                water_ball_delay_elapse += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (water_ball_delay_elapse > water_ball_delay)
                {
                 
                    water_ball_delay_elapse = 0f;
                    Water_Enemy1 w = new Water_Enemy1(water1_texture, 0f, (int)water_ball_source.X, (int)water_ball_source.Y, 0f, water_ball_fire_rotation, water_ball_wave_speed);
                    list_water1.Add(w);
                    water_ball_num++;
                    water_ball_fire_rotation = water_ball_fire_rotation + water_ball_wave_rotation_speed;
                    if (water_ball_num == n_water_ball + 1)
                    {
                        water_ball_num = 0;
                        hud.playerScore += 8;
                    }
                }
            }
            else
            {
                water_ball_wave_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
               if (water_ball_wave_elapsed > water_ball_wave)
                {
                    if (warning_elapsed == 0)
                    {
                        sm.LightningSound.Play(0.2f,0f,0f);                       
                        Random rnd = new Random();
                        water_ball_source = new Vector2(rnd.Next(100, 900), rnd.Next(50, 550));
                        lightning.position = water_ball_source;
                        lightning.frames = 0;
                        lightning.isVisible = true;
                    }
                    warning_elapsed+= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (warning_elapsed > warning_waiting)
                     {
                        warning_elapsed = 0f;
                        water_ball_wave_elapsed = 0f;
                        water_ball_fire_rotation = 0.00f;
                        water_ball_num = 1;
                        //sm.ShootStart.Play();
                        sm.Play(sm.ShootStart);

                    }
                }
            }
            
        }
        protected void WaterSlow(GameTime gameTime) // hiệu ứng làm chậm
        {
            water_slow_elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (water_slow_elapsed > water_slow_spawn_time)
            {
                water_slow_elapsed = 0;
                water_slow_waiting_elapsed = 1f;
                sm.Play(sm.WaterSlow);
            }
            if (water_slow_waiting_elapsed != 0)
            {
                water_slow_waiting_elapsed+= (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
                if (water_slow_waiting_elapsed > sm.WaterSlow.Duration.TotalMilliseconds)
                {
                    water_slow_waiting_elapsed = 0f;
                    if (p.isShielded)
                        p.isShielded = false;
                    else
                    {
                        p.speed -= water_slow_speed;
                        p.p_color = Color.Blue;
                        water_slow_time_elapsed = 1f;
                        sm.SlowTri.Play();
                        
                    }
                }
            }
            if (water_slow_time_elapsed!=0)
            {
                water_slow_time_elapsed+= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (water_slow_time_elapsed > water_slow_time)
                {
                    p.p_color = Color.White;
                    water_slow_time_elapsed = 0;
                    p.speed += water_slow_speed;

                }
            }
            
        }
        protected void reset()
        {
            int i;
            
             game_elapsed_time = 0;
             _game = 40000f;
             water1_spawn_time = 4000f;
             water1_elapsed = 0;
             water1_timeSpan = 7000f;
             water1_speed = 0.15f;

             water_orb_spawn_time = 25000f;
             water_orb_elapsed = 0;
             n_orb = 10;// số lượng orb 1 wave
             max_range = 550;

             water_slow_elapsed=15000f;
             water_slow_spawn_time=20000f;
             water_slow_time=4000f;
             water_slow_time_elapsed=0f;
             water_slow_speed = 7;
             water_slow_waiting_elapsed = 0f;
             water_slow_score =5;

            water_ball_wave_speed =4f;
            water_ball_wave = 10000f;
             water_ball_wave_elapsed = 0;
             n_water_ball = 20;
             water_ball_fire_rotation = 0f;
            water_ball_wave_rotation_speed=2*(float)Math.PI/20;
             water_ball_delay = 100f;
             water_ball_delay_elapse=0;
             water_ball_num = 0;
             warning_waiting = 4000f;
             warning_elapsed = 0f;
             hud.playerScore = 0;
            Water_Orb.speed =2f;
            Water_Orb.rotationSpeed =0.01f;
            sm.isPlaying = false;
            for (i = 0; i < list_water1.Count(); i++)
                list_water1.Remove(list_water1[i--]);
            for (i = 0; i < list_water_orb.Count(); i++)
                list_water_orb.Remove(list_water_orb[i--]);
            sm.elapsed = 0;
            for (i = 0; i < sm.list_sound.Count(); i++)
                sm.list_sound.Remove(sm.list_sound[i--]);
            
            p.reset();   
        }
    }
}
