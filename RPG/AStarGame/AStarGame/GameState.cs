/*
Name: Michael Crawford
Class: CS134
Instructor: Dr. Teoh
Term: Spring 2012
Assignment: Project 2
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public enum GameState { TITLE, EDIT, ASTAR, RUNNING, GAMEOVER, SAVEMAP, LOADMAP, ADDEVENT, MAINMENU, COMBAT, SHOP, INN};
    public enum PlayState { WORLD, INVENTORY, WAITFORNPC, NPCQUEST, MESSAGE, BATTLE, GAMEOVER_WIN, GAMEOVER_LOSE};
}

