using ArduinoDOJO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoDOJO.Controller
{
    public class AIController
    {
        public AIController() { }
        public static IAModel Training(int[,] matrixX, double[,] matrixY, int INPUT_SIZE, int HIDDEN_SIZE, int EPOCH, int OUTPUT_SIZE, double LEARNING_RATE)
        {
            double[,] weights_ih = MatrixController.InitializeMatrix(HIDDEN_SIZE, INPUT_SIZE);
            double[,] weights_ho = MatrixController.InitializeMatrix(OUTPUT_SIZE, HIDDEN_SIZE);

            IAModel iAModel = new IAModel();
            List<DataModel> dataList = new List<DataModel>();

            for (int epoch = 0; epoch < EPOCH; epoch++)
            {
                for (int i = 0; i < matrixX.GetLength(0); i++)
                {
                    // Propagation (Input -> Hidden)
                    double[] hidden = new double[HIDDEN_SIZE];
                    for (int j = 0; j < HIDDEN_SIZE; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < INPUT_SIZE; k++)
                        {
                            sum += matrixX[i, k] * weights_ih[j, k];
                        }
                        hidden[j] = MatrixController.Sigmoid(sum);
                    }

                    // Propagation (Hidden -> Output)
                    double output = 0;
                    for (int j = 0; j < HIDDEN_SIZE; j++)
                    {
                        output += hidden[j] * weights_ho[0, j];
                    }
                    output = MatrixController.Sigmoid(output);

                    // Calculate Error
                    double error = matrixY[i, 0] - output;

                    // RetroPropagation (Output -> Hidden): Ajustement des poids des couches output
                    // Mise à jour des poids  dans weights_ho en fonction de l'erreur
                    double d_output = error * MatrixController.SigmoidDerivative(output);
                    for (int j = 0; j < HIDDEN_SIZE; j++)
                    {
                        weights_ho[0, j] += LEARNING_RATE * d_output * hidden[j];
                    }

                    // RetroPropagation (Hidden -> Input): Ajustement des poids des couches hidden
                    for (int j = 0; j < HIDDEN_SIZE; j++)
                    {
                        double d_hidden = d_output * weights_ho[0, j] * MatrixController.SigmoidDerivative(hidden[j]);
                        for (int k = 0; k < INPUT_SIZE; k++)
                        {
                            weights_ih[j, k] += LEARNING_RATE * d_hidden * matrixX[i, k];
                        }
                    }
                }

                for (int i = 0; i < matrixX.GetLength(0); i++)
                {
                    dataList.Add(new DataModel
                    {
                        Id = i + 1,
                        X = matrixX[i, 0],
                        Y = matrixX[i, 1],
                        Esc = matrixX[i, 2],
                        Up = matrixX[i, 3],
                        Down = matrixX[i, 4],
                        Right = matrixX[i, 5],
                        Left = matrixX[i, 6],
                        Tag = matrixY[i, 0]
                    });
                }
            }

            iAModel.dataModel = dataList;
            iAModel.weights_ih = weights_ih;
            iAModel.weights_ho = weights_ho;

            return iAModel;
        }
        public static List<DataModel> Predict(int[,] matrixX, int INPUT_SIZE, int HIDDEN_SIZE, double[,] GLOBALweight_ih, double[,] GLOBALweight_ho)
        {
            List<DataModel> predictDataList = new List<DataModel>();

            for (int row = 0; row < matrixX.GetLength(0); row++)
            {
                // Calculate hidden layer outputs
                double[] PredictHidden = new double[HIDDEN_SIZE];
                for (int j = 0; j < HIDDEN_SIZE; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < INPUT_SIZE; k++)
                    {
                        sum += matrixX[row, k] * GLOBALweight_ih[j, k]; // Use 'row' instead of 'k'
                    }
                    PredictHidden[j] = MatrixController.Sigmoid(sum);
                }

                // Calculate final output
                double PredictOutput = 0;
                for (int j = 0; j < HIDDEN_SIZE; j++)
                {
                    PredictOutput += PredictHidden[j] * GLOBALweight_ho[0, j];
                }
                PredictOutput = MatrixController.Sigmoid(PredictOutput);
                PredictOutput = Math.Round(PredictOutput, 2);

                Debug.WriteLine($"Prediction for Row {row + 1}: {PredictOutput}");

                predictDataList.Add(new DataModel
                {
                    Id = row + 1,
                    X = matrixX[row, 0],
                    Y = matrixX[row, 1],
                    Esc = matrixX[row, 2],
                    Up = matrixX[row, 3],
                    Down = matrixX[row, 4],
                    Right = matrixX[row, 5],
                    Left = matrixX[row, 6],
                    Tag = PredictOutput
                });
            }


            return predictDataList;
        }
    }
}
