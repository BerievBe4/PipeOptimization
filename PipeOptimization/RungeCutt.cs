using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeOptimization
{
    public static class RungeCutt
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

        public static (double[],double[,]) RungeKutt(double x0, double[] y0, double x1, int n) {
            var h = (x1 - x0) / n;
            var ycount = y0.Length;

            var x = new double[n + 1];
            x[0] = x0;

            //var y = [[Decimal(0)] * ycount for i in range(n + 1)];
            var y = new List<double[]>();

            for (int i = 0; i < ycount; i++)
            {
                for (int j = 0; j < ycount; j++)
                {
                    //y[0] = y0;
                }
            }

            var k1 = new double[ycount];
            var k2 = new double[ycount];
            var k3 = new double[ycount];
            var k4 = new double[ycount];

            for (int i = 0; i < n; i++)
            {

                k1 = ScalarVectorMultiplication(h, TaskDefinition.Task(x[i], y[i]));

                k2 = ScalarVectorMultiplication(h, TaskDefinition.Task(x[i] + 0.5d * h, ScalarVectorSum(y[i], ScalarVectorMultiplication(0.5d, k1))));

                k3 = ScalarVectorMultiplication(h, TaskDefinition.Task(x[i] + 0.5d * h, ScalarVectorSum(y[i], ScalarVectorMultiplication(0.5d, k2))));

                k4 = ScalarVectorMultiplication(h, TaskDefinition.Task(x[i] + h, ScalarVectorSum(y[i], k3)));

                y[i + 1] = ScalarVectorSum(
                    y[i],
                    ScalarVectorMultiplication(
                        1.0d / 6.0d,
                        ScalarVectorSum(
                            k1,
                            ScalarVectorSum(
                                ScalarVectorMultiplication(2d, k2),
                                ScalarVectorSum(
                                    ScalarVectorMultiplication(2d, k3),
                                    k4)
                                )
                            )
                        )
                    );

                x[i + 1] = x[i] + h;
            }
            return (x, y);
        }
    }
}
