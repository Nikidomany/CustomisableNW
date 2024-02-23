using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{
    public partial class MainForm
    {
        Panel mainPanel;
        MenuStrip menuStrip;
        StatusStrip statusStrip;
        ToolStripButton weightsPointingLabelsDrawingModeButton;
        ToolStripButton printDataButton;

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
                errorDiagramPanel.Visible = false;
                schemePanel.Visible = false;
                weightsDiagramPanel.Visible = false;
                dataPanel.Visible = true;
            };
            menuStrip.Items.Add(toolStripMenuItem1);

            // диаграмма ошибки
            ToolStripMenuItem toolStripMenuItem20 = new ToolStripMenuItem("Error diagram");
            toolStripMenuItem20.Click += (o, e) =>
            {
                dataPanel.Visible = false;
                schemePanel.Visible = false;
                weightsDiagramPanel.Visible = false;
                errorDiagramPanel.Visible = true;
                DrawErrorDiagram();
            };
            menuStrip.Items.Add(toolStripMenuItem20);

            // диаграмма весов
            ToolStripMenuItem toolStripMenuItem21 = new ToolStripMenuItem("Weights diagram");
            toolStripMenuItem21.Click += (o, e) =>
            {
                errorDiagramPanel.Visible = false;
                dataPanel.Visible = false;
                schemePanel.Visible = false;
                weightsDiagramPanel.Visible = true;
                DrawWeightDiagram();
            };
            menuStrip.Items.Add(toolStripMenuItem21);

            // графическое представление сети
            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("Web scheme");
            toolStripMenuItem3.Click += (o, e) =>
            {
                errorDiagramPanel.Visible = false;
                dataPanel.Visible = false;
                weightsDiagramPanel.Visible = false;
                schemePanel.Visible = true;
                DrawScheme();
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
                MessageBox.Show(text, caption);
            };
            menuStrip.Items.Add(toolStripMenuItem4);

            mainPanel.Controls.Add(menuStrip);

            // printDataButton
            printDataButton = new ToolStripButton
            {
                Text = "Hide data",
                Tag = true,
                BackColor = Color.LightGray,
            };
            printDataButton.Click += (o, e) =>
            {
                if(printDataButton.Text == "Hide data")
                {
                    printDataButton.Text = "Show data";
                    printDataButton.Tag = false;
                }
                else if (printDataButton.Text == "Show data")
                {
                    printDataButton.Text = "Hide data";
                    printDataButton.Tag = true;
                }
            };
            menuStrip.Items.Add(printDataButton);


            // statusStrip settings
            statusStrip = new StatusStrip
            {
                Dock = DockStyle.Bottom
            };
            statusStrip.Items.Add("Labels drawing mode:");
            mainPanel.Controls.Add(statusStrip);

            // weightsPointingLabelsDrawingModeButton
            weightsPointingLabelsDrawingModeButton = new ToolStripButton
            {
                Text = "Near diagram",
                Tag = "Near",
                BackColor = Color.Gray,
            };
            weightsPointingLabelsDrawingModeButton.Click += (o, e) =>
            {
                if (weightsPointingLabelsDrawingModeButton.Tag.ToString() == "Near")
                {
                    weightsPointingLabelsDrawingModeButton.Text = "Apart diagram";
                    weightsPointingLabelsDrawingModeButton.Tag = "Apart";
                    DrawWeightDiagram();
                }
                else if (weightsPointingLabelsDrawingModeButton.Tag.ToString() == "Apart")
                {
                    weightsPointingLabelsDrawingModeButton.Text = "Near diagram";
                    weightsPointingLabelsDrawingModeButton.Tag = "Near";
                    DrawWeightDiagram();
                }
            };
            statusStrip.Items.Add(weightsPointingLabelsDrawingModeButton);


        }

    }
}
