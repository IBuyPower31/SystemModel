using System.Collections;
using Aspose.Cells; // ��� �������� � Excel
using System.Threading;

namespace KPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region startparams
        // �������� ����������� ���������� ��� �������������
        private static double t1; // ������� ������ �� �����������
        private static double t2; // ������� ���� � �����������
        private static double T; // �������� ���������
        private static double Delta1; // ������� ��� ����������, ������ ���, ���� � �����
        private static double Delta2; // ������� ������ ���-�� ����������� (�� �� 228-232)
        private static double dt; // �������� ������ �������� � ������� � ������ � ��� ������
        private static int N; // ������ ����� ���������

        // ����� �������������
        private static bool IsRunning = false; // �������� �� ������ �������������
        private static bool IsChecking = false; // ������ - �������� ����� � �� ����� ���������, ���� - �������� ��������
        private static bool IsPunishing = false; // ����, ��������� � ���, ��� ����� �������� ���. ��������

        // �� ������ ���������
        private static double TimeSpeed; // ��� ��������
        private static int MinutesInDay; // ���������� ����� � ���
        private static int Days; // ���������� ���� �������������
        private static double deltaT; // ��� �������
        private static string[] Protocol; // ��� ����������������
        private static int QueueIn; // ����� � ������� �� ����
        private static int QueueOut; // ����� � ������� �� �����
        private static int FacesCount; // ����� ���������� ������������ �����
        private static int NoCheckingPeoples = 0; // ������� ��-�� ������������ ������
        private static double Time; // ����� ��������� �����
        private static double IsCheckingTime;
        private static double IsPunishingTime;


        // ���������� �������������� ������
        private static int NoDocs; // ����� ��� ����������
        private static int WithDrugs; // ����� �� �������� ���������� �����
        private static int PunishedPeoples; // ���������� �����
        private static string? WhatIsDoingSecurity;

        // ������� ������ ��� ������
        private static double AvgPunishingTime;
        private static double AvgIn;
        private static double AvgOut;
        private static double AvgT;
        #endregion



        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "2";
            textBox2.Text = "3";
            textBox3.Text = "5";
            textBox4.Text = "10";
            textBox5.Text = "5";
            textBox6.Text = "15";
            textBox7.Text = "200";
            textBox8.Text = "1";
            textBox9.Text = "1";
            progressBar1.Minimum = 0;
            pictureBox1.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            label10.Hide();
            
        }

        private void IsPunishingFunction()
        {
            // ���������, ������� ������� �������� ����� ����� ��� ���������
            if (IsPunishingTime >= 0)
            {
                IsPunishing = true;
            }
            else
            {
                IsPunishing = false;
                IsPunishingTime = 0;
            }
        }

        private void IsCheckingFunction()
        {
            // ���������, ����� �� ��������
            if (IsCheckingTime > 0) // ���� �������� ��������� � ��������� ��������
            {
                IsChecking = true;
            }
            else
            {
                IsChecking = false;
                IsCheckingTime = 0; // �������� ��� �����
            }
        }


        private void SecurityWorking()
        {
            WhatIsDoingSecurity = "���������";
            IsCheckingTime += T; // �������� ���������� � ��������  ���������� ��������
            IsChecking = true;
            Random random = new Random();
            int rnd = random.Next(0, 100);
            // ����� ���������
            Generator generator = new Generator();
            Delta1 = generator.ExponentialFunction(0.1);
            textBox4.Text = Convert.ToString(Math.Round(Delta1, 4));
            Delta2 = generator.ExponentialFunction(0.2);
            textBox5.Text = Convert.ToString(Math.Round(Delta2, 4));
            if (rnd < Delta1 && rnd < Delta2) // ������� ����������
            {
                NoDocs += 1;
                WithDrugs += 1;
                IsPunishingTime += dt;
                PunishedPeoples += 1;
                IsPunishing = true;
            }
            else if (rnd < Delta1) // ������ ������� ��� ����������
            {
                NoDocs += 1; // ��������� � �������� ������� ��� ����������
                IsPunishingTime += dt; // ����������� ����� ���������
                PunishedPeoples += 1;
                IsPunishing = true;
            }
            else if (rnd < Delta2) // ������ ������� ������/����� ���-����
            {
                WithDrugs += 1;
                IsPunishingTime += dt;
                PunishedPeoples += 1;
                IsPunishing = true;
            }
            FacesCount += 1;
        }


        private void ResetParams()
        {
            QueueIn = 0; // ����� � ������� �� ����
            QueueOut = 0; // ����� � ������� �� �����
            FacesCount = 0; // ����� ���������� ������������ �����
            NoCheckingPeoples = 0; // ������� ��-�� ������������ ������
            Time = 0; // ����� ��������� �����
            IsCheckingTime = 0;
            NoDocs = 0; // ����� ��� ����������
            WithDrugs = 0; // ����� �� �������� ���������� �����
            PunishedPeoples = 0; // ���������� �����
            WhatIsDoingSecurity = "";
            IsPunishing = false;
            IsChecking = false;
        }

        private void AnimateInput()
        {
            pictureBox5.Show();
            pictureBox5.Top += 35;
            if (pictureBox5.Top > 625)
            {
                pictureBox5.Top = 45;
            }
            
        }

        private void AnimateOutput()
        {
            pictureBox6.Show();
            pictureBox6.Top -= 35;
            if (pictureBox6.Top < 45)
            {
                pictureBox6.Top = 625;
            }
        }

        private void AnimateCheck()
        {
            pictureBox1.Show();
            pictureBox1.Image = Properties.Resources.check;
        }

        private void AnimatePunish()
        {

            pictureBox1.Image = Properties.Resources.fight;
        }

        async private void main()
        {
            #region stohasparams
            Generator generator = new Generator();
            // �������� ������ �������������� ������
            t1 = Convert.ToDouble(textBox1.Text); // ����� �����
            t2 = Convert.ToDouble(textBox2.Text); // ���-�� ������
            T = Convert.ToDouble(textBox3.Text); // �������� ��������
            Delta1 = Convert.ToDouble(textBox4.Text); // ������� ��� ����������
            Delta2 = Convert.ToDouble(textBox5.Text); // ���-�� ������ ��� ������
            dt = Convert.ToDouble(textBox6.Text); // ��������� �������� �� �������
            N = Convert.ToInt32(textBox7.Text); // ������ ����� ��������� 
            Days = Convert.ToInt32(textBox8.Text);
            deltaT = Convert.ToDouble(textBox9.Text);
            #endregion
            // ������������� � ��������� ������ ����������
            double localTime1 = t1; // ��������� ����� ������� = ����� ������� ��������
            double localTime2 = t2; // ��������� ����� ������ = ����� ������ ��������
            ResetParams();
            // 
            TimeSpeed = Convert.ToInt32(trackBar1.Value); // �������� �������������
            MinutesInDay = Days * 24 * 60; // ���������� ����� � ����
            progressBar1.Maximum = MinutesInDay + 2; // ������������ �������� ������������
            int StepsCounter = 0; // ������� ����� �������������
            // ������� ���������
            double AvgChecking = 0;
            // LW � 3
            double[] C = { 0.37, 0.92, 0.03, 0.14 };
            StationaryGenerator stationaryGenerator = new StationaryGenerator(C, 2, 8);

            // ��������� 
            double TempTime1 = 0;
            double TempTime2 = 0;

            // ��������� ����������
            progressBar1.Value = progressBar1.Minimum;
            for (Time = 0; Time <= MinutesInDay && IsRunning; Time += deltaT) // ����� ��������� �����
            {
                Protocol = new string[9] { "", "", "", "", "", "", "", "", "" };
                StepsCounter += 1; // ���������� ����� �����
                if (Time - localTime1 >= 0)
                {
                    // ������ ������� ������ �� ���, ������ ���������� 1
                    QueueIn += 1;
                    // ���� ��� ������� �����������
                    AnimateInput();
                    AvgIn += 1;
                    TempTime1 = generator.NormalFunction(0.5, 2);
                    localTime1 = Time + TempTime1;
                    textBox1.Text = Convert.ToString(Math.Round(TempTime1, 4));
                }

                if (Time - localTime2 >= 0)
                {
                    // ������ ������� ������ � ���, ������ ���������� 2
                    QueueOut += 1;
                    // ���� ��� ������� �����������
                    AnimateOutput();
                    AvgOut += 1;
                    TempTime2 = generator.NormalFunction(0.5, 3);
                    localTime2 = Time + TempTime2;
                    textBox2.Text = Convert.ToString(Math.Round(TempTime2, 4));
                }

                AvgIn += QueueIn;
                AvgOut += QueueOut;

                //IsCheckingFunction(); // ���������, ��� ���� � ���������
                // ���� �������� ����-�� ���������, �� ����� �� ����� ����, ���� �������� � ��� �����
                if (!IsChecking && !IsPunishing)
                {
                    WhatIsDoingSecurity = "�������������";
                    pictureBox1.Show();
                    pictureBox1.Image = Properties.Resources.sleep;
                }
                if (IsChecking)
                {
                    IsCheckingTime -= deltaT; // ���������� ����� ����� �������� �����
                    WhatIsDoingSecurity = "���������";
                    
                    AnimateCheck();
                    IsCheckingFunction();
                }
                if (IsPunishing && !IsChecking)
                {
                    IsPunishingTime -= deltaT;
                    WhatIsDoingSecurity = "����������";

                    AnimatePunish();
                    IsPunishingFunction();
                }
                

                T = stationaryGenerator.GetNextValue();
                if ((QueueIn > 0 || QueueOut > 0) && IsChecking == false && IsPunishing == false) // � �����-�� �� �������� ���� ���� � �������� ����� �� ���������
                {
                    //UPDATED ����� ������: T ��� ������� ������������ ������
                    AvgT += T;
                    textBox3.Text = Convert.ToString(Math.Round(T, 4));
                    // ������ �������� �������� �������� �������, � ������� ������ �������, ���������� 3
                    if (QueueIn > QueueOut)
                    {
                        QueueIn -= 1; // ������� ������� � ����� ���������
                        SecurityWorking();
                    }
                    else if (QueueOut > QueueIn)
                    {
                        QueueOut -= 1;
                        SecurityWorking();
                    }
                }


                // ���� ������� �����
                if (QueueIn + QueueOut >= N)
                {
                    // �� ���� �� ����� ��������� ��� �������
                    NoCheckingPeoples += QueueOut;
                    QueueOut = 0;
                    
                }
                await Task.Delay((1000 * Convert.ToInt32(deltaT) / 25) * Convert.ToInt32(TimeSpeed)); // ����������� ��������
                Protocol[0] = Time.ToString(); // ����� ��������� �����
                Protocol[1] = QueueIn.ToString(); // ���������� ����� � ������� �� ���� (� ������� ������ �������)
                Protocol[2] = QueueOut.ToString(); // ���������� ����� � ������� �� ����� (����������)
                Protocol[3] = FacesCount.ToString(); // ���������� ������������ �����
                Protocol[4] = NoDocs.ToString(); // ���������� ����� ��� ����������
                Protocol[5] = WithDrugs.ToString(); // � ������
                Protocol[6] = PunishedPeoples.ToString(); // �������� �����������
                Protocol[7] = NoCheckingPeoples.ToString(); // ������� ��-�� ������������ ������
                Protocol[8] = WhatIsDoingSecurity; // ��� � ������ ������ ������� ����� ��������
                Security security1 = new Security(Protocol); // ��� ���������������� � ������
                SecurityList.Add(security1);
                progressBar1.Value += Convert.ToInt32(deltaT);
                dataGridView1.Rows.Add(Protocol);
            }
            richTextBox2.Text = $"������� ����� ������� �� ����: {Math.Round(AvgIn / MinutesInDay, 0)} \n" +
                            $"������� ����� ������� �� �����: {Math.Round(AvgOut / MinutesInDay, 0)} \n" +
                            $"������� �����, ����������� �� �������� �����: {Math.Round(AvgT / FacesCount, 4)} \n" +
                            $"����� �����, ����������� �� �������� �����: {Math.Round(AvgT, 4)} \n" +
                            $"���������� �����, �� ��������� ��������: {PunishedPeoples}";

            ExcelDumping();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                button1.Text = "����";
                dataGridView1.Rows.Clear();
                main();
            }
            else
            {
                IsRunning = false;
                button1.Text = "�������!";
                
            }
        }

        //https://www.cyberforum.ru/ado-net/thread2809628.html
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["Column9"].Value != null)
                {
                    if (row.Cells["Column9"].Value.ToString() == "����������")
                    {
                        row.Cells["Column9"].Style.BackColor = Color.Red;
                        row.Cells["Column9"].Style.ForeColor = Color.White;
                    }
                    else if (row.Cells["Column9"].Value.ToString() == "�������������")
                    {
                        row.Cells["Column9"].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Column9"].Style.BackColor = Color.Yellow;
                    }
                }
            }
        }


        #region protocoling
        // ��� ����������������
        public class Security
        {
            public double Time { get; set; }
            public int Input { get; set; }
            public int Output { get; set; }
            public int Processing { get; set; }
            public int NoDocs { get; set; }
            public int WithDrugs { get; set; }
            public int PunishedPeoples { get; set; }
            public int NoCheckingPeoples { get; set; }
            public string WhatIsDoingSecurity { get; set; }


            public Security(string[] buffer)
            {
                Time = Convert.ToDouble(buffer[0]);
                Input = Convert.ToInt32(buffer[1]);
                Output = Convert.ToInt32(buffer[2]);
                Processing = Convert.ToInt32(buffer[3]);
                NoDocs = Convert.ToInt32(buffer[4]);
                WithDrugs = Convert.ToInt32(buffer[5]);
                PunishedPeoples = Convert.ToInt32(buffer[6]);
                NoCheckingPeoples = Convert.ToInt32(buffer[7]);
                WhatIsDoingSecurity = buffer[8];
            }
        }

        public List<Security> SecurityList = new List<Security>();

        // ��� ��� ������ ������������ ������
        public class Generator
        {
            private readonly Random ExponentialRandom = new Random();

            private readonly Random NormalRandom = new Random();

            public double ExponentialFunction(double lambda)
            {
                return -(1 / lambda) * Math.Log(ExponentialRandom.NextDouble());
            }

            public double NormalFunction(double sigma, double m)
            {
                return (sigma * Math.Cos(2 * Math.PI * NormalRandom.NextDouble()) 
                    * Math.Sqrt(-2 * Math.Log(NormalRandom.NextDouble()))) + m;
            }
        }

        // ������ �� ����� ����������� � ������� ����� - LW_3
        public class StationaryGenerator
        {
            private double M; // �������������� ��������
            private List<double> q; // ��������, �������������� ���������
            private double[] C; // ��������� �������� (�� ����)
            private Generator General = new Generator(); // ��������� �� �� �2

            public StationaryGenerator(double[] C, double min, double max)
            {
                this.C = C;
                M = (max + min) / 2;
                q = new List<double>();
                for (int i = 0; i < C.Length; i++)
                {
                    q.Add(General.NormalFunction(1, 0));
                }
            }

            public double GetStartValue()
            {
                var value = Enumerable.Range(0, C.Length).Select(i => C[i] * q[i]).Sum() + M;
                return value;
            }

            public double GetNextValue()
            {
                q.Remove(q.FirstOrDefault());
                q.Add(General.NormalFunction(1, 0));
                var value = Enumerable.Range(0, C.Length).Select(i => C[i] * q[i]).Sum() + M;
                return value;
            }
        }


        private void ExcelDumping()
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets.Add("Data");
            sheet.Cells.ImportCustomObjects((ICollection)SecurityList,
                new string[] { "Time", "Input", "Output", "Processing", "NoDocs", "WithDrugs", "PunishedPeoples", "NoCheckingPeoples", "WhatIsDoingSecurity" },
                true,
                0,
                0,
                SecurityList.Count,
                true,
                null,
                false
                );
            sheet.AutoFitColumns();
            workbook.Save("Protocol.xlsx");
        }

        #endregion

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            pictureBox3.Show();
            pictureBox4.Show();
            label10.Show();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        // ������ �� ����� ����������� �� ������ ����� - LW_2
        private void button2_Click(object sender, EventArgs e)
        {
            const int N = 2000;
            progressBar2.Minimum = 0;
            progressBar2.Maximum = N;
            Generator generator = new Generator();

            ArrayList NormalArray1 = new ArrayList();
            ArrayList NormalArray2 = new ArrayList();
            ArrayList NormalArray3 = new ArrayList();
            ArrayList ExponentialArray1 = new ArrayList();
            ArrayList ExponentialArray2 = new ArrayList();

            double delta1, delta2, t1, t2, T;

            for (int i = 0; i < N; i++)
            {
                delta1 = generator.ExponentialFunction(0.1);
                ExponentialArray1.Add(delta1);
                delta2 = generator.ExponentialFunction(0.2);
                ExponentialArray2.Add(delta2);

                t1 = generator.NormalFunction(0.5, 2);
                NormalArray1.Add(t1);
                t2 = generator.NormalFunction(0.5, 3);
                NormalArray2.Add(t2);
                // �������� T ������ ����� ������� �� 2 �� 8
                T = generator.NormalFunction(0.75, 5);
                NormalArray3.Add(T);
                progressBar2.Value += 1;
            }

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets.Add("NormalFunction");
            sheet.Cells.ImportArrayList(NormalArray1, 1, 0, true);
            sheet.Cells.ImportArrayList(NormalArray2, 1, 1, true);
            sheet.Cells.ImportArrayList(NormalArray3, 1, 2, true);
            sheet.AutoFitColumns();

            Worksheet sheet1 = workbook.Worksheets.Add("ExponentialFunction");
            sheet1.Cells.ImportArrayList(ExponentialArray1, 1, 0, true);
            sheet1.Cells.ImportArrayList(ExponentialArray2, 1, 1, true);
            sheet1.AutoFitColumns();
            workbook.Save("ExponentialAndNormalFunction.xlsx");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Generator generator = new Generator();
            Delta1 = generator.ExponentialFunction(0.1);
            Delta2 = generator.ExponentialFunction(0.2);
            textBox4.Text = Convert.ToString(Delta1);
            textBox5.Text = Convert.ToString(Delta2);
            t1 = generator.NormalFunction(0.5, 2);
            t2 = generator.NormalFunction(0.5, 3);
            // �������� T ������ ����� ������� �� 2 �� 8 UPDATED ����� ������: T ��� ������� ������������ ������
            T = generator.NormalFunction(0.75, 5);
            textBox1.Text = Convert.ToString(t1);
            textBox2.Text = Convert.ToString(t2);
            textBox3.Text = Convert.ToString(T);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            double[] C = {0.37, 0.92, 0.03, 0.14 };
            ArrayList FirstStationaryArray = new ArrayList(); // �������� �������� (y')
            ArrayList SecondStationaryArray = new ArrayList(); // ������ �������� (y'')
            StationaryGenerator stationaryGenerator = new StationaryGenerator(C, 2, 8);
            for (int i = 0; i < 2000; i++)
            {
                if (i % 2 == 0)
                {
                    SecondStationaryArray.Add(stationaryGenerator.GetNextValue());
                }
                else
                {
                    FirstStationaryArray.Add(stationaryGenerator.GetNextValue());
                }
            }
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets.Add("StationaryProcess");
            sheet.Cells.ImportArrayList(FirstStationaryArray, 1, 0, true);
            sheet.Cells.ImportArrayList(SecondStationaryArray, 1, 1, true);
            sheet.AutoFitColumns();
            FirstStationaryArray.Clear();
            SecondStationaryArray.Clear();
            workbook.Save("StationaryProcess.xlsx");
        }
    }
}