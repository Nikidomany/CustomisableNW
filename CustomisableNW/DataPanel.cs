using System;
using System.Drawing;
using System.Text;
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
                Font = new Font("Arial", 13),
                ScrollBars = ScrollBars.Vertical
            };
            dataPanel.Controls.Add(dataTextBox);

        }

        private void PrintActivations()
        {
            string result = "\r\n\r\nNeural activations:";

            result += "\r\n             ";
            for (int i = 0; i < net.Neurons.Count; i++)
            {
                string layerName = (i == 0 ? "Inp" : (i == net.Neurons.Count - 1 ? " Out" : $"  {i}-H "));
                result += $"  {layerName} ";
            }

            int maxNeuronsQuantity = 0;
            for (int i = 0; i < net.Neurons.Count; i++)
                if (net.Neurons[i].Count > maxNeuronsQuantity)
                    maxNeuronsQuantity = net.Neurons[i].Count;

            for (int j = 0; j < maxNeuronsQuantity; j++)
            {
                result += $"\r\n        N{j} ";

                for (int i = 0; i < net.Neurons.Count; i++)
                {
                    bool isExist = j < net.Neurons[i].Count;
                    if (!isExist)
                    {
                        result += $"    -     ";
                        continue;
                    }

                    float activationValue = net.Neurons[i][j].Activation;
                    string activationString = Math.Round(activationValue, 2).ToString();

                    result += $"   {activationString}  ";
                }

            }

            dataTextBox.Text += result;
        }
        private void PrintWeights()
        {
            string result = "";

            result += "\r\n\r\nWeights:";

            for (int i = 1; i < net.Neurons.Count; i++)
            {
                result += $"\r\n   • {(i == 1 ? "input layer - 1" : (i == net.Neurons.Count - 1 ? $"{i - 1} layer - output" : $"{i - 1} layer - {i}"))} layer";
                result += "\r\n           ";

                for (int x = 0; x < net.Neurons[i].Count; x++)
                    result += $"     N{i}{x}  ";

                for (int j = 0; j < net.Neurons[i - 1].Count; j++)
                {
                    result += $"\r\n        N{i - 1}{j}";

                    for (int k = 0; k < net.Neurons[i].Count; k++)
                    {
                        double weightValue = Math.Round(net.Neurons[i][k].Weights[j].Value, 2);
                        string sign = (weightValue < 0 ? "" : " ");

                        result += $" {sign}{weightValue} ";
                    }

                }

            }

            dataTextBox.Text += result;
        }
        private void PrintNeurosDelta()
        {
            string result = "\r\n\r\nNeurons delta:";

            result += "\r\n             ";
            for (int i = 0; i < net.Neurons.Count; i++)
            {
                string layerName = (i == 0 ? "Inp" : (i == net.Neurons.Count - 1 ? " Out" : $"  {i}-H "));
                result += $"  {layerName} ";
            }

            int maxNeuronsQuantity = 0;
            for (int i = 0; i < net.Neurons.Count; i++)
                if (net.Neurons[i].Count > maxNeuronsQuantity)
                    maxNeuronsQuantity = net.Neurons[i].Count;

            for (int j = 0; j < maxNeuronsQuantity; j++)
            {
                result += $"\r\n        N{j} ";

                for (int i = 0; i < net.Neurons.Count; i++)
                {
                    bool isExist = j < net.Neurons[i].Count;
                    if (!isExist)
                    {
                        result += $"    -     ";
                        continue;
                    }

                    float deltaValue = net.Neurons[i][j].Delta;
                    string deltaString = Math.Round(deltaValue, 2).ToString();

                    result += $"   {deltaString}  ";
                }

            }

            dataTextBox.Text += result;
        }
        private void PrintWeightsGradient()
        {
            string result = "";

            result += "\r\n\r\nWeights GRADients:";

            for (int i = 1; i < net.Neurons.Count; i++)
            {
                result += $"\r\n   • {(i == 1 ? "input layer - 1" : (i == net.Neurons.Count - 1 ? $"{i - 1} layer - output" : $"{i - 1} layer - {i}"))} layer";
                result += "\r\n           ";

                for (int x = 0; x < net.Neurons[i].Count; x++)
                    result += $"     N{i}{x}  ";

                for (int j = 0; j < net.Neurons[i - 1].Count; j++)
                {
                    result += $"\r\n        N{i - 1}{j}";
                    for (int k = 0; k < net.Neurons[i].Count; k++)
                    {
                        float gradientValue = net.Neurons[i][k].Weights[j].Gradient;
                        string sign = (gradientValue < 0 ? "" : " ");

                        result += $" {sign}{Math.Round(gradientValue, 2)} ";
                    }

                }

            }

            dataTextBox.Text += result;
        }
        private void PrintWeightsDelta()
        {
            string result = "";

            result += "\r\n\r\nWeights delta:";

            for (int i = 1; i < net.Neurons.Count; i++)
            {
                result += $"\r\n   • {(i == 1 ? "input layer - 1" : (i == net.Neurons.Count - 1 ? $"{i - 1} layer - output" : $"{i - 1} layer - {i}"))} layer";
                result += "\r\n           ";

                for (int x = 0; x < net.Neurons[i].Count; x++)
                    result += $"     N{i}{x}  ";

                for (int j = 0; j < net.Neurons[i - 1].Count; j++)
                {
                    result += $"\r\n        N{i - 1}{j}";
                    for (int k = 0; k < net.Neurons[i].Count; k++)
                    {
                        float deltaValue = net.Neurons[i][k].Weights[j].Delta;
                        string sign = (deltaValue < 0 ? "" : " ");

                        result += $" {sign}{Math.Round(deltaValue, 2)} ";
                    }

                }

            }

            dataTextBox.Text += result;
        }
        private void PrintError()
        {
            StringBuilder result = new StringBuilder("\r\n");
            double error = Math.Round(net.ErrorList[net.ErrorList.Count - 1], 3);

            result.Append($"ERROR: {error}\r\n");

            dataTextBox.Text += result;
        }



    }

}
