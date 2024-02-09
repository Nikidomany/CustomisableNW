using System.Collections.Generic;
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




        void DrawDiagram()
        {
            Pen axisPen = new Pen(Color.Black, 2);
            Pen errorPen = new Pen(Color.Red, 2);

            Graphics diagram = diagramPB.CreateGraphics();
            diagram.Clear(Color.White);

            DrawAxles();
            DrawGraphAxesDivisions();
            if (net == null)
                return;
            DrawErrorDiagram();
            //DrawWeightsDiagrams();



            void DrawAxles()
            {
                // Ox
                diagram.DrawLine(
                    axisPen,
                    schemePB.Width * 2 / 20,
                    schemePB.Height * 1 / 2,
                    schemePB.Width * 19 / 20,
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
                Point scaleLabelsStartPosition = new Point(0, diagramPB.Height * 1 / 20);
                Point scaleLinesStartPosition = new Point(diagramPB.Width * 2 / 20 - 5, diagramPB.Height * 1 / 20);
                int xInterval = diagramPB.Width * 18 / 20 / 50,
                    yInterval = diagramPB.Height * 18 / 20 / 20;

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
                        Location = new Point(scaleLabelsStartPosition.X + diagramPB.Width * 1 / 20, scaleLabelsStartPosition.Y + yInterval * i - 9)
                    };
                    diagramPB.Controls.Add(scaleLabel);

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
                    diagramPB.Controls.Add(scaleLabel);

                    diagram.DrawLine(axisPen, scaleLinesStartPosition.X, scaleLinesStartPosition.Y + yInterval * i + 1, scaleLinesStartPosition.X + 9, scaleLinesStartPosition.Y + yInterval * i);

                }

                for (int i = 1; i < 49; i++)
                {
                    int x1 = diagramPanel.Width * 2 / 20 + xInterval * i,
                        y1 = diagramPanel.Height / 2 - (i % 5 == 0 ? 6 : 3),
                        x2 = x1,
                        y2 = diagramPanel.Height / 2 + (i % 5 == 0 ? 6 : 3);

                    diagram.DrawLine(new Pen(Color.Black, (i % 5 == 0 ? (float)2 : 1)), x1, y1, x2, y2);
                }


            }

            void DrawErrorDiagram()
            {
                List<float> errorList = net.ErrorList;

                Point O = new Point(diagramPB.Width * 2 / 20, diagramPB.Height * 1 / 2);
                int xDiagramRange = diagramPB.Width * 18 / 20,
                    yDiagramRange = diagramPB.Height * 9 / 20;

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

            void DrawWeightsDiagrams()
            {
                Point O = new Point(diagramPB.Width * 2 / 20, diagramPB.Height * 1 / 2);
                int xDiagramRange = diagramPB.Width * 9 / 20,
                    yDiagramRange = diagramPB.Height * 9 / 20;
                int xInterval = xDiagramRange / 50;

                for (int i = 0; i < net.Neurons.Count; i++)
                    for (int j = 0; j < net.Neurons[i].Count; j++)
                        for (int k = 0; k < net.Neurons[i][j].Weights.Count; k++)
                        {
                            List<Weight> weights = net.Neurons[i][j].Weights;

                            for (int l = 1; l < weights.Count; l++)
                            {
                                int x1 = O.X + xInterval * (i - 1),
                                    y1 = O.Y - (int)(yDiagramRange * weights[l - 1].Value / 10),
                                    x2 = O.X + xInterval * i,
                                    y2 = O.Y - (int)(yDiagramRange * weights[l].Value / 10);

                                diagram.DrawLine(new Pen(Color.Blue, 1), x1, y1, x2, y2);
                            }
                        }
            } // не готов
        }

    }
}
