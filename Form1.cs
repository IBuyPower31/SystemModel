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
            
        }

        private void IsCheckingFunction()
        {
            // ���������, ����� �� ��������
            if (IsCheckingTime >= 0) // ���� �������� ��������� � ��������� ��������
            {
                IsChecking = true;
                WhatIsDoingSecurity = "���������";
            }
            else
            {
                IsChecking = false;
                IsCheckingTime = 0; // �������� ��� �����
                WhatIsDoingSecurity = "�������������";
            }
            if (IsCheckingTime >= T)
            {
                WhatIsDoingSecurity = "����������";
            }
        }

        private void SecurityWorking()
        {
            WhatIsDoingSecurity = "���������";
            IsCheckingTime += T; // �������� ���������� � ��������  ���������� ��������
            Random random = new Random();
            int rnd = random.Next(0, 100);
            if (rnd < Delta1 && rnd < Delta2) // ������� ����������
            {
                NoDocs += 1;
                WithDrugs += 1;
                IsCheckingTime += dt;
                PunishedPeoples += 1;
            }
            else if (rnd < Delta1) // ������ ������� ��� ����������
            {
                NoDocs += 1; // ��������� � �������� ������� ��� ����������
                IsCheckingTime += dt; // ����������� ����� ���������
                PunishedPeoples += 1;
                WhatIsDoingSecurity = "����������";
            }
            else if (rnd < Delta2) // ������ ������� ������/����� ���-����
            {
                WithDrugs += 1;
                IsCheckingTime += dt;
                PunishedPeoples += 1;
                WhatIsDoingSecurity = "����������";
            }
            FacesCount += 1;
        }


        async private void main()
        {
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

            // 
            int localTime1 = t1; // ��������� ����� ������� = ����� ������� ��������
            int localTime2 = t2; // ��������� ����� ������ = ����� ������ ��������
            // 
            TimeSpeed = Convert.ToInt32(trackBar1.Value); // �������� �������������
            MinutesInDay = Days * 24 * 60; // ���������� ����� � ����
            progressBar1.Maximum = MinutesInDay + 2; // ������������ �������� ������������
            int StepsCounter = 0; // ������� ����� �������������
            
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

                IsCheckingFunction(); // ���������, ��� ���� � ���������
                // ���� �������� ����-�� ���������, �� ����� �� ����� ����, ���� �������� � ��� �����
                if (IsChecking)
                {
                    IsCheckingTime -= deltaT; // ���������� ����� ����� �������� ����� 
                }
                IsCheckingFunction();

                if ((QueueIn > 0 || QueueOut > 0) && IsChecking == false) // � �����-�� �� �������� ���� ���� � �������� ����� �� ���������
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
                progressBar1.Value += deltaT;
                dataGridView1.Rows.Add(Protocol);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                button1.Text = "����";
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
    }
}