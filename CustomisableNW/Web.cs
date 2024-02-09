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
    * 10)
    */


    class Net
    {
        private List<int> neuronsPerLayer;
        private int layersQuantity;
        private float learningRate,
                      moment;
        private float maxWeightsRandomization,
                      minWeightsRandomization;

        private List<int[]> trainingSet;

        public List<float> ErrorList { get { return errorList; } }
        private List<float> errorList = new List<float>();

        public int TrainingSetNumber { get { return trainingSetNumber; } }
        private int trainingSetNumber = 0;

        public List<List<Neuron>> Neurons { get { return neurons; } } 
        private List<List<Neuron>> neurons = new List<List<Neuron>>();

        public Net(List<int> neuronsPerLayer, float learningRate, float moment, float maxWeightsRandomization, float minWeightsRandomization, List<int[]> trainingSet)
        {
            this.neuronsPerLayer = neuronsPerLayer;
            layersQuantity = neuronsPerLayer.Count;
            this.learningRate = learningRate;
            this.moment = moment;
            this.maxWeightsRandomization = maxWeightsRandomization;
            this.minWeightsRandomization = minWeightsRandomization;
            this.trainingSet = trainingSet;

            InitializeNeurons();
            SetInitialNeuronWeights();
            ComputeActivations();
            ComputeError();
        }


        private void InitializeNeurons()
        {
            for(int i = 0; i < layersQuantity; i++)
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
        private void SetInitialNeuronWeights()
        {
            Random random = new Random();

            for (int i = 1; i < layersQuantity; i++)
                for (int j = 0; j < neuronsPerLayer[i]; j++)
                    for (int k = 0; k < neurons[i][j].Weights.Count; k++)
                    {
                        int temp = random.Next((int)(minWeightsRandomization * 100), (int)(maxWeightsRandomization * 100));
                        float value = (float)temp / 100;
                        neurons[i][j].Weights[k].Value = value;
                    }
        }
        private void ComputeActivations()
        {
            for(int j = 0; j < 2; j++)
                neurons[0][j].Activation = trainingSet[trainingSetNumber][j];  // input neurons

            for (int i = 1; i < layersQuantity; i++) // i-th neural layer
                for(int j = 0; j < neuronsPerLayer[i]; j++) // j-th neuron (in i-th hidden neural layer)
                {
                    float inputValue = 0;
                    for (int k = 0; k < neuronsPerLayer[i-1]; k++)
                        inputValue += neurons[i - 1][k].Activation * neurons[i][j].Weights[k].Value;
                    neurons[i][j].Activation = ActivationFunction.Sigmoid(inputValue);
                }
        }

        private void BackPropagationMethod()
        {
            ComputeOutputNeuronDelta();
            for(int i = layersQuantity-1; i > 0; i--)
            {
                for(int j = 0; j < neuronsPerLayer[i]; j++)
                    for(int k = 0; k < neuronsPerLayer[i-1]; k++)
                    {
                        ComputeWeightGradient(i, j, k);
                        ComputeWeightDelta(i, j, k);
                        RecordNewWeightValue(i, j, k);
                    }

                for (int j = 0; j < neuronsPerLayer[i - 1]; j++)
                    ComputeNeuronDelta(i-1,j);
            }

            void ComputeOutputNeuronDelta()
            {
                float neuronDelta;
                float idealOutput = trainingSet[trainingSetNumber][2];
                float realOutput = neurons[layersQuantity - 1][0].Activation;

                neuronDelta = (idealOutput - realOutput) * ActivationFunction.SigmoidDerivative(realOutput);

                neurons[layersQuantity - 1][0].Delta = neuronDelta;
            }
            void ComputeWeightGradient(int i, int j, int k)
            {
                float gradient;
                float neuronDelta = neurons[i][j].Delta;
                float previosNeuronActivation = neurons[i - 1][k].Activation;

                gradient = neuronDelta * previosNeuronActivation;

                neurons[i][j].Weights[k].Gradient = gradient;
            }
            void ComputeWeightDelta(int i, int j, int k)
            {
                Weight weight = neurons[i][j].Weights[k];
                float newWeightDelta;
                float weightGradient = weight.Gradient;
                float previousWeightDelta = weight.Delta;

                newWeightDelta = learningRate * weightGradient + moment * previousWeightDelta;

                weight.Delta = newWeightDelta;
            }
            void RecordNewWeightValue(int i, int j, int k)
            {
                Weight weight = neurons[i][j].Weights[k];
                weight.Value += weight.Delta;
            }
            void ComputeNeuronDelta(int i, int j)
            {
                float temp = 0;

                for(int x = 0; x < neuronsPerLayer[i+1]; x++)
                {
                    float outgoingWeitghValue = neurons[i + 1][x].Weights[j].Value;
                    float nextNeuronDelta = neurons[i + 1][x].Delta;

                    temp += outgoingWeitghValue * nextNeuronDelta;
                }

                float neuronDelta;
                float neuronActivation = neurons[i][j].Activation;

                neuronDelta = temp * ActivationFunction.SigmoidDerivative(neuronActivation);

                neurons[i][j].Delta = neuronDelta;
            }
        }

        private void ComputeError()
        {
            float idealOutput = trainingSet[trainingSetNumber][2];
            float realOutput = neurons[layersQuantity - 1][0].Activation;

            float error = (float)Math.Pow((idealOutput - realOutput), 2);

            errorList.Add(error);
        }

        public void PlusIteration(bool[] selectedTrainingsets)
        {
            BackPropagationMethod();
            IncrementTrainSetNumber(selectedTrainingsets);
            ComputeActivations();
            ComputeError();
         
        }
        public void PlusEpoch(bool[] selectedTrainingsets)
        {
            bool isThereAValidTrainningSet = !selectedTrainingsets.All(x => x == false);

            if(isThereAValidTrainningSet)
            while(trainingSetNumber != 0)
            {
                PlusIteration(selectedTrainingsets);
            }
        } 



        private void IncrementTrainSetNumber(bool[] selectedTrainingsets)
        {
            if (trainingSetNumber == 3)
                trainingSetNumber = 0;
            else
                trainingSetNumber++;

            bool isItValidTrainingSet = !selectedTrainingsets[trainingSetNumber];
            bool isThereAValidTrainningSet = !selectedTrainingsets.All(x => x == false);

            if (isItValidTrainingSet && isThereAValidTrainningSet)
                IncrementTrainSetNumber(selectedTrainingsets);
        }
    }


    
    
     
    public class Neuron
    {
        public float Activation
        {
            get { return activationsList[activationsList.Count - 1]; }
            set { activationsList.Add(value); }
        }
        public float Delta
        {
            get { return deltaList[deltaList.Count - 1]; }
            set { deltaList.Add(value); }
        }
        public List<Weight> Weights
        {
            get { return weightsList; }
            set { weightsList = value; }
        }

        List<float> activationsList = new List<float>();
        List<float> deltaList = new List<float>();
        List<Weight> weightsList = new List<Weight>();
    }
    public class Weight
    {
        public float Value
        {
            get { return values[values.Count-1]; }
            set { values.Add(value); }
        }
        public float Delta
        {
            get { return deltaList[deltaList.Count - 1]; }
            set { deltaList.Add(value); }
        }
        public float Gradient
        {
            get { return gradientList[gradientList.Count - 1]; }
            set { gradientList.Add(value); }
        }

        public List<float> ValuesList // исправить
        {
            get { return values; }
        }

        List<float> values = new List<float>();
        List<float> deltaList = new List<float> { 0 };
        List<float> gradientList = new List<float>();
    }

    

    static class ActivationFunction
    {
        public static float Sigmoid(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }

        public static float SigmoidDerivative(float x)
        {
            return (1 - x) * x;
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
