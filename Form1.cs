using System.Collections;
using Aspose.Cells; // ��� �������� � Excel

namespace KPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // �������� ����������� ���������� ��� �������������
        private static int t1; // ������� ������ �� �����������
        private static int t2; // ������� ���� � �����������
        private static int T; // �������� ���������
        private static int Delta1; // ������� ��� ����������, ������ ���, ���� � �����
        private static int Delta2; // ������� ������ ���-�� ����������� (�� �� 228-232)
        private static int dt; // �������� ������ �������� � ������� � ������ � ��� ������
        private static int N; // ������ ����� ���������

        // ����� �������������
        private static bool IsRunning = false; // �������� �� ������ �������������
        private static bool IsChecking = false; // ������ - �������� ����� � �� ����� ���������, ���� - �������� ��������
        private static bool IsPunishing = false; // ����, ��������� � ���, ��� ����� �������� ���. ��������

        // �� ������ ���������
        private static int TimeSpeed; // ��� ��������
        private static int MinutesInDay; // ���������� ����� � ���
        private static int Days; // ���������� ���� �������������
        private static int deltaT; // ��� �������
        private static string[] Protocol; // ��� ����������������
        private static int QueueIn; // ����� � ������� �� ����
        private static int QueueOut; // ����� � ������� �� �����
        private static int FacesCount; // ����� ���������� ������������ �����
        private static int NoCheckingPeoples = 0; // ������� ��-�� ������������ ������
        private static int Time; // ����� ��������� �����
        private static int IsCheckingTime;
        private static int IsPunishingTime;


        // ���������� �������������� ������
        private static int NoDocs; // ����� ��� ����������
        private static int WithDrugs; // ����� �� �������� ���������� �����
        private static int PunishedPeoples; // ���������� �����
        private static string? WhatIsDoingSecurity;

        
        

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
            pictureBox3.Hide();
            pictureBox4.Hide();
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
                //WhatIsDoingSecurity = "����������";
                IsPunishing = true;
            }
            else if (rnd < Delta2) // ������ ������� ������/����� ���-����
            {
                WithDrugs += 1;
                IsPunishingTime += dt;
                PunishedPeoples += 1;
                //WhatIsDoingSecurity = "����������";
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

        async private void main()
        {
            #region stohasparams
            // �������� ������ �������������� ������
            t1 = Convert.ToInt32(textBox1.Text); // ����� �����
            t2 = Convert.ToInt32(textBox2.Text); // ���-�� ������
            T = Convert.ToInt32(textBox3.Text); // �������� ��������
            Delta1 = Convert.ToInt32(textBox4.Text); // ������� ��� ����������
            Delta2 = Convert.ToInt32(textBox5.Text); // ���-�� ������ ��� ������
            dt = Convert.ToInt32(textBox6.Text); // ��������� �������� �� �������
            N = Convert.ToInt32(textBox7.Text); // ������ ����� ��������� 
            Days = Convert.ToInt32(textBox8.Text);
            deltaT = Convert.ToInt32(textBox9.Text);
            #endregion
            // ������������� � ��������� ������ ����������
            int localTime1 = t1; // ��������� ����� ������� = ����� ������� ��������
            int localTime2 = t2; // ��������� ����� ������ = ����� ������ ��������
            ResetParams();
            // 
            TimeSpeed = Convert.ToInt32(trackBar1.Value); // �������� �������������
            MinutesInDay = Days * 24 * 60; // ���������� ����� � ����
            progressBar1.Maximum = MinutesInDay + 2; // ������������ �������� ������������
            int StepsCounter = 0; // ������� ����� �������������

            // ��������� ����������
            progressBar1.Value = progressBar1.Minimum;
            for (Time = 0; Time <= MinutesInDay; Time += deltaT) // ����� ��������� �����
            {
                Protocol = new string[9] { "", "", "", "", "", "", "", "", "" };
                StepsCounter += 1; // ���������� ����� �����
                if (Time - localTime1 >= 0)
                {
                    // ������ ������� ������ �� ���, ������ ���������� 1
                    QueueIn += 1;
                    // ���� ��� ������� �����������
                    localTime1 = Time + t1;
                }

                if (Time - localTime2 >= 0)
                {
                    // ������ ������� ������ � ���, ������ ���������� 2
                    QueueOut += 1;
                    // ���� ��� ������� �����������
                    localTime2 = Time + t2;
                }

                //IsCheckingFunction(); // ���������, ��� ���� � ���������
                // ���� �������� ����-�� ���������, �� ����� �� ����� ����, ���� �������� � ��� �����
                if (!IsChecking && !IsPunishing)
                {
                    WhatIsDoingSecurity = "�������������";
                }
                if (IsChecking)
                {
                    IsCheckingTime -= deltaT; // ���������� ����� ����� �������� �����
                    WhatIsDoingSecurity = "���������";
                    IsCheckingFunction();
                }
                if (IsPunishing && !IsChecking)
                {
                    IsPunishingTime -= deltaT;
                    WhatIsDoingSecurity = "����������";
                    IsPunishingFunction();
                }
                

                if ((QueueIn > 0 || QueueOut > 0) && IsChecking == false && IsPunishing == false) // � �����-�� �� �������� ���� ���� � �������� ����� �� ���������
                {
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
                await Task.Delay((1000 * deltaT / 10) * TimeSpeed); // ����������� ��������
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
                progressBar1.Value += deltaT;
                dataGridView1.Rows.Add(Protocol);
            }
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

        // ��� ����������������
        public class Security
        {
            public int Time { get; set; }
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
                Time = Convert.ToInt32(buffer[0]);
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

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            pictureBox3.Show();
            pictureBox4.Show();
            label10.Show();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

       
    }
}