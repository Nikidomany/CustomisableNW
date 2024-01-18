using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomisableNW
{
    /* Требования к кастомизируемому перцептрону:
    * 
    * 1) регулируемое количество скрытых слоёв и нейронов +++
    * 2) Выбор функции активации 
    * 3) Выбор тренировочного сета (логической функции)+++
    * 3) 
    * 4) Многопоточность: расчёты и графика отдельны
    * 5) Вкладки: графики, расёты, визуализация сети в разных вкладках +++
    * 6) Загрузка: лягушка + loading
    * 7)
    * 8)
    * 9)
    * 10) Нейрон смещения (bias) ?
    */


    class Net
    {
        // commit adding WHITH saving
        MainForm form;



        private int hiddenLayersNumber;
        private List<int> neuronsPerLayer;
        private float minWeightsValue, maxWeightsValue;

        List<List<List<Weight>>> weights = new List<List<List<Weight>>>();
        List<List<Neuron>> neurons = new List<List<Neuron>>();


        public Net(MainForm form)
        {
            this.form = form;
            minWeightsValue = form.MinWeightsRandomize;
            maxWeightsValue = form.MaxWeightsRandomize;

            form.runButton.Click += (s, e) => {  };


            this.hiddenLayersNumber = form.hiddenLayersNum; // скрытых слоёв
            this.neuronsPerLayer = form.neuronsPerHiddenLayer;       // нейронов в скрытых слоях соответсвенно
            form.TextBox += "Web created!\r\n";

            WeightsInitialize();
            NeuronsInitialize();
            PrintWeights();
            ActivationsComputing();
        } 

        private void WeightsInitialize()
        {
            Random random = new Random();
            for(int i = 0; i < hiddenLayersNumber+1; i++) // создание слоя весов
            {
                weights.Add(new List<List<Weight>>());
                for(int j = 0; j < (i < neuronsPerLayer.Count ? neuronsPerLayer[i] : 1); j++) // создание весов, идущих к одному нейрону
                {
                    weights[i].Add(new List<Weight>());
                    for(int k = 0; k < (i == 0 ? 2 : neuronsPerLayer[i-1]); k++) // создание конкретного веса
                    {
                        int value = random.Next((int)(minWeightsValue * 100), (int)(maxWeightsValue * 100)); // установка рандомного значения в заданных пределах 
                        weights[i][j].Add(new Weight((float)value/100));
                    }
                }
            }
            
        }
        private void NeuronsInitialize()
        {
            for(int i = 0; i < hiddenLayersNumber; i++)
            {
                neurons.Add(new List<Neuron>());    // adding a hidden layer

                for (int j = 0; j < neuronsPerLayer[i]; j++)
                    neurons[i].Add(new Neuron());   // adding a neuron to the hidden layer
            }
            neurons.Add(new List<Neuron> { new Neuron() });     // adding an output neuron
        }


        private void PrintWeights()
        {
            string result = "";

            result += "\r\nWeights randomizing:";
            

            for (int i = 0; i < weights.Count; i++) // веса между слоями
            {
                result += $"\r\n   • {(i == 0 ? "input layer - 1" : (i == weights.Count - 1 ? $"{i} layer - output" : $"{i} layer - {i + 1}"))} layer:";
                result += "\r\n           ";
                for (int x = 0; x < weights[i].Count; x++) // X axis: N10  N11  N12...
                    result += $"  N{i+1}{x}  ";
                for (int j = 0; j < (i == 0 ? 2 : weights[i-1].Count); j++) // веса ведущие к одному нейрону
                {
                    result += $"\r\n     N{i}{j}";
                    for (int k = 0; k < weights[i].Count; k++)
                    {
                        string sign = (weights[i][k][j].Value < 0 ? "" : " ");
                        result += $" {sign}{weights[i][k][j].Value} ";
                    }
                }
            }

            form.TextBox += result;
        }
        private void PrintActivations()
        {
            string result = "\r\n\r\nNeuron activation:";

            int maxNeuronsNumber = 0;
            for(int i = 0; i < neuronsPerLayer.Count; i++) // biggest layer computing
                if(neurons[i].Count > maxNeuronsNumber)
                    maxNeuronsNumber = neuronsPerLayer[i];

            result += "\r\n          ";
            for (int i = 0; i < hiddenLayersNumber + 2; i++)
            {
                string layerName = (i == 0 ? "Inp" : (i == hiddenLayersNumber + 1 ? " Out" : $"  {i}-H "));
                result += $"  {layerName} ";
            }

            for (int i = 0; i < maxNeuronsNumber; i++) // i-th neuron in j-th layer
            {
                result += $"\r\n     N{i} ";
                result += $"   {(i < 2 ? $"{TrainSet.Input(TrainSetNumber())[i]}" : " -")}   "; // input value or "  " (when i>1)
                for (int j = 0; j < neurons.Count; j++) // j-th layer
                {
                    string activation = (i < neurons[j].Count) ? (Math.Round(neurons[j][i].Activation,2).ToString()) : "  -   ";
                    result += $"   {activation}  ";
                }
            }
            form.TextBox += result;
        }


        private void ActivationsComputing()
        {
            for(int i = 0; i < hiddenLayersNumber + 1; i++) // i-th hidden neural layer
                for(int j = 0; j < (i < hiddenLayersNumber ? neuronsPerLayer[i] : 1); j++) // j-th neuron (in i-th hidden neural layer)
                {
                    float inputValue = 0; // the sum of the multiplications of the weights and activations

                    if(i == 0) // for the first hidden neural layer
                    {
                        for(int k = 0; k < 2; k++)
                            inputValue += (TrainSet.Input(TrainSetNumber())[k] * weights[i][j][k].Value);
                    }
                    else if(i > 0 && i < hiddenLayersNumber) // for the second and next hidden neuron layers
                    {
                        for(int k = 0; k < neuronsPerLayer[i-1]; k++)
                            inputValue += neurons[i - 1][k].Activation* weights[i][j][k].Value;
                    }
                    else if(i == hiddenLayersNumber) // for the output neuron layer
                    {
                        for (int k = 0; k < neuronsPerLayer[i - 1]; k++)
                            inputValue += neurons[i - 1][k].Activation * weights[i][j][k].Value;
                    }

                    neurons[i][j].Activation = ActivationFunction.Sigmoid(inputValue); // recording output neuron value
                }
            PrintActivations();
        }


        
        
        private int TrainSetNumber()
        {
            return 1;
        }
        
    }


    class Weight
    {
        public Weight(float value)
        {
            values.Add(value);
        }
        public float Value
        {
            get { return values[values.Count - 1]; }
            set { values.Add(value); }
        }
        List<float> values = new List<float>();
    }
    class Neuron
    {
        public float Activation
        {
            get { return activations[activations.Count - 1]; }
            set { activations.Add(value); }
        }
        List<float> activations = new List<float>();

    }



    static class ActivationFunction
    {
        public static float Sigmoid(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }

        [Obsolete("Функция не подходит!", true)]
        public static float HyperbolicTangent(float x)
        {
            return 0;
        }

    }



    static class TrainSet
    {
        static private List<List<int>> input = new List<List<int>>
        {
            new List<int>{0,0},
            new List<int>{0,1},
            new List<int>{1,0},
            new List<int>{1,1},
        };

        static private Dictionary<string, List<int>> output = new Dictionary<string, List<int>>
        {
            {"XOR", new List<int> {0, 1, 1, 0}},
            {"OR", new List<int> {0, 1, 1, 1}},
            {"AND", new List<int> {1, 0, 0, 1}}
        };

        static public int[] Input(int trainCoupleNumber)
        {
            return new int[] { input[trainCoupleNumber][0], input[trainCoupleNumber][1] };
        }
        static public int Output(string trainFunction, int trainCoupleNumber)
        {
            return output[trainFunction][trainCoupleNumber];
        }
    }
















    //class InputNeuron
    //{
    //    public float Activation // Свойство, хранящее в себе ативацию нейрона и его логи, а так же записывающее новое значение
    //    {
    //        get { return activations[activations.Count - 1]; }
    //        set { activations.Add(value); }
    //    }
    //    List<float> activations = new List<float>();
    //}
    //class HiddenNeuron
    //{
    //    public float Activation
    //    {
    //        get { return activations[activations.Count - 1]; }
    //        set { activations.Add(value); }
    //    }
    //    List<float> activations = new List<float>();
    //}
    //class OutputNeuron
    //{
    //    public float Activation
    //    {
    //        get { return activations[activations.Count - 1]; }
    //        set { activations.Add(value); }
    //    }
    //    List<float> activations = new List<float>();
    //}



}
