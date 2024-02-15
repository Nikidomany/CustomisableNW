using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{
    public partial class MainForm
    {
        Panel weightsDiagramPanel;
        MenuStrip layersMenuStrip;
        PictureBox weightsDiagramPB;

        void WeightsDiagramPanelGraphics()
        {
            weightsDiagramPanel = new Panel
            {
                Visible = false,
                Location = new Point(0, menuStrip.Height),
                Size = new Size(mainPanel.Width, mainPanel.Height - menuStrip.Height - statusStrip.Height),
            };
            mainPanel.Controls.Add(weightsDiagramPanel);

            layersMenuStrip = new MenuStrip
            {
                Dock = DockStyle.Top
            };
            weightsDiagramPanel.Controls.Add(layersMenuStrip);

            weightsDiagramPB = new PictureBox
            {
                Location = new Point(0, layersMenuStrip.Height),
                Size = new Size(weightsDiagramPanel.Width, weightsDiagramPanel.Height - layersMenuStrip.Height),
                BackColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            weightsDiagramPanel.Controls.Add(weightsDiagramPB);
        }

        void DrawWeightDiagram(Neuron neuron = null)
        {
            Graphics diagram = weightsDiagramPB.CreateGraphics();
            Pen axisPen = new Pen(Color.Black, 2);
            List<Pen> weightsPens = new List<Pen>
            {
                new Pen(Color.Red, 1),
                new Pen(Color.Green, 1),
                new Pen(Color.Blue, 1),
                new Pen(Color.Yellow, 1),
                new Pen(Color.Black, 1),
                new Pen(Color.Pink, 1),
                new Pen(Color.Orange, 1),
                new Pen(Color.Purple, 1),
                new Pen(Color.Gray, 1),
                new Pen(Color.Olive, 1)
            };

            ClearWeigthDiagram();
            DrawWeightDiagramAxis();
            DrawWeightDiagramAxisDivisions();
            if (neuron == null || neuron.Weights.Count < 2)
                return;
            DrawWeightDiagram();
            DrawDiagramLegendApart();



            void ClearWeigthDiagram()
            {
                diagram.Clear(Color.White);
            }
            void DrawWeightDiagramAxis()
            {
                Pen axesPen = new Pen(Color.Black, 2);

                // X axis
                diagram.DrawLine(
                    axesPen,
                    weightsDiagramPB.Width * 1 / 20,
                    weightsDiagramPB.Height / 2,
                    weightsDiagramPB.Width * 19 / 20,
                    weightsDiagramPB.Height / 2
                    );
                // Y axis
                diagram.DrawLine(
                    axesPen,
                    weightsDiagramPB.Width * 1 / 20,
                    weightsDiagramPB.Height * 1 / 20,
                    weightsDiagramPB.Width * 1 / 20,
                    weightsDiagramPB.Height * 19 / 20
                    );
            }
            void DrawWeightDiagramAxisDivisions()
            {
                Pen boldDivivionPen = new Pen(Color.Black, 2);
                Pen thinDivivionPen = new Pen(Color.Black, 1);

                // X axis
                Point xAxisStartPoint = new Point(weightsDiagramPB.Width * 1 / 20, weightsDiagramPB.Height / 2);
                int xStep = weightsDiagramPB.Width * 18/20 / 49;
                for(int i = 1; i < 51; i++)
                {
                    int x1 = xAxisStartPoint.X + xStep * i,
                        y1 = xAxisStartPoint.Y - (i % 5 == 0 ? 6 : 3),
                        x2 = x1,
                        y2 = xAxisStartPoint.Y + (i % 5 == 0 ? 6 : 3);
                    Pen pen = i % 5 == 0 ? boldDivivionPen : thinDivivionPen;
                    diagram.DrawLine(pen, x1, y1, x2, y2);

                    if (i % 5 == 0)
                    {
                        Label dividionLabel = new Label
                        {
                            Text = i.ToString(),
                            TextAlign = ContentAlignment.MiddleCenter,
                            ForeColor = Color.Black,
                            BackColor = Color.Transparent,
                            Font = new Font(font, 12),
                            Size = new Size(30, 20),
                            Location = new Point(xAxisStartPoint.X + xStep * i - 13, xAxisStartPoint.Y + 15)
                        };
                        weightsDiagramPB.Controls.Add(dividionLabel);
                    }
                }

                // Y axis
                Point yAxisStartPoint = new Point(weightsDiagramPB.Width * 1 / 20, weightsDiagramPB.Height * 1 / 20);
                int yStep = weightsDiagramPB.Height * 18/20 / 19;
                for(int i = 0; i < 21; i++)
                {
                    int x1 = yAxisStartPoint.X - 3,
                        y1 = yAxisStartPoint.Y + yStep * i,
                        x2 = yAxisStartPoint.X + 3,
                        y2 = y1;
                    diagram.DrawLine(boldDivivionPen, x1, y1, x2, y2);

                    Label dividionLabel = new Label
                    {
                        Text = (10 - i).ToString(),
                        TextAlign = ContentAlignment.MiddleRight,
                        ForeColor = Color.Black,
                        BackColor = Color.White,
                        Font = new Font(font, 12),
                        Size = new Size(40, 20),
                        Location = new Point(0, yAxisStartPoint.Y + yStep * i - 10)
                    };
                    weightsDiagramPB.Controls.Add(dividionLabel);
                }                           
            }
            void DrawWeightDiagram()
            {
                Point pointZero = new Point(weightsDiagramPB.Width * 1 / 20, weightsDiagramPB.Height / 2);
                int xRange = weightsDiagramPB.Width * 18 / 20,
                    yRange = weightsDiagramPB.Height * 9 / 20;
                int xStep = xRange / 50;

                for (int i = 0; i < neuron.Weights.Count; i++)
                    for (int j = 1; j < neuron.Weights[i].ValuesList.Count && j < 50; j++)
                    {
                        float weightValue1 = neuron.Weights[i].ValuesList[j - 1],
                              weightValue2 = neuron.Weights[i].ValuesList[j];
                        int x1 = pointZero.X + xStep * (j - 1),
                            y1 = pointZero.Y - (int)(weightValue1 / 10 * yRange),
                            x2 = pointZero.X + xStep * j,
                            y2 = pointZero.Y - (int)(weightValue2 / 10 * yRange);

                        diagram.DrawLine(weightsPens[i], x1, y1, x2, y2);
                    }
            }
            
            void DrawDiagramLegendNextToDiagram()
            {
                int weightsQuantity = neuron.Weights.Count;
                List<float> weightsValue = new List<float>();
                for(int i = 0; i < weightsQuantity; i++)
                {
                    float weightValue = neuron.Weights[i].Value;
                    weightsValue.Add(weightValue);
                }



                // выносные линии под углом


                for(int i = 0; i < weightsQuantity; i++)
                {

                }
                for (int i = 0; i < weightsQuantity; i++)
                {

                }


            }
            void DrawDiagramLegendApart()
            {
                int lineSpacing = 30;
                int weightsQuantity = neuron.Weights.Count;
                Point startPoint = new Point(weightsDiagramPB.Width * 17/20, weightsDiagramPB.Height - lineSpacing * weightsQuantity);
                
                // Color line drawing
                for(int i = 0; i < weightsQuantity; i++)
                {
                    Pen pen = weightsPens[i];
                    int x1 = startPoint.X,
                        y1 = startPoint.Y + lineSpacing * i,
                        x2 = x1 + 40,
                        y2 = y1;

                    diagram.DrawLine(pen, x1, y1, x2, y2);
                }
                // Line labels writing
                for (int i = 0; i < weightsQuantity; i++)
                {
                    int x = startPoint.X + 40,
                        y = startPoint.Y + lineSpacing * i - 15;
                    string labelText = $"w{i}";
                    Label label = new Label
                    {
                        Text = labelText,
                        Font = new Font("Arial", 13),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(x, y),
                        Size = new Size(40, 20)
                    };

                    weightsDiagramPB.Controls.Add(label);
                }
            }

        }
        void InitilizeLayersMenuStripItems()
        {
            AddItems();
            AddNeuralNamesInItems();


            void AddItems()
            {
                for (int i = 0; i < hiddenLayersNum + 1; i++)
                {
                    string layerNames = "";
                    if (i == 0)
                        layerNames = " inp-1h ";
                    else if (i > 0 && i < hiddenLayersNum)
                        layerNames = $" {i + 1}h - {i + 2}h ";
                    else if (i < hiddenLayersNum + 1)
                        layerNames = $" {hiddenLayersNum}h - out ";

                    ToolStripDropDownButton listOfNeuronsInLayer = new ToolStripDropDownButton(layerNames);
                    listOfNeuronsInLayer.MouseEnter += (o, e) => listOfNeuronsInLayer.ShowDropDown();
                    layersMenuStrip.Items.Add(listOfNeuronsInLayer);
                }
            }
            void AddNeuralNamesInItems()
            {
                for (int i = 0; i < hiddenLayersNum + 1; i++)
                {
                    ToolStripDropDownButton dropDownButton = (ToolStripDropDownButton)layersMenuStrip.Items[i];
                    for (int j = 0; j < net.Neurons[i + 1].Count; j++)
                    {
                        string itemText = $"to N{i + 1}{j}";
                        ToolStripMenuItem item = new ToolStripMenuItem(itemText);
                        item.Tag = net.Neurons[i + 1][j];
                        item.Click += (o, e) => DrawWeightDiagram((Neuron)item.Tag);
                        dropDownButton.DropDownItems.Add(item);
                    }
                }
            }
        }
        void CleanLayersMenuStripItems()
        {
            layersMenuStrip.Items.Clear();
        }
    }
}

