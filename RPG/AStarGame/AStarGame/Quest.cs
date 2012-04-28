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
        int returnx;
        int returny;
        String returnmap;
        Item questitem;

        public Quest(String questid, String questtext, String questcompletetext, Item questitem, int returnx, int returny, String returnmap)
        {
            this.questid = questid;
            this.questtext = questtext;
            this.questcompletetext = questcompletetext;
            this.questitem = questitem;
            this.returnx = returnx;
            this.returny = returny;
            this.returnmap = returnmap;
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

        public int getReturnX()
        {
            return returnx;
        }

        public int getReturnY()
        {
            return returny;
        }

        public String getReturnMap()
        {
            return returnmap;
        }
    }
}
