using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{
    public partial class MainForm
    {
        Panel schemePanel;
        PictureBox schemePB;

        //public List<List<Label>> ActivationLabelsList { get => activationLabelsList; set => activationLabelsList = value; }
        private List<List<Label>> activationLabelsList = new List<List<Label>>();


        // графическое представление сети
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
                BorderStyle = BorderStyle.Fixed3D
            };
            schemePanel.Controls.Add(schemePB);


        }


        Pen inputNeuronPen = new Pen(Color.Red, 2);
        Pen hiddenNeuronPen = new Pen(Color.Blue, 2);
        Pen outputNeuronPen = new Pen(Color.Red, 2);
        Pen weightPen = new Pen(Color.Black, 1);
        private void Drawing()
        {
            Graphics scheme = schemePB.CreateGraphics(); // область построения
            scheme.Clear(schemePanel.BackColor);

            int nDiameter = schemePB.Height / 20; ; // диаметр круга нейронов
            int vDis = nDiameter * 2, hDis = nDiameter * 4; // растония между слоями и нейронами в слоях
            int x = -nDiameter / 2, y = -nDiameter / 2; //смещения для отрисовки груга нейрона по координатам не края, а центра
            int horisontalStartPosition = (schemePB.Width - (nDiameter + hDis) * (hiddenLayersNum + 1)) / 2; //Начальная позиция отрисовки первого слоя по оси X
            int[] verticalStartPosition = new int[4];
            for (int i = 0; i < neuronsPerHiddenLayer.Count; i++)
                verticalStartPosition[i] = schemePB.Height / 2 - nDiameter * (neuronsPerHiddenLayer[i] - 1);  //Начальная позиция отрисовки скрытых слоёв по оси Y (упрощённая выведенная формула)

            // input neurons drowing
            for (int i = 0; i < 2; i++)
                scheme.DrawEllipse(
                    inputNeuronPen,
                    horisontalStartPosition + x,
                    schemePB.Height / 2 + (i == 0 ? 1 : -1) * nDiameter + y,
                    nDiameter,
                    nDiameter);

            // hidden layers drowing
            for (int i = 0; i < hiddenLayersNum; i++) // i - layer number
                for (int j = 0; j < neuronsPerHiddenLayer[i]; j++) // j - neuron number
                {
                    int xCoordinate = horisontalStartPosition + hDis * (i + 1) + x; //расстояние от края до входного слоя + i расстояний до скрытого слоя + x(коррекция)
                    int yCoordinate = verticalStartPosition[i] + vDis * j + y;   //расстояние от верхнего края schemePB до верхнего нейрона скрытого слоя + j расстояний между центрами нейронов

                    scheme.DrawEllipse(
                        hiddenNeuronPen,
                        xCoordinate,
                        yCoordinate,
                        nDiameter,
                        nDiameter);

                    if (i == 0) //1st hidden layer
                    {
                        for (int k = 0; k < 2; k++)
                            scheme.DrawLine(
                                weightPen,
                                horisontalStartPosition + nDiameter / 2,     //X1
                                schemePB.Height / 2 - nDiameter + vDis * k,  //Y1
                                xCoordinate,                                 //X2
                                yCoordinate + nDiameter / 2                  //Y2
                                );
                    }
                    else if (i > 0) //2nd and others hidden layers
                    {
                        for (int k = 0; k < neuronsPerHiddenLayer[i - 1]; k++)
                            scheme.DrawLine(
                                weightPen,
                                xCoordinate - hDis + nDiameter,                               //X1
                                (verticalStartPosition[i - 1] + vDis * j + y) + nDiameter / 2 + vDis * (k - j),                 //Y1
                                xCoordinate,                                                  //X2
                                verticalStartPosition[i] + vDis * j//Y2             
                                );

                    }

                    if (i == hiddenLayersNum - 1)
                        scheme.DrawLine(
                            weightPen,
                            xCoordinate + nDiameter,
                            yCoordinate + nDiameter / 2,
                            horisontalStartPosition + hDis * (hiddenLayersNum + 1) - nDiameter / 2,
                            schemePB.Height / 2
                            );
                }

            // output neuron drawing
            scheme.DrawEllipse(
                outputNeuronPen,
                horisontalStartPosition + hDis * (hiddenLayersNum + 1) + x,
                schemePB.Height / 2 + y,
                nDiameter,
                nDiameter);


        }

    }

}
