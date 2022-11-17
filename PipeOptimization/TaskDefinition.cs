using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace PipeOptimization
{
    public class TaskDefinition
    {
        private double alpha = 1d;
        private static double[] u = {
            15.19d,         //1
            8.18d,          //2
            13.198d,        //3
            3.543d,         //4
            4723.7d,        //5
            423.7d,         //6
            204.41d,        //7
            0.001466d,      //8
            0.013d,         //9
            0.09d,          //10
            0.000005428d,   //11
            0.024d,         //12
            0.00000592d     //13
        };   

        private static double[] E = {
            25000d,        //1
            25000d,         //2
            25000d,         //3
            25000d,         //4
            40000d,         //5
            40000d,         //6
            40000d,         //7
            20000d,         //8
            20000d,         //9
            20000d,         //10
            20000d,         //11
            20000d,         //12
            20000d          //13
        };

        private static double[] m = {
            18d,            //0
            84d,            //1
            56d,            //2
            42d,            //3
            28d,            //4
            92d,            //5
            16d            //6
        };

        private static double[] q = {
            78d,           //2
            140d,          //3
            140d          //4
        };

        private double l0 = 0d;
        private double l1 = 180d;

        private static double[] x0 = {
            1d,
            0d,
            0d,
            0d,
            0d,
            0d
        };

        private int n = 5000;

        public TaskDefinition(double alpha)
        {
            this.alpha = alpha;
        }

        private double P(double l)
        {
            return 5d - l / 60d;
        }

        //func01 = lambda t, a: t
        //private double Func_0(double a, double t)
        //{
        //    return t;
        //}

        //private double X7(double l)
        //{
        //    return 373d + 1127d * (Func_0(l / 180d, alpha));
        //}

        private double X7(double l)
        {
            if (0 <= l && l <= 120)
                return 373 + ((alpha - 373) * l) / 120;
            else
                return alpha + ((1500 - alpha) * (l - 120)) / 60;
        }

        private double V(double l, double[] x)
        {
            var G = 1750d;
            var Gs = 3500d;
            var sum = 0d;
            for (int i = 0; i <= 6; i++)
            {
                sum += m[i + 1] * x[i];
            }
            return 509.209 * P(l) * (sum / (X7(l) * (G + Gs * sum / m[0])));
        }

        private double R(int i, double x7) {
            var deg = 23d - E[i - 1] / x7;
            return u[i - 1] * Math.Exp(deg);
        }

        public double[] Task(double l, double[] x)
        {
            var x7calc = X7(l);
            var vl = V(l, x);
            var R1 = R(1, x7calc);
            var R2 = R(2, x7calc);
            var R3 = R(3, x7calc);
            var R4 = R(4, x7calc);
            var R5 = R(5, x7calc);
            var R6 = R(6, x7calc);
            var R7 = R(7, x7calc);
            var R8 = R(8, x7calc);
            var R9 = R(9, x7calc);
            var R10 = R(10, x7calc);
            var R11 = R(11, x7calc);
            var R12 = R(12, x7calc);
            var R13 = R(13, x7calc);
            var dx1 = -1d * (R1 + R2 + R3 + R4) * x[0] * vl;
            var dx2 = (R3 * x[0] - (R6 + R7 + R10 + R13) * x[1]) * vl;
            var dx3 = (R2 * x[0] + R6 * x[1] - (R5 + R9 + R12) * x[2]) * vl;
            var dx4 = (R1 * x[0] + R7 * x[1] + R5 * x[2] - (R8 + R11) * x[3]) * vl;
            var dx5 = (R10 * x[1] + R9 * x[2] + R8 * x[3]) * vl;
            var dx6 = (R4 * x[0] + R13 * x[1] + R12 * x[2] + R11 * x[3]) * vl;
            return new double[] { dx1, dx2, dx3, dx4, dx5, dx6 };
        }

        public double[] Quality(double[,] x) {
            var size = x.GetLength(1);
            var result = new double[size];
            for (int i = 0; i < size; i++)
            {
                var uppersum = 0d;
                for (int up = 0; up <= 3; up++)
                    uppersum += q[up] * m[2 + up] * x[i,1 + up];
                var lowersum = 0d;
                for (int down = 0; down <= 6; down++)
                    lowersum += m[1 + down] * x[i,down];
                result[i] = uppersum / lowersum;
            }
            return result;
        }

        public double iteration(double value) {
            this.alpha = value;
            var rungeResult = RungeKutt(l0, x0, l1, n);
            var x_quality = Quality(rungeResult.Item2);
            var result = x_quality.Max();
            return result;
        }

        public double bisection(double left_border, double right_border, double threshold) {
            var a = left_border;
            var b = right_border;
            var mid = (a + b) / 2d;
            var fmid = iteration(mid);
            var delta = Math.Abs(b - a);
            while (delta > threshold) {
                var y = a + delta / 4d;
                var fy = iteration(y);
                var z = b - delta / 4d;
                var fz = iteration(z);
                if (fy > fmid) {
                    b = mid;
                    mid = y;
                    fmid = fy;
                }
                else {
                    if (fz > fmid) {
                        a = mid;
                        mid = z;
                        fmid = fz;
                    }
                    else {
                        a = y;
                        b = z;
                    }
                }
                delta = Math.Abs(b - a);        
            }
            return (a + b) / 2d;
        }

        public void solve_task(int steps, double left_border, double right_border, double threshold, Chart chart ) {
            //global n, func01, alpha
            //n = steps
            //func01 = custom_func01
            var alpha = bisection(left_border, right_border, threshold);
            var rungeResult = RungeKutt(l0, x0, l1, n);
            var x_quality = Quality(rungeResult.Item2);

            var res = 0d;
            var last = rungeResult.Item2.GetLength(1);
            for (int i = 0; i < last; i++)
            {
                res += rungeResult.Item2[last,i];
            }

            
            //for i in range(0, len(rungeResult[1][last])) :
            //    res += rungeResult[1][last][i]

            //fiqure, axis = plt.subplots(2)
            //axis[0].plot(rungeResult[0], rungeResult[1])
            //axis[0].legend(["x1", "x2", "x3", "x4", "x5", "x6"])
            //    axis[0].set_xlim([l0, l1])
            //axis[0].set_title("x(l)")

            //axis[1].plot(rungeResult[0], x_quality)
            //axis[1].set_xlim([l0, l1])
            //axis[1].set_title("quality")

            //plt.show();
        }

        public (double[], double[,]) RungeKutt(double x0, double[] y0, double x1, int n)
        {
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

                k1 = Utils.ScalarVectorMultiplication(h, Task(x[i], y[i]));

                k2 = Utils.ScalarVectorMultiplication(h, Task(x[i] + 0.5d * h, Utils.ScalarVectorSum(y[i], Utils.ScalarVectorMultiplication(0.5d, k1))));

                k3 = Utils.ScalarVectorMultiplication(h, Task(x[i] + 0.5d * h, Utils.ScalarVectorSum(y[i], Utils.ScalarVectorMultiplication(0.5d, k2))));

                k4 = Utils.ScalarVectorMultiplication(h, Task(x[i] + h, Utils.ScalarVectorSum(y[i], k3)));

                y[i + 1] = Utils.ScalarVectorSum(
                    y[i],
                    Utils.ScalarVectorMultiplication(
                        1.0d / 6.0d,
                        Utils.ScalarVectorSum(
                            k1,
                            Utils.ScalarVectorSum(
                                Utils.ScalarVectorMultiplication(2d, k2),
                                Utils.ScalarVectorSum(
                                    Utils.ScalarVectorMultiplication(2d, k3),
                                    k4)
                                )
                            )
                        )
                    );

                x[i + 1] = x[i] + h;
            }
            return (x, Utils.ListOfArraysToMatrix(y));
        }
    }
}
