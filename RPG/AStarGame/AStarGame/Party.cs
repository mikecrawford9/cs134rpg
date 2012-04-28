using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public class Party
    {
        public Player[] partyMembers;
        public int score;
        public int money;
        Dictionary<String, Quest> open;
        Dictionary<String, Quest> completed;

        public Party(Player[] partyMembers, int score = 0, int money = 1000)
        {
            this.open = new Dictionary<String, Quest>();
            this.completed = new Dictionary<String, Quest>();
            this.partyMembers = partyMembers;
            this.score = score;
            this.money = money;
            //this.partyMembers[0].inventory.AddItem(Item.DRAGON_SKULL);
        }

        public void CalculateScore()
        {
            int output = 0;
            foreach (Player p in partyMembers)
            {
                output += p.currentExp;
                if(p.inventory.Search(Item.DRAGON_SKULL))
                {
                    score += 10000;
                }
            }
            
        }

        public Item[] getAllItems()
        {
            List<Item> itemret = new List<Item>();
            foreach (Player p in partyMembers)
            {
                Item[] x = p.inventory.getItems();
                for (int i = 0; i < x.Length; i++)
                    itemret.Add(x[i]);
            }
            return itemret.ToArray();
        }

        public void completeQuest(Quest q)
        {
            if(open.ContainsKey(q.getQuestID()))
                open.Remove(q.getQuestID());

            if(!completed.ContainsKey(q.getQuestID()))
                completed.Add(q.getQuestID(),q);
        }

        public bool questInProgress(String id)
        {
            if (open.ContainsKey(id))
                return true;
            else
                return false;
        }

        public bool questCompleted(String id)
        {
            if (completed.ContainsKey(id))
                return true;
            else
                return false;
        }

        public void addQuest(Quest q)
        {
            if(!open.ContainsKey(q.getQuestID()))
                open.Add(q.getQuestID(), q);
        }

        public bool questInProgressOrCompleted(String id)
        {
            if (open.ContainsKey(id))
                return true;

            if (completed.ContainsKey(id))
                return true;

            return false;
        }

        public bool CanBuyItem(Item item)
        {
            if (this.money < item.cost)
            {
                return false;
            }
            return true;
        }

        public Item BuyItem(Item item)
        {
            if(CanBuyItem(item))
            {
                foreach (Player p in partyMembers)
                {
                    if (p.inventory.AddItem(item))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public bool hasItem(Item item)
        {
            foreach (Player p in partyMembers)
            {
                if (p.inventory.Search(item))
                {
                    return true;
                }
            }

            return false;
        }


        public void removeItem(Item item)
        {
            foreach (Player p in partyMembers)
            {
                if (p.inventory.Search(item))
                {
                    p.inventory.RemoveItem(item);
                }
            }
        }

        public void Spend(int amount)
        {
            money -= amount;
        }

        public void MakeMoney(int amount)
        {
            money -= amount;
        }


        public bool SellItem(Player member, Item item)
        {
            if(member.inventory.Search(item))
            {
                this.money += (item.cost / 2);
                return member.inventory.RemoveItem(item);
            }
            return false;
        }
             

    }
}
