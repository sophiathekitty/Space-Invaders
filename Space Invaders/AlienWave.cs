using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
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
        //----------------------------------------------------------------------
        // AlienWave
        //----------------------------------------------------------------------
        public class AlienWave : IScreenSpriteProvider
        {
            List<AnimatedSprite> invaders = new List<AnimatedSprite>();
            string[] waves =
                {
                    "000000000" +
                    "002222200" +
                    "000000000" +
                    "000000000" +
                    "000000000",

                    "000000000" +
                    "022222220" +
                    "000000000" +
                    "000000000" +
                    "000000000",

                    "000000000" +
                    "000000000" +
                    "222222222" +
                    "000000000" +
                    "000000000",

                    "000000000" +
                    "111111111" +
                    "222222222" +
                    "000000000" +
                    "000000000",

                    "111111111" +
                    "222222222" +
                    "333333333" +
                    "000000000" +
                    "000000000",

                    "111111111" +
                    "222222222" +
                    "222222222" +
                    "333333333" +
                    "000000000",

                    "111111111" +
                    "222222222" +
                    "222222222" +
                    "333333333" +
                    "333333333"
                };
            int waveIndex = 0;
            RectangleF viewport;
            Vector2 velocity;
            Vector2 position;
            Vector2 startPosition;
            Color[] colors = new Color[] { Color.Cyan, Color.Purple, Color.Yellow };
            public Vector2 Position 
            { 
                get { return position; } 
                set 
                { 
                    position = value; 
                    int i = 0;
                    int j = 0;
                    float spacing = viewport.Width / (gridSize.X + 1);
                    for (int row = 0; row < gridSize.Y; row++)
                    {
                        for(int col = 0; col < gridSize.X; col++)
                        {
                            if (waves[waveIndex][i] != '0')
                            {
                                invaders[j].Position = position + new Vector2(col * (spacing), row * (spacing));
                                j++;
                            }
                            i++;
                        }
                    }
                } 
            }
            public List<Vector2> BulletPoints
            {
                get
                {
                    List<Vector2> points = new List<Vector2>();
                    foreach (var invader in invaders)
                    {
                        if(!invader.Visible) continue;
                        Vector2 screenSize = invader.PixelToScreen(invader.Size);
                        points.Add(invader.Position + new Vector2(screenSize.X / 2, screenSize.Y));
                    }
                    return points;
                }
            }
            public bool isDefeated
            {
                get
                {
                    foreach (var invader in invaders)
                    {
                        if(invader.Visible) return false;
                    }
                    return true;
                }
            }
            Vector2 size;
            Vector2 spriteSize;
            Vector2I gridSize = new Vector2I(9, 5);
            Screen screen;
            public AlienWave(Vector2 position, float scale, Vector2 size, RectangleF viewport)
            {
                int rows = gridSize.Y;
                int cols = gridSize.X;
                this.position = position;
                startPosition = position;
                this.viewport = viewport;
                spriteSize = size;
                float spacing = viewport.Width / (cols + 2);
                LoadWave(0);
                /*
                int i = 0;
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        //invaders.Add(new AnimatedSprite(position + new Vector2(col * (size.X + spacing), row * (size.Y + spacing)), scale, size, frames, death));
                        if(wave[i] != 0) invaders.Add(new AnimatedSprite(position + new Vector2(col * (spacing), row * (spacing)), scale, size, GetAliveInvaders(wave[i]), SpriteLibrary.Sprites["alienDeath"][0]));
                        i++;
                    }
                }
                */
                this.size = new Vector2(cols * spacing, rows * spacing) + (invaders[0].PixelToScreen(invaders[0].Size));
                velocity = new Vector2(InvaderGame.enemy_speed, 0);
            }
            public void NextWave()
            {
                waveIndex++;
                if (waveIndex >= waves.Length)
                {
                    InvaderGame.Score += 1978;
                    waveIndex = 0;
                }
                LoadWave(waveIndex);
            }
            public void LoadWave(int index)
            {
                waveIndex = index;
                int rows = gridSize.Y;
                int cols = gridSize.X;
                if(screen != null) screen.RemoveSprite(this);
                invaders.Clear();
                float spacing = viewport.Width / (cols + 2);
                int i = 0;
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        if (waves[index][i] != '0') invaders.Add(new AnimatedSprite(position + new Vector2(col * (spacing), row * (spacing)), RasterSprite.DEFAULT_PIXEL_SCALE, spriteSize, GetAliveInvaders(waves[index][i]), SpriteLibrary.Sprites["alienDeath"][0]));
                        if (waves[index][i] != '0') invaders[invaders.Count - 1].Color = colors[waves[index][i] - '1'];
                        i++;
                    }
                }
                this.size = new Vector2(cols * spacing, rows * spacing) + (invaders[0].PixelToScreen(invaders[0].Size));
                velocity = new Vector2(InvaderGame.enemy_speed, 0);
                if(screen != null) screen.AddSprite(this);
            }
            public void Reset()
            {
                Position = startPosition;
                foreach (var invader in invaders)
                {
                    invader.Visible = true;
                    invader.IsDead = false;
                }
            }
            List<string> GetAliveInvaders(char type)
            {
                return SpriteLibrary.Sprites["alien" + type];
            }

            void IScreenSpriteProvider.AddToScreen(Screen screen)
            {
                this.screen = screen;
                foreach (var invader in invaders)
                {
                    screen.AddSprite(invader);
                }
            }

            void IScreenSpriteProvider.RemoveToScreen(Screen screen)
            {
                foreach (var invader in invaders)
                {
                    screen.RemoveSprite(invader);
                }
            }
            public void Update()
            {
                Position += velocity;
                if(Position.X < 0)
                {
                    velocity.X = InvaderGame.enemy_speed;
                    Position += new Vector2(0, InvaderGame.enemy_speed/4);
                    Position = new Vector2(1, Position.Y);
                }
                else if(Position.X + size.X > viewport.Width)
                {
                    velocity.X = -InvaderGame.enemy_speed;
                    Position += new Vector2(0, InvaderGame.enemy_speed/4);
                    Position = new Vector2(viewport.Width - size.X - 1, Position.Y);
                }
            }
            int deadframes = 0;
            public bool Hit(Bullet bullet)
            {
                foreach(AnimatedSprite invader in invaders)
                {
                    if(!invader.Visible) continue;
                    if (invader.IsDead && deadframes-- < 0)
                    {
                        invader.Visible = false;
                        continue;
                    }
                    if(bullet.Visible == false) continue;
                    if(invader.Intersect(bullet))
                    {
                        invader.IsDead = true;
                        bullet.IsDead = true;
                        bullet.Visible = false;
                        deadframes = 2;
                        GameSoundPlayer.Play("FlappyPoint");
                        return true;
                    }
                }
                return false;
            }
        }
        //----------------------------------------------------------------------
    }
}
