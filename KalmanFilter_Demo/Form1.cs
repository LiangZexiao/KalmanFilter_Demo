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
                //这样声明的话，如果多次调用rnd则会会出现每次的随机数值都一样
                //Random Random1 = new Random();
                //利用系统日期作为参数传进去之后可以解决上面出现的问题
                Random Random1 = new Random((int)DateTime.Now.Ticks); 
                double i1 = Random1.Next(20, 30);
                //随机生成-0.90至0.99的随机双精度数值
                double result = Random1.NextDouble() * (-0.99) + 0.99;
                ObsRand[i] = i1 + result;
                //用随机数列模拟观察值，存到控件中显示
                this.richTextBox1.Text += ObsRand[i].ToString() + "\n";
            }
            //表格初始化
            chart1.Series.Clear();
            graphPoint(chart1, "测量值", ObsRand, Color.PowderBlue);
            //设置纵坐标
            chart1.ChartAreas[0].AxisY.Minimum = 20;
            chart1.ChartAreas[0].AxisY.Maximum = 30;
            //调用卡尔曼滤波器算法
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
            //chart控件中新建一个图表
            Series s = new Series(name);
            s.ChartType = SeriesChartType.Line;
            //根据点画线
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
            //根据点画点
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
            //导入参数
            double KamanX = CanShu[0];
            double KamanP = CanShu[1];
            double KamanQ = CanShu[2];
            double KamanR = CanShu[3];
            double KamanY = CanShu[4];
            double KamanKg = CanShu[5];
            double KamanSum = CanShu[6];

            //加载观察值
            double[] True = new double[ObsRand.Length];
            for (int i = 0; i <= ObsRand.Length - 1; i++)
            {
                //对每个观察值迭代
                KamanY = KamanX;
                KamanP = KamanP + KamanQ;
                KamanKg = KamanP / (KamanP + KamanR);
                KamanX = (KamanY + KamanKg * (Observe[i] - KamanY));
                KamanSum += KamanX;
                True[i] = KamanX;
                //将滤波后的结果输出到控件中
                this.richTextBox2.Text += KamanX.ToString() + "\n";
                KamanP = (1 - KamanKg) * KamanP;
            }
            Average = KamanSum / Observe.Length;
            return True;

        }
    }
}
