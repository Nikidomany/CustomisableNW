using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace CustomisableNW
{
    public partial class MainForm
    {  
        Panel mainPanel;
        MenuStrip menuStrip;
        StatusStrip statusStrip;

        string font = "Arial";

        void MainPanelGraphics()
        {
            // mainPanel settings
            mainPanel = new Panel
            {
                Size = new Size(this.Width * 2 / 3, this.ClientSize.Height - 22),
                Dock = DockStyle.Right,
                BorderStyle = BorderStyle.Fixed3D,
                Visible = false
            };
            Controls.Add(mainPanel);

            // menuStrip settings
            menuStrip = new MenuStrip
            {
                Dock = DockStyle.Top
            };

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
                Drawing();
                if (setButton.Text == "RESET") // КОСТЫЛЬ!!!
                    UpdateNeuronLabels();
            };
            menuStrip.Items.Add(toolStripMenuItem3);

            // о программе
            ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("About");
            toolStripMenuItem4.Click += (o, e) =>
            {
                string text = "Разработка данного приложения\nцеликом и полностью лежит\nна плечах IlyaProgrammer\n ВО СВЁМ ВИНОВАТ ОН!!!!!!.";
                string caption = "Информация к размышлению";
                MessageBox.Show(text,caption);
            };
            menuStrip.Items.Add(toolStripMenuItem4);

            mainPanel.Controls.Add(menuStrip);

            // statusStrip settings
            statusStrip = new StatusStrip
            {
                Dock = DockStyle.Bottom,
                BackColor = Color.Red
            };
            mainPanel.Controls.Add(statusStrip);






        }

    }
}
