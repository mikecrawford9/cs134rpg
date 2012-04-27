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
