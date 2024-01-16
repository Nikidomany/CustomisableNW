using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{

    public partial class MainForm
    {
        Panel diagramPanel = new Panel();
        PictureBox diagramPB = new PictureBox();

        // график значений весов
        void DiagramPanelGraphics()
        {
            // diagramPanel.Tag = "diagram";
            diagramPanel.Visible = false;
            diagramPanel.Location = new Point(0, menuStrip.Height);
            diagramPanel.Size = new Size(mainPanel.Width, mainPanel.Height - menuStrip.Height - statusStrip.Height);
            mainPanel.Controls.Add(diagramPanel);

            diagramPB.Dock = DockStyle.Fill;
            diagramPB.BorderStyle = BorderStyle.Fixed3D;
            /*с
             * 
             * 
             * 
             * 
             */
            diagramPanel.Controls.Add(diagramPB);
        }


        Pen axisPen = new Pen(Color.Black, 2);



        void DiagramDrawing()
        {
            Graphics diagram = diagramPB.CreateGraphics();

            //axis drawing
            diagram.DrawLine
            (
                axisPen,
                schemePB.Width * 1 / 20,
                schemePB.Height * 1 / 20,
                schemePB.Width * 1 / 20,
                schemePB.Height * 19 / 20
            );
            diagram.DrawLine
            (
                axisPen,
                schemePB.Width * 1 / 20,
                schemePB.Height * 1 / 2,
                schemePB.Width * 19 / 20,
                schemePB.Height * 1 / 2
            );

            // error diagram drawing

        }

    }
}
