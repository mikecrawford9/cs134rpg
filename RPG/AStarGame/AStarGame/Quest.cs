using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public class Quest
    {
        String questtext;
        String questcompletetext;
        String questid;
        Item questitem;

        public Quest(String questid, String questtext, String questcompletetext, Item questitem)
        {
            this.questid = questid;
            this.questtext = questtext;
            this.questcompletetext = questcompletetext;
            this.questitem = questitem;
        }

        public String getQuestID()
        {
            return questid;
        }

        public String getQuestText()
        {
            return questtext;
        }

        public String getQuestCompleteText()
        {
            return questcompletetext;
        }

        public Item getQuestItem()
        {
            return questitem;
        }
    }
}
