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
        // DestructableSprite
        //----------------------------------------------------------------------
        public class DestructableSprite : RasterSprite
        {
            string baseData = "";
            char delChar = rgb(255,0,0);
            public DestructableSprite(Vector2 position, float scale, Vector2 size, string data) : base(position, scale, size, data)
            {
                baseData = data;
            }
            public bool Hit(Bullet bullet)
            {
                if(bullet.Visible == false) return false;
                bool hit = Intersect(bullet,true,true);
                if (bullet.IsDead)
                {
                    bullet.Visible = false;
                }
                if (hit)
                {
                    GridInfo.Echo("Hit: bullet dead? "+ bullet.IsDead);
                    if(bullet.IsDead) bullet.Visible = false;
                    bullet.IsDead = true;
                    Data = Data.Replace(delChar.ToString(), INVISIBLE);
                }
                return hit;
            }
        }
        //----------------------------------------------------------------------
    }
}
