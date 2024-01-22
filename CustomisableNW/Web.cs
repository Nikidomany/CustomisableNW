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
        private List<int> neuronsPerLayer;
        private float learningRate,
                      moment;
        private float maxWeightsRandomization,
                      minWeightsRandomization;

        private List<int[]> trainingSet;

        public int TrainingSetNumber { get { return trainingSetNumber; } }
        private int trainingSetNumber = 0;

        public List<List<Neuron>> Neurons { get { return neurons; } } 
        private List<List<Neuron>> neurons = new List<List<Neuron>>();
        


        public Net(List<int> neuronsPerLayer, float learningRate, float moment, float maxWeightsRandomization, float minWeightsRandomization, List<int[]> trainingSet)
        {
            this.neuronsPerLayer = neuronsPerLayer;
            this.learningRate = learningRate;
            this.moment = moment;
            this.maxWeightsRandomization = maxWeightsRandomization;
            this.minWeightsRandomization = minWeightsRandomization;
            this.trainingSet = trainingSet;

            NeuronsInitialize();
            InitialNeuronWeightsSetting();
            ActivationsComputing();
        }



        private void NeuronsInitialize()
        {
            for(int i = 0; i < neuronsPerLayer.Count; i++)
            {
                neurons.Add(new List<Neuron>());    // layer adding

                for (int j = 0; j < neuronsPerLayer[i]; j++)
                {
                    neurons[i].Add(new Neuron());   // neuron adding

                    for (int k = 0; k < (i == 0 ? 0 : neuronsPerLayer[i - 1]); k++)
                        neurons[i][j].Weights.Add(new Weight());
                }
            }
            
        }
        private void InitialNeuronWeightsSetting()
        {
            Random random = new Random();

            for(int i = 1; i < neuronsPerLayer.Count; i++)
                for(int j = 0; j < neuronsPerLayer[i]; j++)
                    for(int k = 0; k < neurons[i][j].Weights.Count; k++)
                    {
                        int temp = random.Next((int)(minWeightsRandomization * 100), (int)(maxWeightsRandomization * 100));
                        float value = (float)temp / 100;
                        neurons[i][j].Weights[k].Value = value;
                    }
        }


        private void ActivationsComputing()
        {
            for(int j = 0; j < 2; j++)
                neurons[0][j].Activation = trainingSet[trainingSetNumber][j];  // input neurons

            for (int i = 1; i < neuronsPerLayer.Count; i++) // i-th neural layer
                for(int j = 0; j < neuronsPerLayer[i]; j++) // j-th neuron (in i-th hidden neural layer)
                {
                    float inputValue = 0;
                    for (int k = 0; k < neuronsPerLayer[i-1]; k++)
                        inputValue += neurons[i - 1][k].Activation * neurons[i][j].Weights[k].Value;
                    neurons[i][j].Activation = ActivationFunction.Sigmoid(inputValue);
                }
        }


        public void PlusIteration(bool[] xxxx, int iterationQuantity = 1)
        {

        }
        public void PlusEpoch(bool[] xxxx, int epochsQuantity = 1)
        {

        }


        
        private void IncrementTrainSetNumber()
        {
            if (trainingSetNumber == 4)
                trainingSetNumber = 0;
            else
                trainingSetNumber++;
        }
        
    }


    
    
     
    public class Neuron
    {
        public float Activation
        {
            get { return activations[activations.Count - 1]; }
            set { activations.Add(value); }
        }
        public List<Weight> Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        List<float> activations = new List<float>();
        List<Weight> weights = new List<Weight>();

    }
    public class Weight
    {
        public float Value
        {
            get { return values[values.Count-1]; }
            set { values.Add(value); }
        }
        public List<float> ValuesList // исправить
        {
            get { return values; }
        }

        List<float> values = new List<float>();
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

    enum TrainingFunction
    {
        XOR,
        OR,
        AND
    }

    static class TrainingSet 
    {
        public static List<int[]> GetTrainingSet(TrainingFunction e)
        {
            List<int[]> result = null;
            result = dictionary[e];
            return result;
        }


        static private List<int[]> XOR = new List<int[]>
        {
            new int[] { 0 , 0 , 0 },
            new int[] { 0 , 1 , 1 },
            new int[] { 1 , 0 , 1 },
            new int[] { 1 , 1 , 0 },
        };
        static private List<int[]> OR = new List<int[]>
        {
            new int[] { 0 , 0 , 0 },
            new int[] { 0 , 1 , 1 },
            new int[] { 1 , 0 , 1 },
            new int[] { 1 , 1 , 1 },
        };
        static private List<int[]> AND = new List<int[]>
        {
            new int[] { 0 , 0 , 0 },
            new int[] { 0 , 1 , 0 },
            new int[] { 1 , 0 , 0 },
            new int[] { 1 , 1 , 1 },
        };

        static private Dictionary<TrainingFunction, List<int[]>> dictionary = new Dictionary<TrainingFunction, List<int[]>>
        {
            {TrainingFunction.XOR, XOR},
            {TrainingFunction.OR, OR},
            {TrainingFunction.AND, AND},
        };
    }

}
