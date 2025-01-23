using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoDOJO.Controller
{
    public class MatrixController
    {
        public MatrixController() { }
        public static double[,] InitializeMatrix(int rows, int cols)
        {
            double[,] matrix = new double[rows, cols];



            return matrix;

        }

        public static void PrintMatrix(List<List<double>> matrix)
        {
            Debug.WriteLine("[");
            foreach (var row in matrix)
            {
                foreach (var val in row)
                {
                    Debug.WriteLine($"{val:F2}");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("]");
        }
    }
}
