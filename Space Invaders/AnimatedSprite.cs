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
        public class AnimatedSprite : RasterSprite
        {
            List<string> frames = new List<string>();
            string death = "";
            int frame = 0;
            int frameDelay = 0;
            static int frameDelayMax = 5;
            bool isDead = false;
            public bool IsDead { get { return isDead; } set { isDead = value; Data = value ? death : frames[0]; } }
            public AnimatedSprite(Vector2 position, float scale, Vector2 size, List<string> frames, string death) : base(position, scale, size, frames[0])
            {
                this.frames = frames;
                this.death = death;
            }
            public override MySprite ToMySprite(RectangleF _viewport)
            {
                if (isDead) Data = death;
                else if(frameDelay++ > frameDelayMax)
                {
                    frameDelay = 0;
                    frame++;
                    if(frame >= frames.Count) frame = 0;
                    Data = frames[frame];
                }
                return base.ToMySprite(_viewport);
            }
        }
    }
}
