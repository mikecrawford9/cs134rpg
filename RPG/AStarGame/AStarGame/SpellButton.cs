using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{
    public class SpellButton : Button
    {
        public Player player;
        public List<Event> eventList;
        public BattleSequence bs;
        Boolean inprogress = false;

        public SpellButton(Texture2D texture, SpriteFont font, String text, Player p, BattleSequence bs, List<Event> events)
            : base(texture, font, text)
        {
            this.player = p;
            this.eventList = events;
            this.bs = bs;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
            if (base.clicked && !inprogress)
            {
                inprogress = true;
                bs.state = BattleStageType.FIGHT;
                List<Player> playerList = new List<Player>();
                foreach (Enemy e in bs.enemies)
                {
                    playerList.Add(e.player);
                }
                bs.currentActions.Enqueue(new BattleAction(bs, player, playerList.ToArray(), BattleActionType.SPELL, Spell.FIRE, null));
                foreach (Enemy e in bs.enemies)
                {
                    bs.currentActions.Enqueue(new BattleAction(bs, e.player, new Player[] { player }, BattleActionType.ATTACK, Spell.ATTACK, null));
                   
                }
                bs.state = BattleStageType.FIGHT;
                base.clicked = false;
                inprogress = false;
            }
        }
    }
}
