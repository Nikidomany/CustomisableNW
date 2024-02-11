using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomisableNW
{
    public partial class MainForm
    {
        Net net;

        TrainingFunction trainingFunction;

        public Button runButton;
        Button setButton;

        bool[] selectedSetsList = new bool[]
        {true, true, true, true};

        List<Label> tableLabels = new List<Label>();

        Panel settingsPanel = new Panel();

        public int hiddenLayersNum = 1;
        public List<int> neuronsPerLayer = new List<int> { 2, 3, 1 };

        List<NumericUpDown> NUDList = new List<NumericUpDown>();

        public float maxWeightsRandomize = 1,
              minWeightsRandomize = -1,
              learningRate = 0.7F,
              moment = 0.3F;

        //settingsPanel settings
        void SettingsPanelGraphics()
        {
            // settingPanel
            settingsPanel.Size = new Size(this.Width * 1 / 3, this.Height);
            settingsPanel.Dock = DockStyle.Left;
            settingsPanel.BorderStyle = BorderStyle.Fixed3D;
            settingsPanel.Visible = false;
            this.Controls.Add(settingsPanel);






            // lab1 "NW properties:"
            Label lab1 = new Label
            {
                Text = "NW properties:",
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(settingsPanel.Width, 40),
                Font = new Font(font, 22),
                Location = new Point(settingsPanel.Width / 2 - 255)
            };
            settingsPanel.Controls.Add(lab1);


            // lab2 "Number of hidden layers:"
            Label lab2 = new Label
            {
                Text = "Number of hidden layers:",
                Width = settingsPanel.Width * 50 / 100,
                Font = new Font(font, 15),
                Location = new Point(10, settingsPanel.Height * 7 / 100)
            };
            settingsPanel.Controls.Add(lab2);

            // NUD number of hidden layers
            NumericUpDown hiddenlayersNUD = new NumericUpDown
            {
                Location = new Point(lab2.Location.X + lab2.Width, lab2.Location.Y),
                Width = 40,
                TextAlign = HorizontalAlignment.Center,
                Value = 1,
                Minimum = 1,
                Maximum = 4,
            };
            hiddenlayersNUD.ValueChanged += (o, e) =>
            {
                hiddenLayersNum = (int)hiddenlayersNUD.Value; // recording actual hidden layers number

                for (int i = 0; i < 4; i++)     // visualization/hiding NUDs
                    NUDList[i].Visible = (i <= hiddenlayersNUD.Value - 1) ? true : false;

                neuronsPerLayer = new List<int> { 2 }; // 2 - input neurons 
                for (int i = 0; i < hiddenlayersNUD.Value; i++) // recording actual neurons number per layer
                    neuronsPerLayer.Add((int)NUDList[i].Value);
                neuronsPerLayer.Add(1); //  - output neuron

                Drawing(); // drawing scheme according to updated data 
            };
            settingsPanel.Controls.Add(hiddenlayersNUD);


            // lab3 "Number of neurons:"
            Label lab3 = new Label
            {
                Text = "Number of neurons:",
                Width = settingsPanel.Width * 50 / 100,
                Font = new Font(font, 15),
                Location = new Point(10, settingsPanel.Height * 10 / 100)
            };
            settingsPanel.Controls.Add(lab3);

            // NUDs: number of neurons
            for (int i = 0; i < 4; i++)
            {
                NumericUpDown NUD = new NumericUpDown
                {
                    Location = new Point(lab3.Location.X + lab3.Width + 50 * i, lab3.Location.Y),
                    Width = 40,
                    TextAlign = HorizontalAlignment.Center,
                    Value = i == 0 ? 3 : 1,
                    Minimum = 1,
                    Maximum = 10,
                    Visible = i == 0 ? true : false,
                    Tag = i,
                };
                NUD.ValueChanged += (o, e) =>
                {

                    neuronsPerLayer[(int)NUD.Tag + 1] = (int)NUD.Value;
                    Drawing();
                };
                NUDList.Add(NUD);
                settingsPanel.Controls.Add(NUD);
            }


            // lab4 "Range of weights randomization:"
            Label lab4 = new Label
            {
                Text = "Range of weights\nrandomization:",
                Font = new Font(font, 15),
                Size = new Size(164, 50),
                Location = new Point(10, settingsPanel.Height * 15 / 100)
            };
            settingsPanel.Controls.Add(lab4);

            // lab5 {
            Label lab5 = new Label
            {
                Text = "{",
                Font = new Font("ISOCPEUR", 40),
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(30, 80),
                Location = new Point(10 + lab4.Width, settingsPanel.Height * 13 / 100 + 5)
            };
            settingsPanel.Controls.Add(lab5);

            // lab6 "max"
            Label lab6 = new Label
            {
                Text = "max",
                Width = 50,
                TextAlign = ContentAlignment.TopRight,
                Font = new Font(font, 15),
                Location = new Point(lab5.Location.X + lab5.Width - 7, lab5.Location.Y + 10),
            };
            settingsPanel.Controls.Add(lab6);

            // NUD maximum weights randomize
            NumericUpDown maxWeightsRandomizeNUD = new NumericUpDown
            {
                Location = new Point(lab6.Location.X + lab6.Width, lab6.Location.Y + 3),
                Width = 60,
                TextAlign = HorizontalAlignment.Center,
                Minimum = -10,
                Maximum = 10,
                Value = 0.5M,
                Increment = 0.05M,
                DecimalPlaces = 2
            };
            settingsPanel.Controls.Add(maxWeightsRandomizeNUD);

            // lab7 "min"
            Label lab7 = new Label
            {
                Text = "min",
                Width = 50,
                TextAlign = ContentAlignment.TopRight,
                Font = new Font(font, 15),
                Location = new Point(lab5.Location.X + lab5.Width - 7, lab5.Location.Y + 32)
            };
            settingsPanel.Controls.Add(lab7);

            // NUD minimum weights randomize
            NumericUpDown minWeightsRandomizeNUD = new NumericUpDown
            {
                Location = new Point(lab7.Location.X + lab7.Width, lab7.Location.Y + 3),
                Width = 60,
                TextAlign = HorizontalAlignment.Center,
                Minimum = -10,
                Maximum = 10,
                Value = -0.5M,
                Increment = 0.05M,
                DecimalPlaces = 2
            };
            settingsPanel.Controls.Add(minWeightsRandomizeNUD);


            // maxWeightsRandomizeNUD and minWeightsRandomizeNUD settings
            minWeightsRandomizeNUD.ValueChanged += (o, e) =>
            {
                if (minWeightsRandomizeNUD.Value > maxWeightsRandomizeNUD.Value)
                    minWeightsRandomizeNUD.Value = maxWeightsRandomizeNUD.Value;
                else
                    minWeightsRandomize = (float)minWeightsRandomizeNUD.Value;
            };
            maxWeightsRandomizeNUD.ValueChanged += (o, e) =>
            {
                if (maxWeightsRandomizeNUD.Value < minWeightsRandomizeNUD.Value)
                    maxWeightsRandomizeNUD.Value = minWeightsRandomizeNUD.Value;
                else
                    maxWeightsRandomize = (float)maxWeightsRandomizeNUD.Value;
            };

            // lab8 "Learning rate:"
            Label lab8 = new Label
            {
                Text = "Learning rate:",
                Width = 140,
                Font = new Font(font, 15),
                Location = new Point(10, lab4.Location.Y + lab4.Height + 20)
            };
            settingsPanel.Controls.Add(lab8);

            // NUD Learning rate
            NumericUpDown learningRateNUD = new NumericUpDown
            {
                Location = new Point(minWeightsRandomizeNUD.Location.X - 90, lab8.Location.Y),
                Width = 60,
                TextAlign = HorizontalAlignment.Center,
                Minimum = -10,
                Maximum = 10,
                Value = 0.7M,
                Increment = 0.05M,
                DecimalPlaces = 2
            };
            learningRateNUD.ValueChanged += (o, e) => learningRate = (float)learningRateNUD.Value;
            settingsPanel.Controls.Add(learningRateNUD);

            // lab9 "Moment:"
            Label lab9 = new Label
            {
                Text = "Moment:",
                Width = 120,
                //TextAlign = ContentAlignment.TopRight,
                Font = new Font(font, 15),
                Location = new Point(10, lab8.Location.Y + lab8.Height)
            };
            settingsPanel.Controls.Add(lab9);

            // NUD Moment
            NumericUpDown momentNUD = new NumericUpDown
            {
                Location = new Point(minWeightsRandomizeNUD.Location.X - 90, lab9.Location.Y),
                Width = 60,
                TextAlign = HorizontalAlignment.Center,
                Minimum = -10,
                Maximum = 10,
                Value = 0.3M,
                Increment = 0.05M,
                DecimalPlaces = 2
            };
            momentNUD.ValueChanged += (o, e) => moment = (float)momentNUD.Value;
            settingsPanel.Controls.Add(momentNUD);

            // lab10 "Learning function:"
            Label lab10 = new Label
            {
                Text = "Learning function:",
                Size = new Size(90, 50),
                Font = new Font(font, 15),
                Location = new Point(learningRateNUD.Location.Y + learningRateNUD.Width + 10,
                                        learningRateNUD.Location.Y),

            };
            settingsPanel.Controls.Add(lab10);

            // ComboBox training function
            ComboBox trainingFunctionCB = new ComboBox
            {
                Width = 80,
                Font = new Font(font, 14),
                Location = new Point(lab10.Location.X + lab10.Width, lab10.Location.Y + (lab10.Height - 20) / 2 - 4)
            };
            string[] functions = new string[3]
            {
                "XOR",
                "OR",
                "AND"
            };
            trainingFunctionCB.Items.AddRange(functions);
            trainingFunctionCB.SelectionChangeCommitted += (s, e) =>
            {
                Enum.TryParse(trainingFunctionCB.SelectedItem.ToString(), out trainingFunction);
            };
            settingsPanel.Controls.Add(trainingFunctionCB);

            // setButton
            setButton = new Button
            {
                Text = "SET",
                Size = new Size(200, 50),
                Font = new Font(font, 20),
                Location = new Point((settingsPanel.Width - 200) / 2, lab9.Location.Y + lab9.Height + 50)
            };
            setButton.Click += (o, e) =>
            {
                if (trainingFunctionCB.Text == "")
                {
                    lab10.ForeColor = Color.Red;
                    return;
                }

                if (setButton.Text == "SET")
                {
                    lab10.ForeColor = Color.Black;
                    RenameSetButton();
                    AddToDataTextbox($"\r\nWeb createtd!");
                    ActivateBottomControls();
                    UpdateTrainingDataTable();
                    CreateNewNeuralNetwork();
                    PrintWeights();
                    PrintActivations();
                    PrintError();
                }
                else if (setButton.Text == "RESET")
                {
                    RenameSetButton();
                    AddToDataTextbox($"\r\n\r\nWeb deleted!\r\n{Separator(50)}\r\n\r\n");
                    ActivateTopControls();
                    CleanTableLabels();
                }
                
            };
            settingsPanel.Controls.Add(setButton);


            //tablePanel
            Panel tablePanel = new Panel
            {
                Location = new Point(settingsPanel.Width / 4, setButton.Location.Y + setButton.Height + 40),
                Size = new Size(settingsPanel.Width / 2, settingsPanel.Width / 6),
                BorderStyle = BorderStyle.FixedSingle
            };
            for (int i = 0; i < 2; i++)
            {
                int sizeX = tablePanel.Width / 4;
                int sizeY = tablePanel.Height / 2;
                for (int j = 0; j < 4; j++)
                {
                    Panel panel = new Panel
                    {
                        Size = new Size(sizeX, sizeY),
                        Location = new Point(sizeX * j, sizeY * i),
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    Label label = new Label
                    {
                        Size = new Size(sizeX, sizeY),
                        Location = new Point(0, -5),
                        Font = new Font(font, 15),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Text = "-"
                    };
                    tableLabels.Add(label);
                    panel.Controls.Add(label);
                    tablePanel.Controls.Add(panel);
                }
            }
            settingsPanel.Controls.Add(tablePanel);

            //lab11 "Input:"
            Label lab11 = new Label
            {
                Text = "Input:",
                Font = new Font(font, 15),
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(100, tablePanel.Height / 2),
                Location = new Point(tablePanel.Location.X - 100, tablePanel.Location.Y)
            };
            settingsPanel.Controls.Add(lab11);

            //lab12 "Output:"
            Label lab12 = new Label
            {
                Text = "Output:",
                Font = new Font(font, 15),
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(100, tablePanel.Height / 2),
                Location = new Point(tablePanel.Location.X - 100, tablePanel.Location.Y + tablePanel.Height / 2)
            };
            settingsPanel.Controls.Add(lab12);


            //tabel selecting buttons
            for (int i = 0; i < 4; i++)
            {
                Button button = new Button
                {
                    Text = "ON",
                    BackColor = Color.LightGreen,
                    Size = new Size(tablePanel.Width / 4, tablePanel.Width / 6),
                    Location = new Point(tablePanel.Location.X + tablePanel.Width / 4 * i, tablePanel.Location.Y + tablePanel.Height),
                    Tag = i
                };
                button.Click += (o, e) =>
                {
                    if (button.Text == "ON")
                    {
                        button.Text = "OFF";
                        button.BackColor = Color.IndianRed;
                        selectedSetsList[(int)button.Tag] = false;
                    }
                    else if (button.Text == "OFF")
                    {
                        button.Text = "ON";
                        button.BackColor = Color.LightGreen;
                        selectedSetsList[(int)button.Tag] = true;
                    }
                };
                settingsPanel.Controls.Add(button);
            }


            // itrationsLab "Iterations:"
            Label itrationsLab = new Label
            {
                Text = "Iterations: 0",
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(250, 25),
                Font = new Font(font, 17),
                Location = new Point(30, tablePanel.Location.Y + tablePanel.Height + 60)
            };
            settingsPanel.Controls.Add(itrationsLab);

            // epochsLab "Complete epochs:"
            Label epochsLab = new Label
            {
                Text = "Complete epochs: 0", //[эпи/ыкс]
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(250, 30),
                Font = new Font(font, 17),
                Location = new Point(30, itrationsLab.Location.Y + itrationsLab.Height + 10)
            };
            settingsPanel.Controls.Add(epochsLab);

            // errorLabel "Error:"
            Label errorLab = new Label
            {
                Text = "Error: -",
                Font = new Font(font, 20),
                Size = new Size(200, 50),
                Location = new Point(30 + epochsLab.Location.X + epochsLab.Width + 10, itrationsLab.Location.Y + 15)
            };
            settingsPanel.Controls.Add(errorLab);



            // lab13 "Auto:"
            Label lab13 = new Label
            {
                Text = "↓ Auto ↓",
                Size = new Size(100, 30),
                Font = new Font(font, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(settingsPanel.Width / 4 - 50, epochsLab.Location.Y + epochsLab.Height + 20)
            };
            settingsPanel.Controls.Add(lab13);

            // lab14 "Manual:"
            Label lab14 = new Label
            {
                Text = "↓ Manual ↓",
                Size = new Size(120, 30),
                Font = new Font(font, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(settingsPanel.Width * 3 / 4 - 60, epochsLab.Location.Y + epochsLab.Height + 20)
            };
            settingsPanel.Controls.Add(lab14);

            // epochsNUD
            NumericUpDown iterationsNUD = new NumericUpDown
            {
                Value = 0,
                Increment = 1,
                Minimum = 0,
                Maximum = 50,
                Width = 60,
                TextAlign = HorizontalAlignment.Center,
                Font = new Font(font, 20),
                Location = new Point(55, lab13.Location.Y + lab13.Height + 20)
            };
            settingsPanel.Controls.Add(iterationsNUD);

            // runButton
            Button runButton = new Button
            {
                Text = "RUN",
                Font = new Font(font, 17),
                Size = new Size(80, iterationsNUD.Height),
                Location = new Point(iterationsNUD.Location.X + iterationsNUD.Width, iterationsNUD.Location.Y)
            };
            runButton.Click += (s, e) =>
            {
                for (int i = 0; i < iterationsNUD.Value; i++)
                {
                    net.PlusIteration(selectedSetsList);
                    PrintWeightsGradient();
                    PrintWeightsDelta();
                    PrintNeurosDelta();
                    PrintWeights();
                    PrintActivations();
                    PrintError();
                }
                UpdateErrorLabel();
                UpdateIterationsLabel();
            };
            settingsPanel.Controls.Add(runButton);

            // endButton
            Button endButton = new Button
            {
                Text = "END",
                Font = new Font(font, 17),
                Size = new Size(iterationsNUD.Width + runButton.Width, iterationsNUD.Height),
                Location = new Point(55, iterationsNUD.Location.Y + iterationsNUD.Height + 10)
            };
            settingsPanel.Controls.Add(endButton);

            //plusIterationButton
            Button plusIterationButton = new Button
            {
                Text = "+ Iteration",
                Font = new Font(font, 18),
                Size = new Size(150, runButton.Height),
                Location = new Point(settingsPanel.Width * 3 / 4 - 75, runButton.Location.Y)
            };
            plusIterationButton.Click += (o, e) =>
            {
                net.PlusIteration(selectedSetsList);
                PrintWeightsGradient();
                PrintWeightsDelta();
                PrintNeurosDelta();
                PrintWeights();
                PrintActivations();
                PrintError();
                UpdateErrorLabel();
                UpdateIterationsLabel();
            };
            settingsPanel.Controls.Add(plusIterationButton);

            //plusEpochButton
            Button plusEpochButton = new Button
            {
                Text = "+ Epoch",
                Font = new Font(font, 18),
                Size = new Size(150, endButton.Height),
                Location = new Point(settingsPanel.Width * 3 / 4 - 75, endButton.Location.Y)
            };
            plusEpochButton.Click += (s, e) =>
            {
                net.PlusIteration(selectedSetsList);
                PrintWeightsGradient();
                PrintWeightsDelta();
                PrintNeurosDelta();
                PrintWeights();
                PrintActivations();
                PrintError();
                UpdateErrorLabel();
                UpdateIterationsLabel();
            };
            settingsPanel.Controls.Add(plusEpochButton);















            // ___Panel1
            Panel _panel1 = new Panel
            {
                Size = new Size(settingsPanel.Width + 20, 2),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(-10, lab13.Location.Y - 5)
            };
            settingsPanel.Controls.Add(_panel1);

            // ___Panel2
            Panel _panel2 = new Panel
            {
                Size = new Size(2, 400),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(settingsPanel.Width / 2, lab13.Location.Y - 5)
            };
            settingsPanel.Controls.Add(_panel2);

            // ______1
            Label _lab1 = new Label
            {
                Text = "_________________________________________________",
                Width = settingsPanel.Width + 20,
                Font = new Font(font, 15),
                Location = new Point(-10, lab9.Location.Y + lab9.Height),
                Visible = false
            };
            settingsPanel.Controls.Add(_lab1); // костыль, но работает
            // ______2
            Label _lab2 = new Label
            {
                Text = "_________________________________________________",
                Width = settingsPanel.Width + 20,
                Font = new Font(font, 15),
                Location = new Point(-10, lab9.Location.Y + lab9.Height + 105)
            };
            settingsPanel.Controls.Add(_lab2);
            setButton.Click += (o, e) =>
            {
                if (trainingFunctionCB.Text != "")
                    if (setButton.Text == "SET")
                    {
                        _lab1.Visible = false;
                        _lab2.Visible = true;
                    }
                    else if (setButton.Text == "RESET")
                    {
                        _lab1.Visible = true;
                        _lab2.Visible = false;
                    }
            };


            for (int i = 0; i < settingsPanel.Controls.Count - 1; i++)
                settingsPanel.Controls[i].Enabled = (i < 21) ? true : false;
            _lab2.Enabled = true;

            void UpdateIterationsLabel()
            {
                int iterations = net.IterationsQuantity;
                string text = $"Iterations: {iterations}";
                itrationsLab.Text = text;
            }
            void UpdateErrorLabel()
            {
                double errorValue = Math.Round(net.ErrorList[net.ErrorList.Count - 1], 3);
                string text = $"Error: {errorValue}";
                errorLab.Text = text;

                if(errorValue <= 0.01)
                    errorLab.ForeColor = Color.Red;
                else
                    errorLab.ForeColor = Color.Black;
            }
            void ActivateTopControls()
            {
                for (int i = 0; i < settingsPanel.Controls.Count - 2; i++)
                    settingsPanel.Controls[i].Enabled = (i < 21) ? true : false;
            }
            void ActivateBottomControls()
            {
                for (int i = 0; i < settingsPanel.Controls.Count; i++)
                    settingsPanel.Controls[i].Enabled = (i < 20) ? false : true;
            }
            void CleanTableLabels()
            {
                for (int i = 0; i < 8; i++)
                    tableLabels[i].Text = "-";
            }
            void RenameSetButton()
            {
                if (setButton.Text == "SET")
                    setButton.Text = "RESET";
                else if (setButton.Text == "RESET")
                    setButton.Text = "SET";
            }
        }

        void CreateNewNeuralNetwork()
        {
            net = new Net(
                neuronsPerLayer, 
                learningRate, 
                moment, 
                maxWeightsRandomize, 
                minWeightsRandomize, 
                TrainingSet.GetTrainingSet(trainingFunction) );
        }
        void UpdateTrainingDataTable()
        {
            for (int i = 0; i < 8; i++)
            {
                tableLabels[i].Text = i < 4 ?
                $"({TrainingSet.GetTrainingSet(trainingFunction)[i][0]};{TrainingSet.GetTrainingSet(trainingFunction)[i][1]})" :
                $"{TrainingSet.GetTrainingSet(trainingFunction)[i - 4][2]}";
            }
        }
        void AddToDataTextbox(string str)
        {
            dataTextBox.Text += str;
        }
        string Separator(int quantity)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < quantity; i++)
                sb.Append("-");
            string result = sb.ToString();
            return result;
        }
    }
}
