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
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program
    {
        //----------------------------------------------------------------------
        // Bullet
        //----------------------------------------------------------------------
        public class Bullet : AnimatedSprite
        {
            Vector2 velocity = new Vector2(0, -1);
            public Bullet(Vector2 position, float scale, List<string> frames, string death, Vector2 velocity) : base(position, scale, new Vector2(3, 8), frames, death.Replace(rgb(255,255,255),rgb(255,0,0)))
            {
                this.velocity = velocity;
            }
            public void Update()
            {
                if(isDead) return;
                Position += velocity;
            }
        }
    }
}
