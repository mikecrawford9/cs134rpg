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

        public Form1(Event theEvent)
        {
            this.theEvent = theEvent;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            String newmap = this.textBox1.Text;
            String x = this.textBox2.Text;
            String y = this.textBox3.Text;

            theEvent.setEventType(EventType.MAP_TRANSITION);
            theEvent.addProperty("mapfile", newmap);
            theEvent.addProperty("x", x);
            theEvent.addProperty("y", y);
            
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
