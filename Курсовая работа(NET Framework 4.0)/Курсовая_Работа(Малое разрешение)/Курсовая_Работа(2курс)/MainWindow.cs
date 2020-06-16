using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Курсовая_Работа_2курс_
{                                                                                                                                   // Поле РЕЗУЛЬТАТЫ это поле под надписью "Результаты"
    public partial class MainWindow : Form                                                                                          // в самом оконном приложении
    {                                                                                                                               //
        private Point lastPoint;                                                                                                    //
        public MainWindow()                                                                                                         //
        {                                                                                                                           //
            InitializeComponent();                                                                                                  //
        }                                                                                                                           //
                                                                                                                                    //
        private void MainWindow_Load(object sender, EventArgs e)                                                                    //
        {                                                                                                                           //
            HeightDepend.Series[0].Points.AddY(5);                                                                                  // Задание тестовых данных по умолчанию
            TrajectGraph.Series[0].Points.AddY(5);                                                                                  //
            density_comboBox.SelectedIndex = 0;                                                                                     //
            h1Field.Text = "0.2";                                                                                                   //
            h2Field.Text = "0.1";                                                                                                   //
            d1Field.Text = "0.1";                                                                                                   //
            d2Field.Text = "0.01";                                                                                                  //
            MassPistonField.Text = "0";                                                                                             //
            Gravity_comboBox.SelectedIndex = 0;                                                                                     //
        }                                                                                                                           //
                                                                                                                                    //
        private void PlotTrajectGraph(double p, double h1, double h2, double d1, double d2, double M, double g)                     //
        {                                                                                                                           //
            TrajectGraph.Series[0].Points.Clear();                                                                                  //
            TrajectGraph.ChartAreas[0].AxisX.Minimum = 0;                                                                           //
            TrajectGraph.ChartAreas[0].AxisY.Minimum = 0;                                                                           //
            double t_fall;                                                                                                          // Время падения жидкости от h2 до поверхности
            double x;                                                                                                               //
            double y;                                                                                                               //
            double h;                                                                                                               //
            double s1;                                                                                                              //
            double s2;                                                                                                              //
            double F;                                                                                                               //
                                                                                                                                    //
            try                                                                                                                     //
            {                                                                                                                       //
                s1 = (Math.PI * d1 * d1) / 4;                                                                                       // Просчет площади сечений сосуда
                s2 = (Math.PI * d2 * d2) / 4;                                                                                       // и отверстия в стенке сосуда
                                                                                                                                    //
                                                                                                                                    //
                if (h1 == h2)                                                                                                       //
                {                                                                                                                   //
                    throw new Exception("h1 = h2");                                                                                 //
                }                                                                                                                   //
                else if (h2 > h1)                                                                                                   //
                {                                                                                                                   //
                    throw new Exception("h1 не может быть меньше чем h2!");                                                         //
                }                                                                                                                   //
                                                                                                                                    //
                F = p * (2 * g * h2 + ((2 * M * g) / (p * s1))) * s2;                                                               // Просчет реактивной силы
                F_res.Text = Math.Round(F, 6).ToString();                                                                           // Передача значения реактивной силы в поле РЕЗУЛЬТАТЫ
                                                                                                                                    //
                h = h1 - h2;                                                                                                        //
                                                                                                                                    //
                t_fall = Math.Sqrt((2 * h2) / g);                                                                                   //
                                                                                                                                    //
                x = Math.Sqrt((2 * g * h1) + (2 * M * g) / (p * s1)) * t_fall;                                                      // Вычисление максимального расстояни падения жидкости
                Xmax.Text = Math.Round(x, 6).ToString();                                                                            // от сосуда
                                                                                                                                    //
                y = h2;                                                                                                             //
                                                                                                                                    //
                TrajectGraph.ChartAreas[0].AxisY.Maximum = h1;                                                                      // Высота графика всегда задается равной уровню жидкости в сосуде
                double xTemp = 0;                                                                                                   //
                double tTemp = 0;                                                                                                   //
                                                                                                                                    //
                while (xTemp < x && tTemp < t_fall)                                                                                 //
                {                                                                                                                   //
                    TrajectGraph.Series[0].Points.AddXY(xTemp, y);                                                                  //
                    xTemp += x / 10;                                                                                                // В цикле вычислются координаты в момент времени t
                    tTemp += t_fall / 10;                                                                                           // с шагом t/10
                    y = h2 - ((g * (tTemp * tTemp)) / 2);                                                                           //
                    if (xTemp >= x)                                                                                                 //
                    {                                                                                                               //
                        TrajectGraph.Series[0].Points.AddXY(x, 0);                                                                  //
                    }                                                                                                               //
                }                                                                                                                    //
                TrajectGraph.Series[0].Points.AddXY(x, 0);                                                                          //
            }                                                                                                                       //
            catch (Exception ex)                                                                                                    //
            {                                                                                                                       //
                MessageBox.Show(ex.Message);                                                                                        //
            }                                                                                                                       //
        }                                                                                                                           //
                                                                                                                                    //
        private void PlotHeightGraph(double p, double h1, double h2, double d1, double d2, double M, double g)                      //
        {                                                                                                                           //
            HeightDepend.Series[0].Points.Clear();                                                                                  // 
            HeightDepend.ChartAreas[0].AxisX.Minimum = 0;                                                                           //
            HeightDepend.ChartAreas[0].AxisY.Minimum = 0;                                                                           //
            double x, y, h, s1, s2;                                                                                                 //
            double delta_t = 0.1;                                                                                                   //
            double t = 0;                                                                                                           //
            double t_max = 0;                                                                                                       //
            double temp1, temp2, temp3;                                                                                             //
            try                                                                                                                     //
            {                                                                                                                       //
                s1 = (Math.PI * d1 * d1) / 4;                                                                                       // Просчет площади сечений сосуда
                s2 = (Math.PI * d2 * d2) / 4;                                                                                       // и отверстия в стенке сосуда
                                                                                                                                    //
                t_max = (Math.Sqrt(h1 + M / (p * s1)) - Math.Sqrt(h2 + M / (p * s1))) * s1 / (s2 * Math.Sqrt(g / 2));               // Подсчет времени выливания жидкости до h2
                TimeOfEmpty.Text = Math.Round(t_max, 3).ToString();                                                                 // Вывод значения времени в поле РЕЗУЛЬТАТЫ
                delta_t = t_max / 10;                                                                                               // Определение отрезка времени для итерациий в цикле
                                                                                                                                    //
                if (h1 == h2)                                                                                                       //
                {                                                                                                                   //
                    throw new Exception("h1 = h2");                                                                                 //
                }                                                                                                                   //
                else if (h2 > h1) { throw new Exception("h1 не может быть меньше чем h2!"); }                                       //
                                                                                                                                    //
                                                                                                                                    //
                if (g > 0 && d1 > d2 && d2 <= d1 / 10 && d2 > d1 / 100)                                                             //
                {                                                                                                                   //
                    temp3 = M / (p * s1);                                                                                           // В данных полях
                    temp1 = Math.Sqrt(h1 + temp3);                                                                                  // записана формула координаты y
                    temp2 = (s2 / s1) * Math.Sqrt(g / 2) * t;                                                                       // по частям в переменные temp1, temp2, temp3
                    y = Math.Pow(temp1 - temp2, 2) - temp3;                                                                         //
                    HeightDepend.Series[0].Points.AddXY(0d, y);                                                                     //
                    t += delta_t;                                                                                                   //
                    while (y > (h2 + 0.0001))                                                                                       //
                    {                                                                                                               //
                        y = Math.Pow((temp1 - (s2 / s1) * Math.Sqrt(g / 2) * t), 2) - temp3;                                        // Просчет координаты y в момент времени t
                        HeightDepend.Series[0].Points.AddXY(t, y);                                                                  // Отрисовка координа ты на графике
                        t += delta_t;                                                                                               //
                    }                                                                                                               //
                }                                                                                                                   //
                else                                                                                                                //
                {                                                                                                                   //
                    HeightDepend.Series[0].Points.AddY(h1);                                                                         //
                    throw new Exception("d1/10 < d2 < d1/100" +                                                                     //
                        "\ng должно быть больше нуля, т.к при g = 0 " +                                                             //
                        "\nне будет корректных значений.");                                                                         //
                }                                                                                                                   //
            }                                                                                                                       //
            catch (Exception ex)                                                                                                    //
            {                                                                                                                       //
                MessageBox.Show(ex.Message);                                                                                        //
            }                                                                                                                       //
        }                                                                                                                           //
                                                                                                                                    //
                                                                                                                                    //
        private void CloseButton_Click(object sender, EventArgs e)                                                                  //
        {                                                                                                                           //
            Application.Exit();                                                                                                     //
        }                                                                                                                           //
                                                                                                                                    //
        private void CloseButton_MouseEnter(object sender, EventArgs e)                                                             //
        {                                                                                                                           //
            CloseButton.BackColor = Color.Firebrick;                                                                                //
        }                                                                                                                           //
                                                                                                                                    //
        private void CloseButton_MouseLeave(object sender, EventArgs e)                                                             //
        {                                                                                                                           //
            CloseButton.BackColor = Color.Transparent;                                                                              //
        }                                                                                                                           //
                                                                                                                                    //
                                                                                                                                    //
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)                                                          //
        {                                                                                                                           //
            WindowMoving(e);                                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void MainWindow_MouseDown(object sender, MouseEventArgs e)                                                          //
        {                                                                                                                           //
            lastPoint = new Point(e.X, e.Y);                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void WindowMoving(MouseEventArgs e)                                                                                 // Метод отвечающий за перемещение окна.
        {                                                                                                                           //
            if (e.Button == MouseButtons.Left)                                                                                      //
            {                                                                                                                       //
                this.Left += e.X - lastPoint.X;                                                                                     //
                this.Top += e.Y - lastPoint.Y;                                                                                      //
            }                                                                                                                       //
        }                                                                                                                           //
                                                                                                                                    //
        private void panel2_MouseDown(object sender, MouseEventArgs e)                                                              //
        {                                                                                                                           //
            lastPoint = new Point(e.X, e.Y);                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void panel2_MouseMove(object sender, MouseEventArgs e)                                                              //
        {                                                                                                                           //
            WindowMoving(e);                                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void panel3_MouseDown(object sender, MouseEventArgs e)                                                              //
        {                                                                                                                           //
            lastPoint = new Point(e.X, e.Y);                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void panel3_MouseMove(object sender, MouseEventArgs e)                                                              //
        {                                                                                                                           //
            WindowMoving(e);                                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void panel1_MouseDown(object sender, MouseEventArgs e)                                                              //
        {                                                                                                                           //
            lastPoint = new Point(e.X, e.Y);                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void panel1_MouseMove(object sender, MouseEventArgs e)                                                              //
        {                                                                                                                           //
            WindowMoving(e);                                                                                                        //
        }                                                                                                                           //
                                                                                                                                    //
        private void Plot_button_Click(object sender, EventArgs e)                                                                  // При нажатии кнопки происходит
        {                                                                                                                           // считывание данных с полей ввода, перевод в double.
            double h1 = 0, h2 = 0, d1 = 0, d2 = 0, M = 0;                                                                           // 
            List<double> g = new List<double> { 9.81, 1.62, 8.88, 3.86 };                                                           //
            List<double> p = new List<double> { 1000, 806, 1200 };                                                                  //
            char[] sep = { ' ', '\r', '\n' };                                                                                       //
            try                                                                                                                     //
            {                                                                                                                       //
                if (h1Field.Text == "0" || h1Field.Text == "" || h2Field.Text == "0" || h2Field.Text == "" ||
                    d1Field.Text == "0" || d1Field.Text == "" || d2Field.Text == "0" || d2Field.Text == "")                       
                {                                                                                                                   //
                    throw new Exception("Нулевые значения");                                                                        //
                }                                                                                                                   //
                string[] temp;                                                                                                      //
                                                                                                                                    //
                temp = h1Field.Text.Replace(".", ",").Split(sep, System.StringSplitOptions.RemoveEmptyEntries);                     //
                Double.TryParse(temp[0], out h1);                                                                                   //
                h1_NearBottleLabel.Text = temp[0];                                                                                  // Передача значения h1 для отображения на схематическом рисунке.
                h1_res.Text = temp[0];                                                                                              // Передача значения h1 для отображения в поле РЕЗУЛЬТАТЫ
                                                                                                                                    //
                temp = h2Field.Text.Replace(".", ",").Split(sep, System.StringSplitOptions.RemoveEmptyEntries);                     //
                Double.TryParse(temp[0], out h2);                                                                                   //
                h2_res.Text = temp[0];                                                                                              // Передача значения h2 для отображения в поле РЕЗУЛЬТАТЫ
                                                                                                                                    //
                temp = d1Field.Text.Replace(".", ",").Split(sep, System.StringSplitOptions.RemoveEmptyEntries);                     //
                Double.TryParse(temp[0], out d1);                                                                                   //
                d1_NearBottleLabel.Text = temp[0];                                                                                  // Передача значения d1 для отображения на схематическом рисунке
                d1_res.Text = temp[0];                                                                                              // Передача значения d1 для отображения в поле РЕЗУЛЬТАТЫ
                                                                                                                                    //
                temp = d2Field.Text.Replace(".", ",").Split(sep, System.StringSplitOptions.RemoveEmptyEntries);                     //
                Double.TryParse(temp[0], out d2);                                                                                   //
                d2_res.Text = temp[0];                                                                                              // Передача значения d2 для отображения в поле РЕЗУЛЬТАТЫ
                                                                                                                                    //
                temp = MassPistonField.Text.Replace(".", ",").Split(sep, System.StringSplitOptions.RemoveEmptyEntries);             //
                if (MassPistonField.Text == "")                                                                                     //
                {                                                                                                                   //
                    MassPistonRes.Text = "0";
                    MassPistonField.Text = "0";
                    M = 0;                                                                                                          //
                }                                                                                                                   //
                else                                                                                                                //
                {                                                                                                                   //
                    Double.TryParse(temp[0], out M);                                                                                //
                    MassPistonRes.Text = temp[0];                                                                                   //
                }                                                                                                                   //
                                                                                                                                    //
                int index = Gravity_comboBox.SelectedIndex;                                                                         //
                int index2 = density_comboBox.SelectedIndex;                                                                        //
                if (index == -1)                                                                                                    //
                {                                                                                                                   //
                    index = 0;                                                                                                      //
                    Gravity_comboBox.SelectedIndex = index;                                                                         //
                }                                                                                                                   //
                if (index2 == -1)                                                                                                   //
                {                                                                                                                   //
                    index2 = 0;                                                                                                     //
                    density_comboBox.SelectedIndex = index2;                                                                        //
                }                                                                                                                   //
                                                                                                                                    //
                PlotTrajectGraph(p[index2], h1, h2, d1, d2, M, g[index]);                                                           // Вызов функции отрисовки траектории.
                PlotHeightGraph(p[index2], h1, h2, d1, d2, M, g[index]);                                                            // Вызов функции отрисовки графика зависимости высоты от времени.
            }                                                                                                                       //
            catch (Exception ex)                                                                                                    //
            {                                                                                                                       //
                MessageBox.Show("Ошибка ввода данных!" + "\n" + ex.Message);                                                        //
            }                                                                                                                       //
        }                                                                                                                           //
    }                                                                                                                               //
}
