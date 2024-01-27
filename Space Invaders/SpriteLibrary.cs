using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        // SpriteLibrary
        //----------------------------------------------------------------------
        public class SpriteLibrary
        {
            public static Dictionary<string,List<string>> Sprites = new Dictionary<string, List<string>>();
            public static List<char> pallet = new List<char>();
            public static void Load()
            {
                //GridInfo.Echo("Loading Sprites....");
                IMyTextPanel Block = GridBlocks.GetTextPanel("Main Display");
                //GridInfo.Echo("Block: " + Block.CustomName);
                Sprites.Clear();
                RasterSprite sheet = new RasterSprite(Vector2.Zero,RasterSprite.DEFAULT_PIXEL_SCALE,Vector2.Zero,Block.CustomData);
                RasterSprite.IGNORE = sheet.Data[0];
                //GridInfo.Echo("Sheet: " + sheet.Size);
                // alien 1
                Sprites.Add("alien1", new List<string>());
                Sprites["alien1"].Add(sheet.getPixels(0, 0, 16, 8).Replace(RasterSprite.IGNORE.ToString(),RasterSprite.INVISIBLE));
                Sprites["alien1"].Add(sheet.getPixels(0, 8, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien 1: " + Sprites["alien1"][0].Length);
                // alien 2
                Sprites.Add("alien2", new List<string>());
                Sprites["alien2"].Add(sheet.getPixels(16, 0, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alien2"].Add(sheet.getPixels(16, 8, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien 2: " + Sprites["alien2"][0].Length);
                // alien 3
                Sprites.Add("alien3", new List<string>());
                Sprites["alien3"].Add(sheet.getPixels(32, 0, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alien3"].Add(sheet.getPixels(32, 8, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien 3: " + Sprites["alien3"][0].Length);
                // alien death
                Sprites.Add("alienDeath", new List<string>());
                Sprites["alienDeath"].Add(sheet.getPixels(48, 0, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien Death: " + Sprites["alienDeath"][0].Length);
                // saucer
                Sprites.Add("saucer", new List<string>());
                Sprites["saucer"].Add(sheet.getPixels(48, 8, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Saucer: " + Sprites["saucer"][0].Length);
                // saucer death
                Sprites.Add("saucerDeath", new List<string>());
                Sprites["saucerDeath"].Add(sheet.getPixels(64, 16, 24, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Saucer Death: " + Sprites["saucerDeath"][0].Length);
                // player
                Sprites.Add("player", new List<string>());
                Sprites["player"].Add(sheet.getPixels(32, 16, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Player: " + Sprites["player"][0].Length);
                // player death
                Sprites.Add("playerDeath", new List<string>());
                Sprites["playerDeath"].Add(sheet.getPixels(48, 16, 16, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Player Death: " + Sprites["playerDeath"][0].Length);
                // barrier
                Sprites.Add("barrier", new List<string>());
                Sprites["barrier"].Add(sheet.getPixels(64, 0, 24, 16).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Barrier: " + Sprites["barrier"][0].Length);
                // alien bullet
                Sprites.Add("alienBullet", new List<string>());
                Sprites["alienBullet"].Add(sheet.getPixels(0, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alienBullet"].Add(sheet.getPixels(3, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alienBullet"].Add(sheet.getPixels(6, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alienBullet"].Add(sheet.getPixels(9, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien Bullet: " + Sprites["alienBullet"][0].Length);
                // alien bullet 2
                Sprites.Add("alienBullet2", new List<string>());
                Sprites["alienBullet2"].Add(sheet.getPixels(12, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alienBullet2"].Add(sheet.getPixels(15, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alienBullet2"].Add(sheet.getPixels(18, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["alienBullet2"].Add(sheet.getPixels(21, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien Bullet 2: " + Sprites["alienBullet2"][0].Length);
                // player bullet
                Sprites.Add("playerBullet", new List<string>());
                Sprites["playerBullet"].Add(sheet.getPixels(24, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["playerBullet"].Add(sheet.getPixels(27, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                Sprites["playerBullet"].Add(sheet.getPixels(30, 16, 3, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Player Bullet: " + Sprites["playerBullet"][0].Length);
                // explosion
                Sprites.Add("explosion", new List<string>());
                Sprites["explosion"].Add(sheet.getPixels(88, 0, 6, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Explosion: " + Sprites["explosion"][0].Length);
                // alien fork
                Sprites.Add("alienFork", new List<string>());
                Sprites["alienFork"].Add(sheet.getPixels(88, 8, 5, 8).Replace(RasterSprite.IGNORE.ToString(), RasterSprite.INVISIBLE));
                //GridInfo.Echo("Alien Fork: " + Sprites["alienFork"][0].Length);
                // pallet
                pallet.Clear();
                string palletString = sheet.getPixels(93, 17, 6, 1);
                for (int i = 0; i < palletString.Length; i++)
                {
                    if (!pallet.Contains(palletString[i]) && palletString[i]!='\n') pallet.Add(palletString[i]);
                }
            }
        }
        //----------------------------------------------------------------------
    }
}
