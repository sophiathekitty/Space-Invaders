using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class InvaderGame : Screen
        {
            public static int Score { get { return score; } set { score = value; if (highScore < score) { highScore = score; GridInfo.SetVar("HighScore",highScore.ToString()); } } }
            static int score = 0;
            static int highScore = 0;
            int lives = 3;
            public static float player_speed = 3;
            public static float enemy_speed = 0.5f;
            public static bool game_over = false;
            float bullet_speed = 6f;
            GameInput input;
            Player player;
            Bullet playerBullet;
            Bullet EnemyBullet;
            List<DestructableSprite> barrier = new List<DestructableSprite>();
            AlienWave wave;
            ScreenSprite scoreDisplay;
            ScreenSprite highScoreDisplay;
            ScreenSprite livesDisplay;
            float fontSize = 0.5f;
            // game over display (when player is present and dead)
            ScreenSprite gameOverDisplay;
            ScreenSprite continueControls;
            // title display (when player not present)
            ScreenSprite title;
            ScreenSprite gameControls;
            Random rand = new Random();
            //------------------------------------------------------------------
            // constructor
            //------------------------------------------------------------------
            public InvaderGame(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                BackgroundColor = new Color(0, 0, 100);
                // initialize game
                GridInfo.Echo("Initializing Game");
                highScore = GridInfo.GetVarAs("HighScore", highScore);
                input = new GameInput(GridBlocks.GetPlayer());
                // create player
                player = new Player(new Vector2(Size.X/2,Size.Y*0.9f), RasterSprite.DEFAULT_PIXEL_SCALE, SpriteLibrary.Sprites["player"], SpriteLibrary.Sprites["playerDeath"][0], input);
                AddSprite(player);
                // player bullet
                playerBullet = new Bullet(new Vector2(0, 0), RasterSprite.DEFAULT_PIXEL_SCALE, SpriteLibrary.Sprites["playerBullet"], SpriteLibrary.Sprites["explosion"][0], new Vector2(0, -bullet_speed));
                AddSprite(playerBullet);
                playerBullet.Color = Color.LightGreen;
                playerBullet.IsDead = true;
                playerBullet.Visible = false;
                // create enemy bullet
                EnemyBullet = new Bullet(new Vector2(0, 0), RasterSprite.DEFAULT_PIXEL_SCALE, SpriteLibrary.Sprites["alienBullet"], SpriteLibrary.Sprites["explosion"][0], new Vector2(0, bullet_speed));
                AddSprite(EnemyBullet);
                EnemyBullet.IsDead = true;
                EnemyBullet.Visible = false;
                // create barriars
                for(int i = 0; i < 4; i++)
                {
                    barrier.Add(new DestructableSprite(new Vector2(Size.X * 0.1f + i * Size.X * 0.2f, Size.Y * 0.8f), RasterSprite.DEFAULT_PIXEL_SCALE, new Vector2(24, 16), SpriteLibrary.Sprites["barrier"][0]));
                    AddSprite(barrier[i]);
                }
                // create alien wave
                wave = new AlienWave(new Vector2(Size.X * 0.02f, Size.Y * 0.1f), RasterSprite.DEFAULT_PIXEL_SCALE, new Vector2(16, 8), Viewport);
                AddSprite(wave);
                // create score display
                scoreDisplay = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, new Vector2(0, 0), fontSize, Vector2.Zero,Color.White,"Monospace","Score: 0",TextAlignment.LEFT,SpriteType.TEXT);
                AddSprite(scoreDisplay);
                // create high score display
                highScoreDisplay = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter, new Vector2(0, 0), fontSize, Vector2.Zero, Color.White, "Monospace", "High Score: 0", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(highScoreDisplay);
                // create lives display
                livesDisplay = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopRight, new Vector2(0, 0), fontSize, Vector2.Zero, Color.White, "Monospace", "Lives: 3", TextAlignment.RIGHT, SpriteType.TEXT);
                AddSprite(livesDisplay);
                // create game over display
                gameOverDisplay = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(0, -50), fontSize*2.5f, Vector2.Zero, Color.White, "Monospace", "Game Over", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(gameOverDisplay);
                // create continue controls
                continueControls = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(0, 10), fontSize*1.5f, Vector2.Zero, Color.White, "Monospace", "Press C to Continue", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(continueControls);
                gameOverDisplay.Visible = false;
                continueControls.Visible = false;
                // create title display
                title = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(0, -50), fontSize*2.5f, Vector2.Zero, Color.White, "Monospace", "Space Invaders", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(title);
                // create game controls
                gameControls = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.Center, new Vector2(0, 10), fontSize*1.5f, Vector2.Zero, Color.White, "Monospace", "A and W to Move\nSpace to Shoot", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(gameControls);
            }
            //------------------------------------------------------------------
            // main function to handle arguments
            //------------------------------------------------------------------
            override public void Main(string argument)
            {
                // handle arguments
                if(argument == "reset")
                {
                    highScore = 0;
                    GridInfo.SetVar("HighScore", "0");
                }
                else base.Main(argument);
            }
            //------------------------------------------------------------------
            // update function to handle game logic
            //------------------------------------------------------------------
            bool playerPresent = false;
            public override void Update()
            {
                // update score
                scoreDisplay.Data = "Score: " + score.ToString();
                if(score > highScore) { highScore = score; GridInfo.SetVar("HighScore", highScore.ToString()); }
                highScoreDisplay.Data = "High Score: " + highScore.ToString();
                livesDisplay.Data = "Lives: " + lives.ToString();
                // check for player
                if (input.PlayerPresent)
                {
                    if (!playerPresent)
                    {
                        playerPresent = true;
                        Reset();
                    }
                    title.Visible = false;
                    gameControls.Visible = false;
                } 
                else
                {
                    if(playerPresent) Reset();
                    playerPresent = false;
                    title.Visible = true;
                    gameControls.Visible = true;
                    return;
                }
                // check for game over
                if(game_over)
                {
                    gameOverDisplay.Visible = true;
                    continueControls.Visible = true;
                    // game over logic goes here
                    if(input.C)
                    {
                        Reset();
                    }
                    return;
                }
                // game logic goes here
                player.Update();
                playerBullet.Update();
                EnemyBullet.Update();
                wave.Update();
                // check for enemy bullet
                if(EnemyBullet.Visible == false)
                {
                    // shoot
                    List<Vector2> origins = wave.BulletPoints;
                    if(origins.Count > 0)
                    {
                        Vector2 origin = origins[rand.Next(origins.Count)];
                        EnemyBullet.Position = origin;
                        EnemyBullet.Visible = true;
                        EnemyBullet.IsDead = false;
                        GridInfo.Echo("Enemy Bullet: " + EnemyBullet.Position.ToString());
                    } 
                    else
                    {
                        // wave dead
                        wave.NextWave();
                        foreach(DestructableSprite b in barrier)
                        {
                            b.Reset();
                        }
                        score += 100;
                    }
                } else if (EnemyBullet.IsDead) EnemyBullet.Visible = false;
                if (player.IsDead && lives > 0)
                {
                    player.IsDead = false;
                    player.Position = new Vector2(Size.X / 2, Size.Y * 0.9f);
                    if (EnemyBullet.Visible && EnemyBullet.IsDead)
                    {
                        EnemyBullet.Visible = false;
                    }
                }
                // check for player bullet
                if (input.Space && !playerBullet.Visible)
                {
                    // shoot
                    playerBullet.Position = player.Position + new Vector2(player.PixelToScreen(player.Size).X/2,0) - playerBullet.PixelToScreen(playerBullet.Size);
                    playerBullet.Visible = true;
                    playerBullet.IsDead = false;
                }
                // make sure the player stays on screen
                if(player.Position.X < 0) player.Position = new Vector2(0, player.Position.Y);
                else if(player.Position.X + player.PixelToScreen(player.Size).X > Size.X) player.Position = new Vector2(Size.X - player.PixelToScreen(player.Size).X, player.Position.Y);
                // make sure player bullet stays on screen
                if(playerBullet.Position.Y < 0)
                {
                    playerBullet.IsDead = true;
                    playerBullet.Visible = false;
                }
                // hit barrier
                foreach(DestructableSprite b in barrier)
                {
                    b.Hit(playerBullet);
                    b.Hit(EnemyBullet);
                }
                // hit enemy
                if(wave.Hit(playerBullet))
                {
                    playerBullet.IsDead = true;
                    playerBullet.Visible = false;
                    score += 10;
                    if(score > highScore)
                    {
                        highScore = score;
                        GridInfo.SetVar("HighScore", highScore.ToString());
                    }
                }
                if(player.Intersect(EnemyBullet))
                {
                    player.IsDead = true;
                    //game_over = true;
                    lives--;
                    if(lives <= 0)
                    {
                        game_over = true;
                        lives = 0;
                    }
                    EnemyBullet.IsDead = true;

                }
                if (EnemyBullet.Position.Y > Size.Y)
                {
                    EnemyBullet.IsDead = true;
                    EnemyBullet.Visible = false;
                }
            }
            // reset game
            public void Reset()
            {
                score = 0;
                lives = 3;
                player.Position = new Vector2(Size.X / 2, Size.Y * 0.9f);
                player.IsDead = false;
                wave.LoadWave(0);
                foreach (DestructableSprite b in barrier)
                {
                    b.Reset();
                }
                game_over = false;
                EnemyBullet.IsDead = true;
                EnemyBullet.Visible = false;
                EnemyBullet.Position = new Vector2(-100, 0);
                playerBullet.IsDead = true;
                playerBullet.Visible = false;
                playerBullet.Position = new Vector2(-100, 0);
                gameOverDisplay.Visible = false;
                continueControls.Visible = false;
            }
        }
    }
}
