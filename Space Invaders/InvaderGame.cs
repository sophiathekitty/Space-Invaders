﻿using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            int score = 0;
            int highScore = 0;
            public static float player_speed = 2;
            float enemy_speed = 1;
            float bullet_speed = 3f;
            GameInput input;
            Player player;
            Bullet playerBullet;
            Bullet EnemyBullet;
            List<DestructableSprite> barrier = new List<DestructableSprite>();
            //------------------------------------------------------------------
            // constructor
            //------------------------------------------------------------------
            public InvaderGame(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
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
            public override void Update()
            {
                // game logic goes here
                player.Update();
                playerBullet.Update();
                EnemyBullet.Update();
                if(input.Space && !playerBullet.Visible)
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
                }
            }
        }
    }
}
