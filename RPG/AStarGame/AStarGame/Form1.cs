using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RPG
{
    partial class Form1 : Form
    {
        private Event theEvent;
        private List<Control> items;

        public Form1(Event theEvent)
        {
            this.theEvent = theEvent;
            items = new List<Control>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String s = (String)this.comboBox1.SelectedItem;
            if (s == "Map Transition")
            {
                theEvent.setEventType(EventType.MAP_TRANSITION);
            }
            else if (s == "Start Battle")
            {
                theEvent.setEventType(EventType.BATTLE_TILE);
            }
            else if (s == "Wait For NPC")
            {
                theEvent.setEventType(EventType.WAITFORNPC);
            }

            for(int i = 0; i < items.Count; i++)
                if (items[i] is TextBox)
                    theEvent.addProperty(items[i].Name, items[i].Text);

            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeItemsFromLayout();
            clearItems();

            String s = (String)this.comboBox1.SelectedItem;
            if (s == "Map Transition")
            {
                processMapTransitionLayout();
            }
            else if (s == "Start Battle")
            {
                processBattleTileLayout();
            }
            else if (s == "Wait For NPC")
            {
                processWaitForNPCLayout();
            }
        }

        public void processMapTransitionLayout()
        {
            System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox1 = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Label label3 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox2 = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Label label4 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox3 = new System.Windows.Forms.TextBox();

            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 59);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(47, 13);
            label2.TabIndex = 4;
            label2.Text = "Map File";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(13, 75);
            textBox1.Name = "mapfile";
            textBox1.Size = new System.Drawing.Size(247, 20);
            textBox1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(10, 103);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(104, 13);
            label3.TabIndex = 6;
            label3.Text = "X Spawn Coordinate";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(13, 119);
            textBox2.Name = "x";
            textBox2.Size = new System.Drawing.Size(50, 20);
            textBox2.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(10, 147);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(104, 13);
            label4.TabIndex = 8;
            label4.Text = "Y Spawn Coordinate";
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(13, 163);
            textBox3.Name = "y";
            textBox3.Size = new System.Drawing.Size(50, 20);
            textBox3.TabIndex = 9;
            // 
            // Form1
            // 

            items.Add(textBox3);
            items.Add(label4);
            items.Add(textBox2);
            items.Add(label3);
            items.Add(textBox1);
            items.Add(label2);

            addItemsToLayout();
        }

        public void processBattleTileLayout()
        {
            System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox1 = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Label label3 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox2 = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Label label4 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox3 = new System.Windows.Forms.TextBox();

            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 59);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(47, 13);
            label2.TabIndex = 4;
            label2.Text = "Enemy Texture";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(13, 75);
            textBox1.Name = "enemytexture";
            textBox1.Size = new System.Drawing.Size(247, 20);
            textBox1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(10, 103);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(104, 13);
            label3.TabIndex = 6;
            label3.Text = "Hitpoints";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(13, 119);
            textBox2.Name = "hp";
            textBox2.Size = new System.Drawing.Size(50, 20);
            textBox2.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(10, 147);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(104, 13);
            label4.TabIndex = 8;
            label4.Text = "Battle Map";
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(13, 163);
            textBox3.Name = "battlemap";
            textBox3.Size = new System.Drawing.Size(247, 20);
            textBox3.TabIndex = 9;

            items.Add(textBox3);
            items.Add(label4);
            items.Add(textBox2);
            items.Add(label3);
            items.Add(textBox1);
            items.Add(label2);

            addItemsToLayout();
        }

        public void processWaitForNPCLayout()
        {
            createBoxAndLabel("NPC Texture", "npctexture", 5, 55, 247);
            createBoxAndLabel("Spawn X", "x", 5, 90, 50);
            createBoxAndLabel("Spawn Y", "y", 60, 90, 50);
            createBoxAndLabel("Goal X", "goalx", 115, 90, 50);
            createBoxAndLabel("Spawn Y", "goaly", 170, 90, 50);
            createBoxAndLabel("Quest to give", "questid", 5, 125, 247);
            createBoxAndLabel("Quest Return Map", "questret", 5, 160, 247);
            createBoxAndLabel("QR X", "questretx", 5, 195, 50);
            createBoxAndLabel("QR Y", "questrety", 60, 195, 50);
            
            addItemsToLayout();
        }

        private void createBoxAndLabel(String label, String id, int x, int y, int width)
        {
            System.Windows.Forms.Label label5 = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox5 = new System.Windows.Forms.TextBox();

            // 
            // label3
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(x, y);
            label5.Name = "label3";
            label5.Size = new System.Drawing.Size(width, 13);
            label5.TabIndex = 6;
            label5.Text = label;
            // 
            // textBox2
            // 
            textBox5.Location = new System.Drawing.Point(x, y+13);
            textBox5.Name = id;
            textBox5.Size = new System.Drawing.Size(width, 20);
            textBox5.TabIndex = 7;

            items.Add(textBox5);
            items.Add(label5);
        }

        private void addItemsToLayout()
        {
            this.SuspendLayout();
            for (int i = 0; i < items.Count; i++)
                this.Controls.Add(items[i]);

            this.ResumeLayout();
        }

        public void removeItemsFromLayout()
        {
            this.SuspendLayout();
            for (int i = 0; i < items.Count; i++)
                this.Controls.Remove(items[i]);

            this.ResumeLayout();
        }

        public void clearItems()
        {
            while (items.Count > 0)
                items.RemoveAt(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            theEvent.setEventType(EventType.CANCELED);
            this.Close();
        }
    }
}
