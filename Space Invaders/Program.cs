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
    partial class Program : MyGridProgram
    {
        //=======================================================================
        InvaderGame game;
        public Program()
        {
            Echo("Space Invaders");
            GridInfo.Init("Space Invaders", this);
            Echo("Grid Info Ready");
            GridBlocks.InitBlocks(GridTerminalSystem);
            Echo("Grid Blocks Ready");
            SpriteLibrary.Load();
            Echo("Sprite Library Ready");
            game = new InvaderGame(GridBlocks.GetTextSurface("Main Display"));
            Echo("Game Ready");
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            Echo("Space Invaders Ready");
        }

        public void Save()
        {
            GridInfo.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            game.Main(argument);
        }
        //=======================================================================
    }
}
