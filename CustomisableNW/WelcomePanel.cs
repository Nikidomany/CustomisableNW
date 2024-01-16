using System.Drawing;
using System.Windows.Forms;


namespace CustomisableNW
{
    // Welcome panel graphics
    public partial class MainForm : Form
    {
        private Panel welcomePanel = new Panel();
        private Label welcomelabel = new Label();
        private Button buttonStart = new Button();

        public MainForm()
        {
            // form settings
            MainForm form = this;
            form.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            form.WindowState = FormWindowState.Maximized;

            WelcomePanelGraphics();
            MainPanelGraphics();
            DataPanelGraphics();
            DiagramPanelGraphics();
            SettingsPanelGraphics();
            SchemePanelGraphics();

            //// ВРЕМЕННО
            //buttonStart.Visible = true;
            //buttonStart.Enabled = true;
        }

        void WelcomePanelGraphics()
        {
            // welcomePanel. settings
            welcomePanel.Dock = DockStyle.Fill;
            string picturePath = @"C:\Users\Никита\Desktop\С#\NeuroWebs\CustomisableNW\CustomisableNW\Pictures\WelcomePicture.png";
            welcomePanel.BackgroundImage = Image.FromFile(picturePath);
            Controls.Add(welcomePanel);

            // label settings
            welcomelabel.Text = "Super Intelligent\nNeuro Web\nUltra Max Pro 5000+";
            welcomelabel.Font = new Font("Arial", 50);
            welcomelabel.BackColor = Color.Transparent;
            welcomelabel.Visible = false;
            welcomelabel.TextAlign = ContentAlignment.MiddleCenter;
            welcomelabel.Width = welcomePanel.Width;
            welcomelabel.Height = 250;
            welcomelabel.Location = new Point(0, welcomePanel.Height * 1 / 10);
            welcomePanel.Controls.Add(welcomelabel);

            // button settings
            buttonStart.Visible = false;
            buttonStart.Enabled = false;
            buttonStart.BackColor = Color.LightGray;
            welcomelabel.ForeColor = Color.FromArgb(255, 255, 255);
            buttonStart.Font = new Font("Arial", welcomePanel.Width / 60);
            buttonStart.Size = new Size(welcomePanel.Width * 1 / 10, welcomePanel.Height * 1 / 10);
            buttonStart.Location = new Point(welcomePanel.Width / 2 - buttonStart.Width / 2, welcomePanel.Height * 5 / 10);
            buttonStart.MouseEnter += (o, e) => buttonStart.BackColor = Color.FromArgb(0, 50, 120);
            buttonStart.MouseLeave += (o, e) => buttonStart.BackColor = Color.LightGray;
            buttonStart.Click += (o, e) =>
            {
                welcomePanel.Visible = false;
                mainPanel.Visible = true;
                settingsPanel.Visible = true;
            };
            welcomePanel.Controls.Add(buttonStart);

            WelcomePanelAnimanion();
        }

        void WelcomePanelAnimanion()
        {
            // label animation
            System.Windows.Forms.Timer labelAnimationTimer = new System.Windows.Forms.Timer();
            int labelAnimationRuntime = 1000;
            labelAnimationTimer.Interval = 20;
            int numberOfFames = labelAnimationRuntime / labelAnimationTimer.Interval;
            int passedNumberOfFames = 0;
            labelAnimationTimer.Tick += (a, b) =>
            {
                welcomelabel.Visible = true;
                int num = (int)(255 * passedNumberOfFames / numberOfFames);

                welcomelabel.ForeColor = Color.FromArgb(num, num, num);
                passedNumberOfFames++;

                if (passedNumberOfFames == numberOfFames)
                    labelAnimationTimer.Stop();
            };
            labelAnimationTimer.Start();

            //button animation
            System.Windows.Forms.Timer buttonAnimationTimer = new System.Windows.Forms.Timer();
            buttonAnimationTimer.Interval = 150;
            buttonStart.Visible = true;
            char[] chars = " START ".ToCharArray();
            int index = 0;
            buttonAnimationTimer.Tick += (o, e) =>
            {
                buttonStart.Text += chars[index++];
                if (index == chars.Length)
                {
                    buttonAnimationTimer.Stop();
                    buttonStart.Enabled = true;
                }
            };
            buttonAnimationTimer.Start();
        }

    }
}