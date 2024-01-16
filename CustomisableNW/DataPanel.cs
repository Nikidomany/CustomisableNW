using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{
    public partial class MainForm
    {
        Panel dataPanel = new Panel();
        TextBox dataTextBox = new TextBox();

        //вывод численных данных 
        void DataPanelGraphics()
        {
            dataPanel.Visible = true;
            dataPanel.Location = new Point(0, menuStrip.Height);
            dataPanel.Size = new Size(mainPanel.Width, mainPanel.Height - menuStrip.Height - statusStrip.Height);
            mainPanel.Controls.Add(dataPanel);

            dataTextBox.Dock = DockStyle.Fill;
            dataTextBox.Multiline = true;
            dataTextBox.Font = new Font("Arial", 15);
            dataTextBox.ScrollBars = ScrollBars.Vertical;
            //dataTextBox.Text = "Text";
            dataPanel.Controls.Add(dataTextBox);

        }





    }

}
