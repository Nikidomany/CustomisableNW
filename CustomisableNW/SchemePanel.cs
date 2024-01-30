using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace CustomisableNW
{
    public partial class MainForm
    {
        

        Panel schemePanel;
        PictureBox schemePB;
        

        List<List<Label>> neuronLabels;

        private List<List<Point>> neuronsCoordinates;

        private List<List<Label>> activationLabelsList = new List<List<Label>>();


        
        void SchemePanelGraphics()
        {


            // schemePanel settings
            schemePanel = new Panel
            {
                Visible = false,
                Location = new Point(0, menuStrip.Height),
                Size = new Size(mainPanel.Width, mainPanel.Height - menuStrip.Height - statusStrip.Height)
            };
            mainPanel.Controls.Add(schemePanel);

            // scheme PictureBox settings
            schemePB = new PictureBox
            {
                Size = new Size(schemePanel.Width, schemePanel.Height),
                BorderStyle = BorderStyle.Fixed3D, 
                BackColor = Color.White
            };
            schemePanel.Controls.Add(schemePB);


        }

        private void NeuronsCoordinatesComputing()
        {
            // INPUT: schemePanel.Size, layers quantity, neurons per layer quantity
            // OUTPUT: List<List<Point>> neuronCoordinates

            List<List<Point>> result = new List<List<Point>>();

            int neuronDiameter = schemePB.Height / 20;

            int xDistance = neuronDiameter * 4, // distance between neurons
                yDistance = neuronDiameter * 2;

            int xStartPosition = (schemePB.Width - xDistance * (hiddenLayersNum + 2)) / 2;
            int[] yStartPosition = new int[hiddenLayersNum + 2];
            for (int i = 0; i < yStartPosition.Length; i++)
                yStartPosition[i] = (schemePB.Height - yDistance * neuronsPerLayer[i]) / 2;
                
            // coordinates computing
            for (int i = 0; i < hiddenLayersNum + 2; i++)
            {
                result.Add(new List<Point>());
                for (int j = 0; j < neuronsPerLayer[i] ; j++)
                {
                    int x = xStartPosition + xDistance * i,
                        y = yStartPosition[i] + yDistance * j;

                    result[i].Add(new Point(x,y));
                }
            }
            neuronsCoordinates = result;
        }

        private void Drawing()
        {
            NeuronsCoordinatesComputing();

            Graphics scheme = schemePB.CreateGraphics();
            scheme.Clear(Color.White);
            
            Pen inputOrOutputNeuronPen = new Pen(Color.Red, 2);
            Pen hiddenNeuronPen = new Pen(Color.Blue, 2);
            Pen weightPen = new Pen(Color.Black, 1);

            int neuronDiameter = schemePB.Height / 15;
            int xCorrection = -neuronDiameter/2, // for drawing neurons from center, not from top left angle
                yCorrection = -neuronDiameter/2;

            for(int i = 0; i < neuronsPerLayer.Count; i++)
                for(int j = 0; j < neuronsPerLayer[i] ; j++)
                {
                    //neuron drawing
                    Pen neuronPen = (i == 0 || i == neuronsPerLayer.Count - 1) ? inputOrOutputNeuronPen : hiddenNeuronPen;
                    int x = neuronsCoordinates[i][j].X + xCorrection,
                        y = neuronsCoordinates[i][j].Y + yCorrection + 30;
                    int diameter = neuronDiameter;
                    
                    scheme.DrawEllipse(neuronPen, x, y, diameter, diameter);


                    //weights drawing
                    if (i > 0)
                        for (int k = 0; k < neuronsPerLayer[i - 1]; k++)
                        {
                            int x1 = neuronsCoordinates[i - 1][k].X + neuronDiameter / 2,
                                y1 = neuronsCoordinates[i - 1][k].Y + 30,
                                x2 = neuronsCoordinates[i][j].X - neuronDiameter / 2,
                                y2 = neuronsCoordinates[i][j].Y + 30;
                           
                            scheme.DrawLine(weightPen, x1, y1, x2, y2);
                        }
                }

            LabelsDrawing();
        }

        private void LabelsDrawing()
        {
            neuronLabels = new List<List<Label>>();
            schemePB.Controls.Clear();

            for (int i = 0; i < neuronsPerLayer.Count; i++)
            {
                neuronLabels.Add(new List<Label>());
                for(int j = 0; j < neuronsPerLayer[i]; j++)
                {
                    Label label = CreateNeuronLabel(neuronsCoordinates[i][j].X, neuronsCoordinates[i][j].Y);
                    neuronLabels[i].Add(label);
                    schemePB.Controls.Add(label);
                }
            }


            Label CreateNeuronLabel(int x, int y)
            {
                return new Label
                {
                    Text = "-",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(font, 11),
                    Size = new Size(40, 14),
                    Location = new Point(x - 19, y + 23)
                };
            }
        }
        public void UpdateNeuronLabels()
        {
            for(int i = 0; i < neuronLabels.Count; i++)
                for(int j = 0; j < neuronLabels[i].Count; j++)
                    neuronLabels[i][j].Text = Math.Round(net.Neurons[i][j].Activation,2).ToString();
        }

        

    }

}
