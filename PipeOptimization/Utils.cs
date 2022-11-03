using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeOptimization
{
    public static class Utils
    {

        public static double[] ScalarVectorMultiplication(double scalar, double[] vector) {

            var length = vector.Length;
            var result = new double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = vector[i] * scalar;
            }
            return result;
        }

        public static double[] ScalarVectorSum(double[] vector1, double[] vector2)
        {
            var length = vector1.Length;
            var result = new double[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = vector1[i] + vector2[i];
            }
            return result;
        }

        public static double[,] ListOfArraysToMatrix(List<double[]> list)
        {
            var matrix = new double[list.Count, list[0].Count()];

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[j].Count(); j++)
                {
                    matrix[i, j] = list[i][j];
                }
            }

            return matrix;
        }
    }
}
