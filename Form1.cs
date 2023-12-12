using System.Collections;
using Aspose.Cells; // Для выгрузки в Excel
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
        // Создадим необходимые переменные для моделирования
        private static double t1; // Человек пришел на предприятие
        private static double t2; // Человек ушел с предприятия
        private static double T; // Человека проверяют
        private static double Delta1; // Человек без документов, только уши, лапы и хвост
        private static double Delta2; // Человек принес что-то запрещенное (УК РФ 228-232)
        private static double dt; // Охранник уводит человека в комнату и делает с ним ВСЯКОЕ
        private static int N; // Размер стека охранника

        // Флаги моделирования
        private static bool IsRunning = false; // Проверка на работу моделирования
        private static bool IsChecking = false; // Правда - охранник занят и не может проверять, ложь - охранник свободен
        private static bool IsPunishing = false; // Флаг, говорящий о том, что нужно провести доп. проверку

        // По работе программы
        private static double TimeSpeed; // Для тракбара
        private static int MinutesInDay; // Количество минут в дне
        private static int Days; // Количество дней моделирования
        private static double deltaT; // Шаг времени
        private static string[] Protocol; // Для протоколирования
        private static int QueueIn; // Людей в очереди на вход
        private static int QueueOut; // Людей в очереди на выход
        private static int FacesCount; // Общее количество обработанных людей
        private static int NoCheckingPeoples = 0; // Выгнато из-за переполнения буфера
        private static double Time; // Общее модельное время
        private static double IsCheckingTime;
        private static double IsPunishingTime;


        // Интересные статистические данные
        private static int NoDocs; // Людей без документов
        private static int WithDrugs; // Людей со странным содержимым сумок
        private static int PunishedPeoples; // Наказанных людей
        private static string? WhatIsDoingSecurity;

        // Средние данные для вывода
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
            // Проверяем, сколько времени охранник будет занят доп проверкой
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
            // Проверяем, занят ли охранник
            if (IsCheckingTime > 0) // Если охранник находится в состоянии проверки
            {
                IsChecking = true;
            }
            else
            {
                IsChecking = false;
                IsCheckingTime = 0; // Обнуляем его время
            }
        }


        private void SecurityWorking()
        {
            WhatIsDoingSecurity = "Проверяет";
            IsCheckingTime += T; // Охранник приступает к проверке  очередного человека
            IsChecking = true;
            Random random = new Random();
            int rnd = random.Next(0, 100);
            // Новый генератор
            Generator generator = new Generator();
            Delta1 = generator.ExponentialFunction(0.1);
            textBox4.Text = Convert.ToString(Math.Round(Delta1, 4));
            Delta2 = generator.ExponentialFunction(0.2);
            textBox5.Text = Convert.ToString(Math.Round(Delta2, 4));
            if (rnd < Delta1 && rnd < Delta2) // Двойной нарушитель
            {
                NoDocs += 1;
                WithDrugs += 1;
                IsPunishingTime += dt;
                PunishedPeoples += 1;
                IsPunishing = true;
            }
            else if (rnd < Delta1) // Значит человек без документов
            {
                NoDocs += 1; // Добавляем к счетчику человек без документов
                IsPunishingTime += dt; // Увеличиваем время охранника
                PunishedPeoples += 1;
                IsPunishing = true;
            }
            else if (rnd < Delta2) // Значит человек принес/вынес что-либо
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
            QueueIn = 0; // Людей в очереди на вход
            QueueOut = 0; // Людей в очереди на выход
            FacesCount = 0; // Общее количество обработанных людей
            NoCheckingPeoples = 0; // Выгнато из-за переполнения буфера
            Time = 0; // Общее модельное время
            IsCheckingTime = 0;
            NoDocs = 0; // Людей без документов
            WithDrugs = 0; // Людей со странным содержимым сумок
            PunishedPeoples = 0; // Наказанных людей
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
            // Исходные данные стохастической модели
            t1 = Convert.ToDouble(textBox1.Text); // Новый гость
            t2 = Convert.ToDouble(textBox2.Text); // Кто-то уходит
            T = Convert.ToDouble(textBox3.Text); // Проверка человека
            Delta1 = Convert.ToDouble(textBox4.Text); // Человек без документов
            Delta2 = Convert.ToDouble(textBox5.Text); // Что-то принес или ворует
            dt = Convert.ToDouble(textBox6.Text); // Наказание человека по времени
            N = Convert.ToInt32(textBox7.Text); // Размер стека охранника 
            Days = Convert.ToInt32(textBox8.Text);
            deltaT = Convert.ToDouble(textBox9.Text);
            #endregion
            // Инициализация и обнуление нужных переменных
            double localTime1 = t1; // Локальное время прихода = Время прихода человека
            double localTime2 = t2; // Локальное время выхода = Время выхода человека
            ResetParams();
            // 
            TimeSpeed = Convert.ToInt32(trackBar1.Value); // Скорость моделирования
            MinutesInDay = Days * 24 * 60; // Количество минут в днях
            progressBar1.Maximum = MinutesInDay + 2; // Максимальное значение прогрессбара
            int StepsCounter = 0; // Счетчик шагов моделирования
            // Средние параметры
            double AvgChecking = 0;
            // LW № 3
            double[] C = { 0.37, 0.92, 0.03, 0.14 };
            StationaryGenerator stationaryGenerator = new StationaryGenerator(C, 2, 8);

            // Временные 
            double TempTime1 = 0;
            double TempTime2 = 0;

            // Обнуление переменных
            progressBar1.Value = progressBar1.Minimum;
            for (Time = 0; Time <= MinutesInDay && IsRunning; Time += deltaT) // Общее модельное время
            {
                Protocol = new string[9] { "", "", "", "", "", "", "", "", "" };
                StepsCounter += 1; // Количество шагов цикла
                if (Time - localTime1 >= 0)
                {
                    // Значит человек пришел на КПП, запуск активности 1
                    QueueIn += 1;
                    // Сюда еще впилить прогрессбар
                    AnimateInput();
                    AvgIn += 1;
                    TempTime1 = generator.NormalFunction(0.5, 2);
                    localTime1 = Time + TempTime1;
                    textBox1.Text = Convert.ToString(Math.Round(TempTime1, 4));
                }

                if (Time - localTime2 >= 0)
                {
                    // Значит человек уходит с КПП, запуск активности 2
                    QueueOut += 1;
                    // Сюда еще впилить прогрессбар
                    AnimateOutput();
                    AvgOut += 1;
                    TempTime2 = generator.NormalFunction(0.5, 3);
                    localTime2 = Time + TempTime2;
                    textBox2.Text = Convert.ToString(Math.Round(TempTime2, 4));
                }

                AvgIn += QueueIn;
                AvgOut += QueueOut;

                //IsCheckingFunction(); // Проверяем, как дела у охранника
                // Если охранник кого-то проверяет, то время всё равно идет, люди приходят и так далее
                if (!IsChecking && !IsPunishing)
                {
                    WhatIsDoingSecurity = "Бездельничает";
                    pictureBox1.Show();
                    pictureBox1.Image = Properties.Resources.sleep;
                }
                if (IsChecking)
                {
                    IsCheckingTime -= deltaT; // Отнимается время одной итерации цикла
                    WhatIsDoingSecurity = "Проверяет";
                    
                    AnimateCheck();
                    IsCheckingFunction();
                }
                if (IsPunishing && !IsChecking)
                {
                    IsPunishingTime -= deltaT;
                    WhatIsDoingSecurity = "Наказывает";

                    AnimatePunish();
                    IsPunishingFunction();
                }
                

                T = stationaryGenerator.GetNextValue();
                if ((QueueIn > 0 || QueueOut > 0) && IsChecking == false && IsPunishing == false) // В какой-то из очередей есть люди и охранник готов их проверять
                {
                    //UPDATED после защиты: T для третьей лабораторной работы
                    AvgT += T;
                    textBox3.Text = Convert.ToString(Math.Round(T, 4));
                    // Значит охранник начинает проверку очереди, в которой больше человек, активность 3
                    if (QueueIn > QueueOut)
                    {
                        QueueIn -= 1; // Человек заходит в рубку охранника
                        SecurityWorking();
                    }
                    else if (QueueOut > QueueIn)
                    {
                        QueueOut -= 1;
                        SecurityWorking();
                    }
                }


                // Если человек много
                if (QueueIn + QueueOut >= N)
                {
                    // То всех на выход выпускаем без проблем
                    NoCheckingPeoples += QueueOut;
                    QueueOut = 0;
                    
                }
                await Task.Delay((1000 * Convert.ToInt32(deltaT) / 25) * Convert.ToInt32(TimeSpeed)); // Программная задержка
                Protocol[0] = Time.ToString(); // Общее модельное время
                Protocol[1] = QueueIn.ToString(); // Количество людей в очереди на ВХОД (В текущий момент времени)
                Protocol[2] = QueueOut.ToString(); // Количество людей в очереди на ВЫХОД (аналогично)
                Protocol[3] = FacesCount.ToString(); // Количество обработанных людей
                Protocol[4] = NoDocs.ToString(); // Количество людей без документов
                Protocol[5] = WithDrugs.ToString(); // С грузом
                Protocol[6] = PunishedPeoples.ToString(); // Наказано нарушителей
                Protocol[7] = NoCheckingPeoples.ToString(); // Выгнали из-за переполнения буфера
                Protocol[8] = WhatIsDoingSecurity; // Чем в данный момент времени занят охранник
                Security security1 = new Security(Protocol); // Для протоколирования в Эксель
                SecurityList.Add(security1);
                progressBar1.Value += Convert.ToInt32(deltaT);
                dataGridView1.Rows.Add(Protocol);
            }
            richTextBox2.Text = $"Средняя длина очереди на вход: {Math.Round(AvgIn / MinutesInDay, 0)} \n" +
                            $"Средняя длина очереди на выход: {Math.Round(AvgOut / MinutesInDay, 0)} \n" +
                            $"Среднее время, затраченное на проверки людей: {Math.Round(AvgT / FacesCount, 4)} \n" +
                            $"Общее время, затраченное на проверки людей: {Math.Round(AvgT, 4)} \n" +
                            $"Количество людей, не прошедших проверку: {PunishedPeoples}";

            ExcelDumping();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                button1.Text = "Стоп";
                dataGridView1.Rows.Clear();
                main();
            }
            else
            {
                IsRunning = false;
                button1.Text = "Поехали!";
                
            }
        }

        //https://www.cyberforum.ru/ado-net/thread2809628.html
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (row.Cells["Column9"].Value != null)
                {
                    if (row.Cells["Column9"].Value.ToString() == "Наказывает")
                    {
                        row.Cells["Column9"].Style.BackColor = Color.Red;
                        row.Cells["Column9"].Style.ForeColor = Color.White;
                    }
                    else if (row.Cells["Column9"].Value.ToString() == "Бездельничает")
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
        // Для протоколирования
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

        // Код для второй лабораторной работы
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

        // Третья ЛР будет выполняться в третьей ветке - LW_3
        public class StationaryGenerator
        {
            private double M; // Математическое ожидание
            private List<double> q; // Величины, распределенные нормально
            private double[] C; // Найденные значения (из СЛАУ)
            private Generator General = new Generator(); // Генератор из ЛР №2

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

        // Вторая ЛР будет выполняться во второй ветке - LW_2
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
                // Параметр T должен иметь разброс от 2 до 8
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
            // Параметр T должен иметь разброс от 2 до 8 UPDATED после защиты: T для третьей лабораторной работы
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
            ArrayList FirstStationaryArray = new ArrayList(); // Нечетные значения (y')
            ArrayList SecondStationaryArray = new ArrayList(); // Четные значения (y'')
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