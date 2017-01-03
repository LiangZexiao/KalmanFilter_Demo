using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace KalmanFilter_Demo
{
    public partial class Form1 : Form
    {
        double[] CanShu = { 23, 9, 16, 16, 1, 0, 0, 0 };

        //double[] Observ = { 22, 24, 24, 25, 24, 26, 21, 26, };

        //double[] Observ = { 25, 26 };

        double[] ObsRand = new double[100];

        public Form1()
        {
            InitializeComponent();
        }

        private void 产生随机数列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //产生随机数列
            for (int i = 0; i < 100; i++)
            {
                //Random Random1 = new Random();//这样声明的话，如果多次调用rnd则会会出现每次的随机数值都一样
                Random Random1 = new Random((int)DateTime.Now.Ticks);   //利用系统日期作为参数传进去之后可以解决上面出现的问题
                double i1 = Random1.Next(20, 30);
                double result = Random1.NextDouble() * (-0.99) + 0.99;//随机生成-0.90至0.99的随机双精度数值
                ObsRand[i] = i1 + result;
                this.richTextBox2.Text += ObsRand[i].ToString() + "\n";
            }
            chart1.Series.Clear();
            graphPoint(chart1, "测量值", ObsRand, Color.PowderBlue);
            double[] True = GetKalMan(CanShu, ObsRand);

            graph(chart1, "滤波后值", True, Color.Black);

            double[] Ave = new double[ObsRand.Length];

            for (int i = 0; i < ObsRand.Length; i++)
            {
                Ave[i] = Average;
            }

            graph(chart1, "平均值", Ave, Color.Red);
        }

        //画图函数
        protected void graph(Chart c, string name, double[] vals, Color clr)
        {
            Series s = new Series(name);
            s.ChartType = SeriesChartType.Line;
            for (int i = 0; i < vals.Length; i++)
            {
                s.Points.Add(new DataPoint(i, vals[i]));
            }
            s.Color = clr;
            c.Series.Add(s);
        }

        //画图函数2
        protected void graphPoint(Chart c, string name, double[] vals, Color clr)
        {
            Series s = new Series(name);
            s.ChartType = SeriesChartType.Point;
            for (int i = 0; i < vals.Length; i++)
            {
                s.Points.Add(new DataPoint(i, vals[i]));
            }
            s.Color = clr;
            c.Series.Add(s);
        }
        //滤波函数

        double Average;
        public double[] GetKalMan(double[] CanShu, double[] Observe)
        {
            double KamanX = CanShu[0];
            double KamanP = CanShu[1];
            double KamanQ = CanShu[2];
            double KamanR = CanShu[3];
            double KamanY = CanShu[4];
            double KamanKg = CanShu[5];
            double KamanSum = CanShu[6];
            double[] True = new double[ObsRand.Length];
            for (int i = 0; i <= ObsRand.Length - 1; i++)
            {
                KamanY = KamanX;
                KamanP = KamanP + KamanQ;
                KamanKg = KamanP / (KamanP + KamanR);
                KamanX = (KamanY + KamanKg * (Observe[i] - KamanY));
                KamanSum += KamanX;
                True[i] = KamanX;
                this.richTextBox1.Text += KamanX.ToString() + "\n";
                KamanP = (1 - KamanKg) * KamanP;
            }
            Average = KamanSum / Observe.Length;
            return True;

        }
    }
}
