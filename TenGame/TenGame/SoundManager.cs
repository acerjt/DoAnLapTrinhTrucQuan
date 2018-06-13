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
//using Microsoft.Xna.Framework.Audio;

namespace TenGame
{
    class SoundManager     
    {
        public SoundEffect MenuSound; // Sound khi vào menu
        public SoundEffect gameStart; // sound khi vào trò chơi
        public SoundEffect _game_increased; // sound khi độ khó game tăng
        public SoundEffect LightningSound; // sound sấm sét
        public SoundEffect ShootStart; // sound khi hiệu ứng bắn vòng
        public SoundEffect Done; // sound khi game over
        public SoundEffect OrbStart; // sound khi các cục nước xuất hiện
        public SoundEffect Shield; // sound của skill khiên
        public SoundEffect onCD; // sound khi xài skill chưa hồi
        public SoundEffect waterloop; //
        public SoundEffect WaterSlow; // sound của hiệu ứng làm chậm
        public SoundEffect SlowTri; 
        public SoundEffect AntiMagic; // sound khi các cục nước chạm vào khiên
        public SoundEffect SpellShieldImpact; // sound khi khiên bị phá vỡ
        public SoundEffect Blink; // sound khi sử dụng tốc biến
        public SoundEffect TimeStop; // sound khi sử dụng time stop
        public float elapsed = 0;
        public Song bgMusic; //
        

        public bool isPlaying = false;
        public List<SoundEffect> list_sound = new List<SoundEffect>();

        public void Update(GameTime gameTime)
        {
            if (isPlaying)
            {
                if (elapsed == 0)               
                    list_sound[0].Play();
                
                if (elapsed > list_sound[0].Duration.TotalMilliseconds)
                {
                    elapsed = 0;
                    list_sound.RemoveAt(0);
                    if (list_sound.Count==0)
                    isPlaying = false;
                }
                else
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }


        }
        public void Play(SoundEffect s)
        {
            list_sound.Add(s);
            isPlaying = true;  
        }
        public SoundManager()
        {
            MenuSound = null;
            bgMusic = null;
            _game_increased = null;
            gameStart = null;
            LightningSound = null;
            ShootStart = null;
            Done = null;
            OrbStart = null;
            Shield = null;
            onCD = null;
            waterloop = null;
            WaterSlow = null;
            SlowTri = null;
            AntiMagic = null;
            SpellShieldImpact = null;
            Blink = null;
            TimeStop = null;
        }
        public void LoadContent(ContentManager Content)
        {
            MenuSound = Content.Load<SoundEffect>("MenuSound");
            bgMusic = Content.Load<Song>("NagaTheme");
            _game_increased = Content.Load<SoundEffect>("_game_increased");
            gameStart = Content.Load<SoundEffect>("gameStart");
            LightningSound = Content.Load<SoundEffect>("LightningSound");
            ShootStart = Content.Load<SoundEffect>("ShootStart");
            Done= Content.Load<SoundEffect>("Done");
            OrbStart=Content.Load<SoundEffect>("OrbStart");
            Shield = Content.Load<SoundEffect>("Shield");
            onCD = Content.Load<SoundEffect>("onCD");
            waterloop= Content.Load<SoundEffect>("WaterLakeLoop1");
            WaterSlow= Content.Load<SoundEffect>("WaterSlow");
            SlowTri = Content.Load<SoundEffect>("SlowTri");
            AntiMagic = Content.Load<SoundEffect>("AntiMagic");
            SpellShieldImpact = Content.Load<SoundEffect>("SpellShieldImpact");
            Blink = Content.Load<SoundEffect>("BlinkSound");
            TimeStop = Content.Load<SoundEffect>("TimeStopSound");
        }
    }
}
