using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{
    public class ItemButton : Button
    {
        public Player player;
        public List<Event> eventList;
        public BattleSequence bs;

        public ItemButton(Texture2D texture, SpriteFont font, String text, Player p, BattleSequence bs, List<Event> events) : base(texture, font, text)
        {
            this.player = p;
            this.eventList = events;
            this.bs = bs;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (base.clicked)
            {
                bs.state = BattleStageType.FIGHT;
                List<Player> playerList = new List<Player>();
                foreach (Enemy e in bs.enemies)
                {
                    playerList.Add(e.player);
                }
                bs.currentActions.Add(new BattleAction(bs, player, new Player[] {player}, BattleActionType.ITEM, null, Item.HP_POTION_100));
                bs.combatLog.Add("You use a Low Potion");
                foreach (Enemy e in bs.enemies)
                {
                    bs.currentActions.Add(new BattleAction(bs, e.player, new Player[] {player},BattleActionType.ATTACK, Spell.ATTACK, null));
                    bs.combatLog.Add("Enemy attacks you.");
                }


            }

        }
    }
}
