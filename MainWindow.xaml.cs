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

namespace ArduinoDOJO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MatrixController matrixController;
        RobotController robotController;
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

        private void BTN_Train_Click(object sender, RoutedEventArgs e)
        {
            BTN_Predict.IsEnabled = true;
            BTN_Save.IsEnabled = true;

            int INPUT_SIZE = (int)InputSlider.Value;
            int HIDDEN_SIZE = (int)HiddenSlider.Value;
            int OUTPUT_SIZE = 1; // on ne veut que "left right down up"

            int EPOCH = 2000;
            double LEARNING_RATE = (double)LearningRateSlider.Value;

            var weights_ih = MatrixController.InitializeMatrix(HIDDEN_SIZE, INPUT_SIZE);

            var weights_ho = MatrixController.InitializeMatrix(OUTPUT_SIZE, HIDDEN_SIZE);
            
            MatrixController.PrintMatrix(weights_ih);
            MatrixController.PrintMatrix(weights_ho);

            for (int epoch = 0; epoch < EPOCH; epoch++)
            {
                for (int )
            }
        }

        private void BTN_Predict_Click(object sender, RoutedEventArgs e)
        {

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