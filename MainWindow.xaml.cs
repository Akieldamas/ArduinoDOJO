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

        double[,] GLOBALweight_ih;
        double[,] GLOBALweight_ho;
        int INPUT_SIZE, HIDDEN_SIZE, OUTPUT_SIZE, EPOCH;
        double LEARNING_RATE;

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
            LEARNING_RATE = (double)LearningRateSlider.Value;
            EPOCH = (int)EpochSlider.Value;
            OUTPUT_SIZE = 1; // on ne veut que "left right down up"

            IAModel iaModel = AIController.Training(matrixX, matrixY, INPUT_SIZE, HIDDEN_SIZE, EPOCH, OUTPUT_SIZE, LEARNING_RATE);
            TrainingGrid.ItemsSource = iaModel.dataModel;
            GLOBALweight_ho = iaModel.weights_ho;
            GLOBALweight_ih = iaModel.weights_ih;
        }
        private void BTN_Predict_Click(object sender, RoutedEventArgs e)
        {
            List<DataModel> predictDataList = AIController.Predict(matrixX, INPUT_SIZE, HIDDEN_SIZE, GLOBALweight_ih, GLOBALweight_ho);

            PredictGrid.ItemsSource = predictDataList;
        }
        private void BTN_Load_Click(object sender, RoutedEventArgs e)
        {
            BTN_Load.IsEnabled = true;

        }

        private void BTN_Save_Click(object sender, RoutedEventArgs e)
        {
            // save hidden
            //for (int )
            

        }
    }
}