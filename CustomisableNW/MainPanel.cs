using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace CustomisableNW
{
    public partial class MainForm
    {
        public PictureBox Scheme { get => schemePB; }
        public PictureBox Diagram { get => diagramPB; }
        public string TextBox
        {
            get => dataTextBox.Text;
            set => dataTextBox.Text = value;
        }


        

        Panel mainPanel = new Panel();
        MenuStrip menuStrip = new MenuStrip();
        StatusStrip statusStrip = new StatusStrip();

        string font = "Arial";

        void MainPanelGraphics()
        {
            // mainPanel settings
            mainPanel.Size = new Size(this.Width * 2 / 3, this.ClientSize.Height - statusStrip.Height);
            mainPanel.Dock = DockStyle.Right;
            mainPanel.BorderStyle = BorderStyle.Fixed3D;
            mainPanel.Visible = false;
            Controls.Add(mainPanel);

            // menuStrip settings
            menuStrip.Dock = DockStyle.Top;

            //вывод численных данных 
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Calculations");
            toolStripMenuItem1.Click += (o, e) =>
            {
                diagramPanel.Visible = false;
                schemePanel.Visible = false;
                dataPanel.Visible = true;
            };
            menuStrip.Items.Add(toolStripMenuItem1);

            // график значений весов
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Weights diagram");
            toolStripMenuItem2.Click += (o, e) =>
            {
                dataPanel.Visible = false;
                schemePanel.Visible = false;
                diagramPanel.Visible = true;
                DiagramDrawing();
            };
            menuStrip.Items.Add(toolStripMenuItem2);

            // графическое представление сети
            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("Web scheme");
            toolStripMenuItem3.Click += (o, e) =>
            {
                diagramPanel.Visible = false;
                dataPanel.Visible = false;
                schemePanel.Visible = true;
                //Drawing1();
                Drawing();
            };
            menuStrip.Items.Add(toolStripMenuItem3);

            // о программе
            ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("About");
            toolStripMenuItem4.Click += (o, e) =>
            {
                string text = "Разработка данного приложения\nцеликом и полностью лежит\nна плечах IlyaProgrammer\n ВО СВЁМ ВИНОВАТ ОН!!!!!!.";
                MessageBox.Show(text);
            };
            menuStrip.Items.Add(toolStripMenuItem4);

            mainPanel.Controls.Add(menuStrip);

            // statusStrip settings
            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.BackColor = Color.Red;
            mainPanel.Controls.Add(statusStrip);






        }

        //void PanelVisualizer()
        //{
        //    List<Panel> panels = new List<Panel>
        //    {
        //        dataPanel,
        //        schemePanel,
        //        diagramPanel
        //    };
        //    List<ToolStripMenuItem> toolStripMenuItems = new List<ToolStripMenuItem>();
        //    foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
        //    {
        //        toolStripMenuItems.Add(toolStripMenuItem);
        //    }


        //    for(int i = 0; i < panels.Count; i++)
        //    {
        //        if ()
        //        {
        //            //item подкрасить
        //            // панель визуализировать
        //        }
        //        else
        //        {
        //            //item обесцветить
        //            //панель спрятать
        //        }
        //    }


        //}
    }
}
