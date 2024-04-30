using MathNet.Numerics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TorqueSensorHub.Drivers;

namespace TorqueSensorHub
{
    public partial class TorqueReg : Form
    {
        readonly IDevice readDevice;
        readonly IFeedbackDevice feedbackDevice;
        Stopwatch timeKeeper = new Stopwatch();
        List<decimal> torqueArray = new List<decimal>();
        List<decimal> angularArray = new List<decimal>();
        ScrewData currentScrew = null;
        double currentStrength = 0;
        double holeSize = 3.2e-3;
        double alpha = 2 * Math.PI;
        double offset = 2 * Math.PI;
        double mu_thread = 0.17;
        double mu_head = 0.17;
        ConcurrentQueue<Tuple<DataPointCollection, double, double>> pointsToAdd = new ConcurrentQueue<Tuple<DataPointCollection, double, double>>();
        Timer updateCharts = new Timer();
        float calfac = 1f;

        List<Tuple<string, double>> knownMaterials = new List<Tuple<string, double>>
        {
            new Tuple<string, double>("M80", 0.8e6),
            new Tuple<string, double>("M150", 1.6e6),
            new Tuple<string, double>("M330", 4e6),
            new Tuple<string, double>("M450", 10e6),
            new Tuple<string, double>("M600", 17e6),
        };

        class ScrewData
        {
            public string Name { get; set; }
            public double Pitch { get; set; }
            public double Major { get; set; }
            public double Minor { get; set; }
            public double Shank { get; set; } = 4;
            public double Head { get; set; } = 10;
            public double Taper { get; set; } = 60;//degrees
            public double Beta { get; set; }//radians
            public ScrewData(string Name, double Pitch, double Major, double Minor, double Beta)
            {
                this.Name = Name;
                this.Pitch = Pitch;
                this.Major = Major;
                this.Minor = Minor;
                this.Beta = Beta;
            }
        }

        List<ScrewData> screws = new List<ScrewData>
        {
            //Name, Pitch, Major-Dim, Minor-Diam
            //new ScrewData ( "HA 4.5", 1.75e-3, 4.5e-3, 3.0e-3, 0.3316 ),
            new ScrewData ( "HB 6.5", 2.75e-3, 6.5e-3, 3.0e-3, 0.2618 ),
        };

        readonly TimeSpan UpdateInterval = new TimeSpan(0, 0, 0, 0, 500);//100 ms
        TimeSpan NextUpdate = new TimeSpan(0);

        readonly TimeSpan PointInterval = new TimeSpan(0, 0, 0, 0, 1);//10 ms
        TimeSpan NextPoint = new TimeSpan(0);

        public TorqueReg(IDevice readDevice, IFeedbackDevice feedbackDevice)
        {
            InitializeComponent();
            this.readDevice = readDevice;
            this.feedbackDevice = feedbackDevice;
            MaterialListBox.Items.AddRange(knownMaterials.Select(item => item.Item1).ToArray());
            MaterialListBox.SelectedItem = "M330";
            ScrewListBox.Items.AddRange(screws.Select(item => item.Name).ToArray());
            ScrewListBox.SelectedIndex = 0;
            torqueChart.ChartAreas.First().AxisX.Minimum = 0;
            torqueChart.ChartAreas.First().AxisY.Minimum = -0.5;
            torqueChart.ChartAreas.First().AxisY.Interval = 0.5;
            strengthChart.ChartAreas.First().AxisY.Minimum = -5;
            strengthChart.ChartAreas.First().AxisY.Maximum = 25;
            strengthChart.ChartAreas.First().AxisY.Interval = 5;
            updateCharts.Interval = 50;
            updateCharts.Tick += UpdateCharts_Tick;
        }

        private void UpdateCharts_Tick(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                while (pointsToAdd.Count > 0)
                {
                    Tuple<DataPointCollection, double, double> point;
                    pointsToAdd.TryDequeue(out point);//it will be there

                    point.Item1.AddXY(point.Item2, point.Item3);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NextUpdate = new TimeSpan(UpdateInterval.Ticks);
            NextPoint = new TimeSpan(0);
            torqueArray.Clear();
            angularArray.Clear();
            timeKeeper.Reset();
            feedbackDevice.StartInsertion();
            timeKeeper.Start();
            updateCharts.Start();

            foreach (var series in torqueChart.Series)
            {
                series.Points.Clear();
            }

            foreach (var series in strengthChart.Series)
            {
                series.Points.Clear();
            }
            Vec_x = 5e6;
            Mat_P = 5e6*5e6;
            lastIndex = 0;

            readDevice.OnDataPoint += ReadDevice_OnDataPoint;
        }

        private void ReadDevice_OnDataPoint(object sender, List<KeyValuePair<DataPointType, decimal>> e)
        {
            if (e.Count(item => item.Key == DataPointType.Torque) < 1 || (e.Count(item => item.Key == DataPointType.AngularDisplacment) < 1 && e.Count(item => item.Key == DataPointType.LinearDisplacment) < 1))
            {
                return;
            }


            if (timeKeeper.Elapsed > NextPoint)
            {
                NextPoint = timeKeeper.Elapsed + PointInterval;
            }
            else
            {
                return;
            }

            var torque = e.First(item => item.Key == DataPointType.Torque).Value;
            decimal angular = 0;
            if (e.Count(item => item.Key == DataPointType.LinearDisplacment) > 0)
            {
                var linear = e.First(item => item.Key == DataPointType.LinearDisplacment).Value;
                var linear_x0 = (holeSize / 2) / Math.Tan((currentScrew.Taper / 2) / 180 * Math.PI);
                var angular_x0 = linear_x0 / currentScrew.Pitch * 2 * Math.PI;
                angular = (-linear / (decimal)currentScrew.Pitch * 2 * (decimal)Math.PI / 1000) - (decimal)angular_x0;
            }
            else
            {
                angular = e.First(item => item.Key == DataPointType.AngularDisplacment).Value / 180L * (decimal)Math.PI;
            }
            torqueArray.Add(torque);
            angularArray.Add(angular);

            if (timeKeeper.Elapsed > NextUpdate)
            {
                NextUpdate = timeKeeper.Elapsed + UpdateInterval;
            updateEstimateIncremental2();
            }

            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(torqueChart.Series.FindByName("Measured Torque").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round((double)torque, 3)));

        }


        private double Vec_x = 5e6 ;
        private double Mat_P =  5e6*5e6 ;
        int lastIndex = 0;

        private void updateEstimateIncremental()
        {

            //Matrix arithmetic based on https://stackoverflow.com/questions/46836908/double-inversion-c-sharp

            Func<double[,], double[,]> transposeMatrix = matrix =>
            {
                double[,] result = new double[matrix.GetLength(1), matrix.GetLength(0)];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        result[j, i] = matrix[i, j];
                    }
                }
                return result;
            };


            Func<double[,], double, double[,]> scaleMatrix = (matrix, scalar) =>
            {
                double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        result[i, j] = matrix[i, j] * scalar;
                    }
                }
                return result;
            };

            Func<double[,], double[,], double[,]> addMatrix = (matrixA, matrixB) =>
            {
                if (matrixA.GetLength(0) != matrixB.GetLength(0) || matrixA.GetLength(1) != matrixB.GetLength(1))
                {
                    throw new Exception("Adding matricies of different sizes");
                }
                double[,] result = new double[matrixA.GetLength(0), matrixA.GetLength(1)];
                for (int i = 0; i < matrixA.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixA.GetLength(1); j++)
                    {
                        result[i, j] = matrixA[i, j] + matrixB[i, j];
                    }
                }
                return result;
            };

            Func<double[,], double[,], double[,]> subMatrix = (matrixA, matrixB) =>
            {
                if (matrixA.GetLength(0) != matrixB.GetLength(0) || matrixA.GetLength(1) != matrixB.GetLength(1))
                {
                    throw new Exception("Adding matricies of different sizes");
                }
                double[,] result = new double[matrixA.GetLength(0), matrixA.GetLength(1)];
                for (int i = 0; i < matrixA.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixA.GetLength(1); j++)
                    {
                        result[i, j] = matrixA[i, j] - matrixB[i, j];
                    }
                }
                return result;
            };

            Func<int, double[,]> identityMatrix = (size) =>
            {
                double[,] result = new double[size, size];
                for (int i = 0; i < size; i++)
                {
                    result[i, i] = 1;
                }
                return result;
            };

            Func<double[,], double[,], double[,]> multiplyMatrix = (matrixA, matrixB) =>
            {
                int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
                int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
                if (aCols != bRows)
                    throw new Exception("Non-conformable matrices in MatrixProduct");

                double[,] result = new double[aRows, bCols];

                for (int i = 0; i < aRows; ++i) // each row of A
                    for (int j = 0; j < bCols; ++j) // each col of B
                        for (int k = 0; k < aCols; ++k) // could use k less-than bRows
                            result[i, j] += matrixA[i, k] * matrixB[k, j];

                return result;
            };

            Func<double[,], double[], double[]> solveHelper = (luMatrix, b) =>
            {
                // before calling this helper, permute b using the perm array
                // from MatrixDecompose that generated luMatrix
                int n = luMatrix.GetLength(0);
                double[] x = new double[n];
                b.CopyTo(x, 0);

                for (int i = 1; i < n; ++i)
                {
                    double sum = x[i];
                    for (int j = 0; j < i; ++j)
                        sum -= luMatrix[i, j] * x[j];
                    x[i] = sum;
                }

                x[n - 1] /= luMatrix[n - 1, n - 1];
                for (int i = n - 2; i >= 0; --i)
                {
                    double sum = x[i];
                    for (int j = i + 1; j < n; ++j)
                        sum -= luMatrix[i, j] * x[j];
                    x[i] = sum / luMatrix[i, i];
                }

                return x;
            };

            Func<double[,], Tuple<double[,], int[], int>> decomposeMatrix = matrix =>
            {
                int[] perm;
                int toggle;
                // Doolittle LUP decomposition with partial pivoting.
                // rerturns: result is L (with 1s on diagonal) and U;
                // perm holds row permutations; toggle is +1 or -1 (even or odd)
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1); // assume square
                if (rows != cols)
                    throw new Exception("Attempt to decompose a non-square m");

                int n = rows; // convenience

                double[][] result = new double[n][];
                for (int i = 0; i < n; i++)
                {
                    result[i] = new double[n];
                    for (int j = 0; j < n; j++)
                    {
                        result[i][j] = matrix[i, j];
                    }
                }

                perm = new int[n]; // set up row permutation result
                for (int i = 0; i < n; ++i) { perm[i] = i; }

                toggle = 1; // toggle tracks row swaps.
                            // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

                for (int j = 0; j < n - 1; ++j) // each column
                {
                    double colMax = Math.Abs(result[j][j]); // find largest val in col
                    int pRow = j;
                    //for (int i = j + 1; i less-than n; ++i)
                    //{
                    //  if (result[i][j] greater-than colMax)
                    //  {
                    //    colMax = result[i][j];
                    //    pRow = i;
                    //  }
                    //}

                    // reader Matt V needed this:
                    for (int i = j + 1; i < n; ++i)
                    {
                        if (Math.Abs(result[i][j]) > colMax)
                        {
                            colMax = Math.Abs(result[i][j]);
                            pRow = i;
                        }
                    }
                    // Not sure if this approach is needed always, or not.

                    if (pRow != j) // if largest value not on pivot, swap rows
                    {
                        double[] rowPtr = result[pRow];
                        result[pRow] = result[j];
                        result[j] = rowPtr;

                        int tmp = perm[pRow]; // and swap perm info
                        perm[pRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }

                    // --------------------------------------------------
                    // This part added later (not in original)
                    // and replaces the 'return null' below.
                    // if there is a 0 on the diagonal, find a good row
                    // from i = j+1 down that doesn't have
                    // a 0 in column j, and swap that good row with row j
                    // --------------------------------------------------

                    if (result[j][j] == 0.0)
                    {
                        // find a good row to swap
                        int goodRow = -1;
                        for (int row = j + 1; row < n; ++row)
                        {
                            if (result[row][j] != 0.0)
                                goodRow = row;
                        }

                        if (goodRow == -1)
                            throw new Exception("Cannot use Doolittle's method");

                        // swap rows so 0.0 no longer on diagonal
                        double[] rowPtr = result[goodRow];
                        result[goodRow] = result[j];
                        result[j] = rowPtr;

                        int tmp = perm[goodRow]; // and swap perm info
                        perm[goodRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }
                    // --------------------------------------------------
                    // if diagonal after swap is zero . .
                    //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                    //  return null; // consider a throw

                    for (int i = j + 1; i < n; ++i)
                    {
                        result[i][j] /= result[j][j];
                        for (int k = j + 1; k < n; ++k)
                        {
                            result[i][k] -= result[i][j] * result[j][k];
                        }
                    }


                } // main j column loop

                double[,] resultConv = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        resultConv[i, j] = result[i][j];
                    }
                }

                return new Tuple<double[,], int[], int>(resultConv, perm, toggle);
            };

            Func<double[,], double[,]> invertMatrix = matrix =>
            {
                int n = matrix.GetLength(0);
                double[,] result = matrix.Clone() as double[,];

                var tuple = decomposeMatrix(matrix);
                double[,] lum = tuple.Item1;
                int[] perm = tuple.Item2;
                int toggle = tuple.Item3;
                if (lum == null)
                    throw new Exception("Unable to compute inverse");

                double[] b = new double[n];
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        if (i == perm[j])
                            b[j] = 1.0;
                        else
                            b[j] = 0.0;
                    }

                    double[] x = solveHelper(lum, b);

                    for (int j = 0; j < n; ++j)
                        result[j, i] = x[j];
                }
                return result;
            };

            //get relevant data

            int length = torqueArray.Count;
            int newCount = length - lastIndex;
            Console.WriteLine("Newcount: " + newCount);
            //if(length < 50)
            //{
            //    return;
            //}

            var torqueDoubleArray = torqueArray.Skip(lastIndex).Take(newCount).Select(t => (double)t).ToArray();
            var angularDoubleArray = angularArray.Skip(lastIndex).Take(newCount).Select(a => ((double)a)).ToArray();

            lastIndex += newCount;

            //mathematical constants

            var theta = Math.Atan2(currentScrew.Pitch, (Math.PI * currentScrew.Major));
            var rs = (2 * holeSize + currentScrew.Major) / 6;
            var Ac = Math.Tan(currentScrew.Beta) * Math.Pow(currentScrew.Major - holeSize, 2) / 4;
            var G1 = rs * Ac * Math.Cos(theta);

            var rf = (holeSize + currentScrew.Major) / 4;
            var Kf0 =
                0.5
                * (currentScrew.Major - holeSize)
                * Math.Sqrt((1 + Math.Pow(Math.Tan(currentScrew.Beta), 2)) * (Math.Pow((currentScrew.Major + holeSize) / 4, 2) + Math.Pow(currentScrew.Pitch / (2 * Math.PI), 2)));
            var G2 = 2 * rf * Kf0 * Math.Cos(theta);

            //create equation

            double[,] Mat_H = new double[newCount, 1];

            for (int i = 0; i < angularDoubleArray.Length; i++)
            {
                var phi = angularDoubleArray[i] - offset;
                var zeta = ((phi > alpha) ? 1 : 0) * (phi - alpha);
                var psi = phi;

                var Tsz = (1 / alpha) * G1 * (psi - zeta);
                var Tfz = mu_thread * G2 * (phi - alpha / 2);

                Mat_H[i, 0] = Tsz + Tfz;
            }

            //conversions
            double[,] Mat_x = { { Vec_x } };
            double[,] Mat_y = new double[torqueDoubleArray.Length, 1];
            for (int i = 0; i < torqueDoubleArray.Length; i++)
            {
                Mat_y[i, 0] = torqueDoubleArray[i];
            }

            //helper variables
            double[,] Mat_Ht = transposeMatrix(Mat_H);
            double[,] Mat_R = scaleMatrix(identityMatrix(newCount), 0.001);


            //update
            double[,] Mat_K = multiplyMatrix(multiplyMatrix(new[,]{ { Mat_P} }, Mat_Ht), invertMatrix(addMatrix(multiplyMatrix(multiplyMatrix(Mat_H, new[,] { { Mat_P } }), Mat_Ht), Mat_R)));


            Mat_P = (multiplyMatrix(subMatrix(identityMatrix(1), multiplyMatrix(Mat_K, Mat_H)), new[,] { { Mat_P } }))[0,0];
            Mat_x = addMatrix(Mat_x, multiplyMatrix(Mat_K, subMatrix(Mat_y, multiplyMatrix(Mat_H, Mat_x))));

            Vec_x = Mat_x[0, 0];
            //Console.WriteLine($"P={Mat_P[0, 0]}");
            //Console.WriteLine($"x={Mat_x[0, 0]}");

            ////solve pseudoinverse (non-iterative)
            //double[,] Mat_y = new double[torqueDoubleArray.Length, 1];
            //for (int i = 0; i < torqueDoubleArray.Length; i++)
            //{
            //    Mat_y[i, 0] = torqueDoubleArray[i];
            //}

            //var Mat_x = multiplyMatrix(multiplyMatrix(invertMatrix(multiplyMatrix(Mat_Ht, Mat_H)), Mat_Ht), Mat_y);

            var strength = Mat_x[0, 0];




            //calculate torque
            var theta_tau = Math.Atan2(currentScrew.Pitch, Math.PI * currentScrew.Shank) + Math.Atan2(1, -mu_thread);
            var tau_ult = strength;// / Math.Sqrt(3);

            var Ac_fail = Math.PI * currentScrew.Major * angularDoubleArray[angularDoubleArray.Length - 1] / (2 * Math.PI) * currentScrew.Pitch;

            //Console.WriteLine(angularDoubleArray[length - 1]/Math.PI*180);

            var T_fail = tau_ult * Ac_fail * ((1 / 3) * (Math.Pow(currentScrew.Head, 3) - Math.Pow(currentScrew.Shank, 3)) / (Math.Pow(currentScrew.Head, 2) - Math.Pow(currentScrew.Shank, 2))
                * mu_head * Math.Sin(theta_tau) - currentScrew.Major / 2 * Math.Cos(theta_tau)) * calfac;

            var l = (currentScrew.Major - currentScrew.Minor) / 2;
            var gamma = 2 * l / currentScrew.Pitch;
            var k_t = l + (1 + 1.5 * Math.Tanh(0.3 * Math.Log(gamma) + 0.7)) * Math.Sqrt(gamma);

            var T_fail_kz = T_fail / k_t;



            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Estimated Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(strength / 1e6, 3)));
            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Known Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(currentStrength / 1e6, 3)));
            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(torqueChart.Series.FindByName("Estimated Limit").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(Math.Max(T_fail, 0), 3)));

            if (autoStopBox.Checked && torqueDoubleArray[torqueDoubleArray.Length - 1] > T_fail * 0.8 && angularDoubleArray[angularDoubleArray.Length - 1] > Math.PI * 20)
            {
                feedbackDevice.StopInsertion();
            }
        }
        private void updateEstimateIncremental2()
        {
            //get relevant data

            int length = torqueArray.Count;
            int newCount = length - lastIndex;
            Console.WriteLine("Newcount: " + newCount);
            //if(length < 50)
            //{
            //    return;
            //}

            var torqueDoubleArray = torqueArray.Skip(lastIndex).Take(newCount).Select(t => (double)t).ToArray();
            var angularDoubleArray = angularArray.Skip(lastIndex).Take(newCount).Select(a => ((double)a)).ToArray();

            lastIndex += newCount;

            //mathematical constants

            var theta = Math.Atan2(currentScrew.Pitch, (Math.PI * currentScrew.Major));
            var rs = (2 * holeSize + currentScrew.Major) / 6;
            var Ac = Math.Tan(currentScrew.Beta) * Math.Pow(currentScrew.Major - holeSize, 2) / 4;
            var G1 = rs * Ac * Math.Cos(theta);

            var rf = (holeSize + currentScrew.Major) / 4;
            var Kf0 =
                0.5
                * (currentScrew.Major - holeSize)
                * Math.Sqrt((1 + Math.Pow(Math.Tan(currentScrew.Beta), 2)) * (Math.Pow((currentScrew.Major + holeSize) / 4, 2) + Math.Pow(currentScrew.Pitch / (2 * Math.PI), 2)));
            var G2 = 2 * rf * Kf0 * Math.Cos(theta);

            //create equation


            for (int i = 0; i < angularDoubleArray.Length; i++)
            {
                var phi = angularDoubleArray[i] - offset;
                var zeta = ((phi > alpha) ? 1 : 0) * (phi - alpha);
                var psi = phi;

                var Tsz = (1 / alpha) * G1 * (psi - zeta);
                var Tfz = mu_thread * G2 * (phi - alpha / 2);

                var h = Tsz + Tfz;
                var r = 0.001;

                var k = Mat_P * h / (Mat_P * h * h + r);
                Mat_P = (1 - k * h) * Mat_P;
                Vec_x = Vec_x + k*(torqueDoubleArray[i] - h * Vec_x);
            }

            var strength = Vec_x;


            //calculate torque
            var theta_tau = Math.Atan2(currentScrew.Pitch, Math.PI * currentScrew.Shank) + Math.Atan2(1, -mu_thread);
            var tau_ult = strength;// / Math.Sqrt(3);

            var Ac_fail = Math.PI * currentScrew.Major * angularDoubleArray[angularDoubleArray.Length - 1] / (2 * Math.PI) * currentScrew.Pitch;

            //Console.WriteLine(angularDoubleArray[length - 1]/Math.PI*180);

            var T_fail = tau_ult * Ac_fail * ((1 / 3) * (Math.Pow(currentScrew.Head, 3) - Math.Pow(currentScrew.Shank, 3)) / (Math.Pow(currentScrew.Head, 2) - Math.Pow(currentScrew.Shank, 2))
                * mu_head * Math.Sin(theta_tau) - currentScrew.Major / 2 * Math.Cos(theta_tau)) * calfac;

            var l = (currentScrew.Major - currentScrew.Minor) / 2;
            var gamma = 2 * l / currentScrew.Pitch;
            var k_t = l + (1 + 1.5 * Math.Tanh(0.3 * Math.Log(gamma) + 0.7)) * Math.Sqrt(gamma);

            var T_fail_kz = T_fail / k_t;



            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Estimated Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(strength / 1e6, 3)));
            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Known Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(currentStrength / 1e6, 3)));
            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(torqueChart.Series.FindByName("Estimated Limit").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(Math.Max(T_fail, 0), 3)));

            if (autoStopBox.Checked && torqueDoubleArray[torqueDoubleArray.Length - 1] > T_fail * 0.8 && angularDoubleArray[angularDoubleArray.Length - 1] > Math.PI * 20)
            {
                feedbackDevice.StopInsertion();
            }
        }

        private void updateEstimate()
        {
            int length = torqueArray.Count;

            var torqueDoubleArray = torqueArray.Select(t => (double)t).Take(length).ToArray();
            var angularDoubleArray = angularArray.Select(a => ((double)a)).Take(length).ToArray();


            var theta = Math.Atan2(currentScrew.Pitch , (Math.PI * currentScrew.Major));
            var rs = (2 * holeSize + currentScrew.Major) / 6;
            var Ac = Math.Tan(currentScrew.Beta) * Math.Pow(currentScrew.Major - holeSize, 2) / 4;
            var G1 = rs * Ac * Math.Cos(theta);

            var rf = (holeSize + currentScrew.Major) / 4;
            var Kf0 = 
                0.5 
                * (currentScrew.Major - holeSize) 
                * Math.Sqrt((1 + Math.Pow(Math.Tan(currentScrew.Beta), 2)) * (Math.Pow((currentScrew.Major + holeSize) / 4, 2) + Math.Pow(currentScrew.Pitch / (2 * Math.PI), 2)));
            var G2 = 2 * rf * Kf0 * Math.Cos(theta);

            var strength = Fit.LinearCombination(angularDoubleArray, torqueDoubleArray, (phi) =>
            {
                phi = Math.Max(phi - offset, -500);
                var zeta = ((phi > alpha) ? 1 : 0) * (phi - alpha);
                var psi = phi;

                var Tsz = (1 / alpha) * G1 * (psi - zeta);
                var Tfz = mu_thread * G2 * (phi - alpha / 2);

                return Tsz + Tfz;
            })[0];

            var theta_tau = Math.Atan2(currentScrew.Pitch, Math.PI * currentScrew.Shank) + Math.Atan2(1, -mu_thread);
            var tau_ult = strength;// / Math.Sqrt(3);

            var Ac_fail = Math.PI * currentScrew.Major * angularDoubleArray[length - 1] / (2 * Math.PI) * currentScrew.Pitch;

            //Console.WriteLine(angularDoubleArray[length - 1]/Math.PI*180);

            var T_fail = tau_ult * Ac_fail * ((1 / 3) * (Math.Pow(currentScrew.Head, 3) - Math.Pow(currentScrew.Shank, 3)) / (Math.Pow(currentScrew.Head, 2) - Math.Pow(currentScrew.Shank, 2)) 
                * mu_head * Math.Sin(theta_tau) - currentScrew.Major / 2 * Math.Cos(theta_tau)) * calfac;

            var l = (currentScrew.Major - currentScrew.Minor) / 2;
            var gamma = 2 * l / currentScrew.Pitch;
            var k_t = l + (1 + 1.5 * Math.Tanh(0.3 * Math.Log(gamma) + 0.7)) * Math.Sqrt(gamma);

            var T_fail_kz = T_fail / k_t ;



            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Estimated Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(strength / 1e6, 3)));
            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Known Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(currentStrength / 1e6, 3)));
            pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(torqueChart.Series.FindByName("Estimated Limit").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(Math.Max(T_fail, 0), 3)));

            if(autoStopBox.Checked && torqueDoubleArray[length-1] > T_fail*0.8 && angularDoubleArray[length - 1] > Math.PI * 20)
            {
                feedbackDevice.StopInsertion();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            feedbackDevice.StopInsertion();
            readDevice.OnDataPoint -= ReadDevice_OnDataPoint;
            updateCharts.Stop();
            while (pointsToAdd.Count > 0)
            {
                Tuple<DataPointCollection, double, double> point;
                pointsToAdd.TryDequeue(out point);
            }
        }

        private void TorqueReg_FormClosing(object sender, FormClosingEventArgs e)
        {
            feedbackDevice.StopInsertion();
            readDevice.OnDataPoint -= ReadDevice_OnDataPoint;
            updateCharts.Stop();
            while (pointsToAdd.Count > 0)
            {
                Tuple<DataPointCollection, double, double> point;
                pointsToAdd.TryDequeue(out point);
            }
        }

        private void ScrewListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentScrew = screws.First(s => s.Name == ScrewListBox.SelectedItem as string);
        }

        private void MaterialListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStrength = knownMaterials.First(m => m.Item1 == (string)MaterialListBox.SelectedItem).Item2;
        }
    }
}
