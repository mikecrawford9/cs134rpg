﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RPG
{
    class BattleSequence
    {
        Party party;
        Enemy[] enemies;
        List<String> combatLog;
        SpriteFont combatLogFont;
        List<Button> visibleButtons;
        List<BattleAction> currentActions;
        bool continueCombat; 
        bool partyDead;
        bool enemiesDead;
        Game1 game;

        public BattleSequence(Party party, Enemy[] enemies, SpriteFont displayTextFont, TileMap battleMap, Game1 game)
        {
            this.party = party;
            this.enemies = enemies;
            this.combatLog = new List<String>();
            this.combatLogFont = displayTextFont;
            this.currentActions = new List<BattleAction>();
            this.continueCombat = true; 
            this.partyDead = false;
            this.enemiesDead= false;
            this.game = game;
            
        }

        public void Start()
        {
            combatLog.Add("Enemies have appeared.");
            while (continueCombat)
            {
                foreach (Player p in party.partyMembers)
                {
                    AttackButton p1Attack = new AttackButton(game.Content.Load<Texture2D>("Tiles/buttonSmall"), game.Content.Load<SpriteFont>("Tiles/buttonFont"), game.spriteBatch, "Attack", p, null);
                    p1Attack.Location(100, 100);
                    p1Attack.Draw();
                }
            }
        }

        public void AttemptToFlee()
        {
            switch ((new Random()).Next(0, 1))
            {
                case 0: break;
                case 1: this.continueCombat = false; break;
                default: break;
            }
        }

        public void Draw()
        {

        }


    }
    public enum BattleActionType {ATTACK, SPELL, ITEM, FLEE}

    class BattleAction
    {
        public Player user;
        public Player[] target;
        public BattleActionType type;
        public BattleSequence battleSequence;
        public Spell spell; //null if not a spell
        public Item item; //null if not an item 

        
        public BattleAction(BattleSequence bs, Player user, Player[] target, BattleActionType type, Spell spell, Item item)
        {
            this.user = user;
            this.target = target;
            this.type = type;
            this.spell = spell;
            this.item = item;
            this.battleSequence = bs;
        }

        public void performAction()
        {
            switch (type)
            {
                case BattleActionType.ATTACK:
                    foreach(Player t in target)
                    {
                        user.UseSpell(t, spell);
                    }
                    break;
                case BattleActionType.ITEM:
                    foreach (Player t in target)
                    {
                        t.UseHealingItem(item);
                    }
                    break;
                case BattleActionType.SPELL:
                    foreach (Player t in target)
                    {
                        user.UseSpell(t, spell);
                    }
                    break;
                case BattleActionType.FLEE:
                    
                    break;
                default: 
                    break;
            }
        }
    }
}