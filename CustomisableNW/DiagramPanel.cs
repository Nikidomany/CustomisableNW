using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{

    public partial class MainForm
    {
        Panel errorDiagramPanel = new Panel();
        PictureBox errorDiagramPB = new PictureBox();

        // график значений весов
        void ErrorDiagramPanelGraphics()
        {
            errorDiagramPanel.Visible = false;
            errorDiagramPanel.Location = new Point(0, menuStrip.Height);
            errorDiagramPanel.Size = new Size(mainPanel.Width, mainPanel.Height - menuStrip.Height - statusStrip.Height);
            mainPanel.Controls.Add(errorDiagramPanel);

            errorDiagramPB.Dock = DockStyle.Fill;
            errorDiagramPB.BorderStyle = BorderStyle.Fixed3D;
            /*с
             * 
             * 
             * 
             * 
             */
            errorDiagramPanel.Controls.Add(errorDiagramPB);
        }




        void DrawErrorDiagram()
        {
            Pen axisPen = new Pen(Color.Black, 2);
            Pen errorPen = new Pen(Color.Red, 2);

            Graphics diagram = errorDiagramPB.CreateGraphics();
            diagram.Clear(Color.White);

            DrawAxles();
            DrawGraphAxesDivisions();
            if (net == null)
                return;
            DrawErrorDiagram();


            void DrawAxles()
            {
                // Ox
                diagram.DrawLine(
                    axisPen,
                    schemePB.Width * 2 / 20,
                    schemePB.Height * 1 / 2,
                    schemePB.Width,
                    schemePB.Height * 1 / 2
                    );

                // Oy
                diagram.DrawLine(
                    axisPen,
                    schemePB.Width * 2 / 20,
                    schemePB.Height * 1 / 20,
                    schemePB.Width * 2 / 20,
                    schemePB.Height * 19 / 20
                    );
            }
            void DrawGraphAxesDivisions()
            {
                Point scaleLabelsStartPosition = new Point(0, errorDiagramPB.Height * 1 / 20);
                Point scaleLinesStartPosition = new Point(errorDiagramPB.Width * 2 / 20 - 5, errorDiagramPB.Height * 1 / 20);
                int xInterval = errorDiagramPB.Width * 18 / 20 / 50,
                    yInterval = errorDiagramPB.Height * 18 / 20 / 20;

                for (int i = 0; i < 11; i++)
                {
                    Label scaleLabel = new Label
                    {
                        Text = $"{(double)(1.0 - 0.1 * i)}",
                        TextAlign = ContentAlignment.MiddleRight,
                        ForeColor = Color.Black,
                        BackColor = Color.White,
                        Font = new Font(font, 12),
                        Size = new Size(40, 20),
                        Location = new Point(scaleLabelsStartPosition.X + errorDiagramPB.Width * 1 / 20, scaleLabelsStartPosition.Y + yInterval * i - 9)
                    };
                    errorDiagramPB.Controls.Add(scaleLabel);

                    diagram.DrawLine(axisPen, scaleLinesStartPosition.X, scaleLinesStartPosition.Y + yInterval * i + 1, scaleLinesStartPosition.X + 9, scaleLinesStartPosition.Y + yInterval * i);
                }

                for (int i = 0; i < 21; i++)
                {
                    Label scaleLabel = new Label
                    {
                        Text = $"{(10 - 1 * i)}",
                        TextAlign = ContentAlignment.MiddleRight,
                        ForeColor = Color.Blue,
                        BackColor = Color.White,
                        Font = new Font(font, 12),
                        Size = new Size(40, 20),
                        Location = new Point(scaleLabelsStartPosition.X, scaleLabelsStartPosition.Y + yInterval * i - 9)
                    };
                    errorDiagramPB.Controls.Add(scaleLabel);

                    diagram.DrawLine(axisPen, scaleLinesStartPosition.X, scaleLinesStartPosition.Y + yInterval * i + 1, scaleLinesStartPosition.X + 9, scaleLinesStartPosition.Y + yInterval * i);

                }

                for (int i = 1; i < 51; i++)
                {
                    int x1 = errorDiagramPanel.Width * 2 / 20 + xInterval * i,
                        y1 = errorDiagramPanel.Height / 2 - (i % 5 == 0 ? 6 : 3),
                        x2 = x1,
                        y2 = errorDiagramPanel.Height / 2 + (i % 5 == 0 ? 6 : 3);

                    if(i % 5 == 0)
                    {
                        Label scaleLabel = new Label
                        {
                            Text = $"{(double)i}",
                            TextAlign = ContentAlignment.MiddleCenter,
                            ForeColor = Color.Black,
                            BackColor = Color.White,
                            Font = new Font(font, 12),
                            Size = new Size(30, 20),
                            Location = new Point(x2 - 13, y1 + 12)
                        };
                        errorDiagramPB.Controls.Add(scaleLabel);
                    }


                    diagram.DrawLine(new Pen(Color.Black, (i % 5 == 0 ? (float)2 : 1)), x1, y1, x2, y2);
                }


            }
            void DrawErrorDiagram()
            {
                List<float> errorList = net.ErrorList;

                Point O = new Point(errorDiagramPB.Width * 2 / 20, errorDiagramPB.Height * 1 / 2);
                int xDiagramRange = errorDiagramPB.Width * 18 / 20,
                    yDiagramRange = errorDiagramPB.Height * 9 / 20;

                int xInterval = xDiagramRange / 50;

                for (int i = 1; i < errorList.Count; i++)
                {
                    int x1 = O.X + xInterval * (i - 1),
                        y1 = O.Y - (int)(yDiagramRange * errorList[i - 1]),
                        x2 = O.X + xInterval * i,
                        y2 = O.Y - (int)(yDiagramRange * errorList[i]);

                    diagram.DrawLine(errorPen, x1, y1, x2, y2);
                }

            }
        }

    }
}
