using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{
    public partial class MainForm
    {
        Panel dataPanel;
        TextBox dataTextBox;

        //вывод численных данных 
        void DataPanelGraphics()
        {
            // dataPanel settings
            dataPanel = new Panel
            {
                Visible = true,
                Location = new Point(0, menuStrip.Height),
                Size = new Size(mainPanel.Width, mainPanel.Height - menuStrip.Height - statusStrip.Height)
            };
            mainPanel.Controls.Add(dataPanel);

            // dataTextBox settings
            dataTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                Font = new Font("Arial", 15),
                ScrollBars = ScrollBars.Vertical
            };
            dataPanel.Controls.Add(dataTextBox);

        }





    }

}
