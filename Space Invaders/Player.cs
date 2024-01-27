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
        // Player
        //----------------------------------------------------------------------
        public class Player : AnimatedSprite
        {
            GameInput input;
            public Player(Vector2 position, float scale, List<string> frames, string death, GameInput input) : base(position, scale, new Vector2(16,8), frames, death)
            {
                this.input = input;
            }
            public void Update()
            {
                if(IsDead) return;
                if(input.A) Position += new Vector2(-InvaderGame.player_speed, 0);
                if(input.D) Position += new Vector2(InvaderGame.player_speed, 0);
            }
        }
        //----------------------------------------------------------------------
    }
}
