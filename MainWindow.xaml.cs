using ArduinoDOJO.Controller;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using ArduinoDOJO.Model;
using System.Data;

namespace ArduinoDOJO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MatrixController matrixController;
        RobotController robotController;

        double[,] GLOBALweight_ih;
        double[,] GLOBALweight_ho;
        int INPUT_SIZE, HIDDEN_SIZE, OUTPUT_SIZE, EPOCH;

        int[,] matrixX = new int[,]
            {
                {  1,  0,  0, 1, 1, 0, 0 },
                { -1,  0,  0, 1, 1, 0, 0 },
                {  0,  1,  0, 0, 0, 1, 1 },
                {  0, -1,  0, 0, 0, 1, 1 },
                {  1,  0,  0, 0, 1, 1, 0 },
                {  0, -1,  0, 0, 1, 1, 0 },
                {  0, -1,  0, 0, 1, 0, 1 },
                { -1,  0,  0, 0, 1, 0, 1 },
                {  1,  0,  0, 1, 0, 1, 0 },
                {  0,  1,  0, 1, 0, 1, 0 },
                {  0,  1,  0, 1, 0, 1, 1 },
                {  0,  0,  0, 1, 1, 1, 1 },
                {  0, -1,  0, 0, 1, 1, 1 },
                { -1,  0,  0, 1, 0, 0, 1 },
                {  0,  1,  0, 1, 0, 0, 1 },
                {  1,  0,  0, 1, 0, 0, 1 }
            };

        double[,] matrixY = new double[,]
            {
                { 0.25 },
                { 0.75 },
                { 1.0  },
                { 0.5  },
                { 1.0  },
                { 0.75 },
                { 0.25 },
                { 1.0  },
                { 0.5  },
                { 0.75 },
                { 0.5  },
                { 0.0  },
                { 1.0  },
                { 0.5  },
                { 0.25 },
                { 0.25 }
            };


        public MainWindow()
        {
            InitializeComponent();
            matrixController = new MatrixController();
            robotController = new RobotController();
        }
        private void InputSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TB_InputCurrent != null)
            {
                TB_InputCurrent.Text = InputSlider.Value.ToString();
            }
        }
        private void HiddenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TB_HiddenCurrent != null)
            {
                TB_HiddenCurrent.Text = HiddenSlider.Value.ToString();
            }
        }
        
        private void LearningRateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TB_LearningRateCurrent != null)
            {
                TB_LearningRateCurrent.Text = LearningRateSlider.Value.ToString();
            }
        }
        private void EpochSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TB_EpochCurrent != null)
            {
                TB_EpochCurrent.Text = EpochSlider.Value.ToString();
            }
        }
        private void BTN_Train_Click(object sender, RoutedEventArgs e)
        {
            BTN_Predict.IsEnabled = true;
            BTN_Save.IsEnabled = true;

            INPUT_SIZE = (int)InputSlider.Value;
            HIDDEN_SIZE = (int)HiddenSlider.Value;

            EPOCH = (int)EpochSlider.Value;
            OUTPUT_SIZE = 1; // on ne veut que "left right down up"

            
            double LEARNING_RATE = (double)LearningRateSlider.Value;

            double[,] weights_ih = MatrixController.InitializeMatrix(HIDDEN_SIZE, INPUT_SIZE);

            double[,] weights_ho = MatrixController.InitializeMatrix(OUTPUT_SIZE, HIDDEN_SIZE);

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

            }

            List<dataModel> dataList = new List<dataModel>();
            for (int i = 0; i < matrixX.GetLength(0); i++)
            {
                dataList.Add(new dataModel
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

            TrainingGrid.ItemsSource = dataList;

            GLOBALweight_ho = weights_ho;
            GLOBALweight_ih = weights_ih;
        }

        private void BTN_Predict_Click(object sender, RoutedEventArgs e)
        {
            List<dataModel> predictDataList = new List<dataModel>();

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

                predictDataList.Add(new dataModel
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
            PredictGrid.ItemsSource = predictDataList;

        }
        private void BTN_Load_Click(object sender, RoutedEventArgs e)
        {
            BTN_Load.IsEnabled = true;

        }

        private void BTN_Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}