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
    public class HUD
    {


        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHud;

        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 600;
            screenWidth = 1000;
            playerScoreFont = null;
            playerScorePos = new Vector2(10, 10);
        }
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("myFont");

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (showHud)
            {
                spriteBatch.DrawString(playerScoreFont, "Score  " + playerScore, playerScorePos, Color.Red);
            }
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();


        }

    }
}

