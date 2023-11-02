namespace KPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Создадим необходимые переменные для моделирования
        private static int t1; // Человек пришел на предприятие
        private static int t2; // Человек ушел с предприятия
        private static int T; // Человека проверяют
        private static int Delta1; // Человек без документов, только уши, лапы и хвост
        private static int Delta2; // Человек принес что-то запрещенное (УК РФ 228-232)
        private static int dt; // Охранник уводит человека в комнату и делает с ним ВСЯКОЕ
        private static int N; // Размер стека охранника

        // Флаги моделирования
        private static bool IsRunning = false; // Проверка на работу моделирования
        private static bool IsChecking = false; // Правда - охранник занят и не может проверять, ложь - охранник свободен

        // По работе программы
        private static int TimeSpeed; // Для тракбара
        private static int MinutesInDay; // Количество минут в дне
        private static int Days; // Количество дней моделирования
        private static int deltaT; // Шаг времени
        private static string[] Protocol; // Для протоколирования
        private static int QueueIn; // Людей в очереди на вход
        private static int QueueOut; // Людей в очереди на выход
        private static int FacesCount; // Общее количество обработанных людей
        private static int NoCheckingPeoples = 0; // Выгнато из-за переполнения буфера
        private static int Time; // Общее модельное время
        private static int IsCheckingTime;

        // Интересные статистические данные
        private static int NoDocs; // Людей без документов
        private static int WithDrugs; // Людей со странным содержимым сумок
        private static int PunishedPeoples; // Наказанных людей
        private static string WhatIsDoingSecurity;
       

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
        }

        private void IsCheckingFunction()
        {
            // Проверяем, занят ли охранник
            if (IsCheckingTime >= 0) // Если охранник находится в состоянии проверки
            {
                IsChecking = true;
                WhatIsDoingSecurity = "Проверяет";
            }
            else
            {
                IsChecking = false;
                IsCheckingTime = 0; // Обнуляем его время
                WhatIsDoingSecurity = "Бездельничает";
            }
            if (IsCheckingTime >= T)
            {
                WhatIsDoingSecurity = "Наказывает";
            }
        }

        private void SecurityWorking()
        {
            WhatIsDoingSecurity = "Проверяет";
            IsCheckingTime += T; // Охранник приступает к проверке  очередного человека
            Random random = new Random();
            int rnd = random.Next(0, 100);
            if (rnd < Delta1) // Значит человек без документов
            {
                NoDocs += 1; // Добавляем к счетчику человек без документов
                IsCheckingTime += dt; // Увеличиваем время охранника
                PunishedPeoples += 1;
                WhatIsDoingSecurity = "Наказывает";
            }
            if (rnd < Delta2) // Значит человек принес/вынес что-либо
            {
                WithDrugs += 1;
                IsCheckingTime += dt;
                PunishedPeoples += 1;
                WhatIsDoingSecurity = "Наказывает";
            }
            FacesCount += 1;
        }


        async private void main()
        {
            // Исходные данные стохастической модели
            t1 = Convert.ToInt32(textBox1.Text); // Новый гость
            t2 = Convert.ToInt32(textBox2.Text); // Кто-то уходит
            T = Convert.ToInt32(textBox3.Text); // Проверка человека
            Delta1 = Convert.ToInt32(textBox4.Text); // Человек без документов
            Delta2 = Convert.ToInt32(textBox5.Text); // Что-то принес или ворует
            dt = Convert.ToInt32(textBox6.Text); // Наказание человека по времени
            N = Convert.ToInt32(textBox7.Text); // Размер стека охранника 
            Days = Convert.ToInt32(textBox8.Text);
            deltaT = Convert.ToInt32(textBox9.Text);

            // 
            int localTime1 = 0; // Время прихода человека
            int localTime2 = 0; // Время выхода человека
            // 
            TimeSpeed = Convert.ToInt32(trackBar1.Value); // Скорость моделирования
            MinutesInDay = Days * 24 * 60; // Количество минут в днях
            int StepsCounter = 0; // Счетчик шагов моделирования
            
            for (Time = 0; Time <= MinutesInDay; Time += deltaT) // Общее модельное время
            {
                Protocol = new string[9] { "", "", "", "", "", "", "", "", "" };
                StepsCounter += 1; // Количество шагов цикла
                if (Time - localTime1 >= 0)
                {
                    // Значит человек пришел на КПП, запуск активности 1
                    QueueIn += 1;
                    // Сюда еще впилить прогрессбар
                    localTime1 = Time + t1;
                }

                if (Time - localTime2 >= 0)
                {
                    // Значит человек уходит с КПП, запуск активности 2
                    QueueOut += 1;
                    // Сюда еще впилить прогрессбар
                    localTime2 = Time + t2;
                }

                IsCheckingFunction(); // Проверяем, как дела у охранника
                // Если охранник кого-то проверяет, то время всё равно идет, люди приходят и так далее
                if (IsChecking)
                {
                    IsCheckingTime -= deltaT; // Отнимается время одной итерации цикла 
                }
                IsCheckingFunction();

                if ((QueueIn > 0 || QueueOut > 0) && IsChecking == false) // В какой-то из очередей есть люди и охранник готов их проверять
                {
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
                    // Если человек слишком много, прямо перебор, то все люди на выход выходят без проверки
                    if (QueueOut > N)
                    {
                        QueueOut = 0;
                    }
                    else // Если человек на выход нормально
                    {
                        // Тогда охранник выпускает нужное количество людей из очереди на выход, не проверяя их, с запасом на 1 человека
                        int Kicked = Math.Abs(N - QueueIn - QueueOut) + 1;
                        if (Kicked < QueueOut)
                        {
                            QueueOut -= Kicked;
                            NoCheckingPeoples += Kicked;
                        }
                        else
                        {
                            NoCheckingPeoples += QueueOut;
                            QueueOut = 0;
                        }
                    }
                    
                }

                Protocol[0] = Time.ToString(); // Общее модельное время
                Protocol[1] = QueueIn.ToString(); // Количество людей в очереди на ВХОД (В текущий момент времени)
                Protocol[2] = QueueOut.ToString(); // Количество людей в очереди на ВЫХОД (аналогично)
                Protocol[3] = FacesCount.ToString(); // Количество обработанных людей
                Protocol[4] = NoDocs.ToString(); // Количество людей без документов
                Protocol[5] = WithDrugs.ToString(); // С грузом
                Protocol[6] = PunishedPeoples.ToString(); // Наказано нарушителей
                Protocol[7] = NoCheckingPeoples.ToString(); // Выгнали из-за переполнения буфера
                Protocol[8] = WhatIsDoingSecurity; // Чем в данный момент времени занят охранник
                dataGridView1.Rows.Add(Protocol);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                button1.Text = "Стоп";
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
    }
}