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
        private static int Time; // ����� ��������� �����

        // ���������� �������������� ������
        private static int NoDocs; // ����� ��� ����������
        private static int WithDrugs; // ����� �� �������� ���������� �����
        private static int PunishedPeoples; // ���������� �����

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "2";
            textBox2.Text = "3";
            textBox3.Text = "5";
            textBox4.Text = "10";
            textBox5.Text = "5";
            textBox6.Text = "15";
            textBox7.Text = "20";
            textBox8.Text = "1";
            textBox9.Text = "1";
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

            // 
            int localTime1 = 0; // ����� ������� ��������
            int localTime2 = 0; // ����� ������ ��������
            // 
            TimeSpeed = Convert.ToInt32(trackBar1.Value); // �������� �������������
            MinutesInDay = Days * 24 * 60; // ���������� ����� � ����
            int StepsCounter = 0; // ������� ����� �������������
            
            for (Time = 0; Time <= MinutesInDay; Time += deltaT) // ����� ��������� �����
            {
                Protocol = new string[5] { "", "", "", "", "" };
                StepsCounter += 1; // ���������� ����� �����
                if (Time - localTime1 >= 0)
                {
                    // ������ ������� ������ �� ���, ������ ���������� 1
                    QueueIn += 1;
                }

                if (Time - localTime2 >= 0)
                {
                    // ������ ������� ������ � ���, ������ ���������� 2
                    QueueOut += 1;
                }
                if ((QueueIn > 0 || QueueOut > 0) && IsChecking == false) // � �����-�� �� �������� ���� ���� � �������� ����� �� ���������
                {
                    // ������ �������� �������� �������� �������, � ������� ������ �������, ���������� 3
                    if (QueueIn > QueueOut)
                    {
                        QueueIn -= 1; // ������� ������� � ����� ���������
                        Random random = new Random();
                        int rnd = random.Next(0, 100); 
                        if (rnd > Delta1) // ������ ������� ��� ����������
                        {
                            NoDocs += 1; 
                        }
                        if (rnd > Delta2) // ������ ������� ������/����� ���-����
                        {
                            WithDrugs += 1;
                        }
                    }
                    else if (QueueOut > QueueIn)
                    {
                        QueueOut -= 1;
                        Random random = new Random();
                        int rnd = random.Next(0, 100);
                        if (rnd > Delta2) 
                        {

                        }
                    }
                    else
                    {
                        // ���� ����� ��������� � ����� ��������, �������� ������ �� �������
                    }

                    // ���� ������� ������� �����
                    if (QueueIn + QueueOut >= N)
                    {
                        // ����� �������� ��������� ������ ���������� ����� �� ������� �� �����, �� �������� ��, � ������� �� 1 ��������\

                        int Kicked = Math.Abs(N - QueueIn - QueueOut) + 1;
                        QueueOut -= Kicked;
                        FacesCount += Kicked;
                    }
                }
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
    }
}