using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalmanFilter_Demo
{
    class KalmanFilter
    {
        private double R, Q;
        private double currentX, nextX;
        private double currentP, nextP;
        private double K;
        private double rootR;
        private double d, d_, currentsample;
        private double alpha = 0.8;

        public KalmanFilter(double x0, double R0, double Q0)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            currentX = x0;
            currentsample = x0;
            nextX = x0;
            R = R0;
            Q = Q0;
            currentP = 1.0;
            d = 0;
            d_ = 0;
            rootR = Math.Sqrt(R);
        }
        public double Next(double sample)
        {
            currentX = nextX;
            currentP = nextP + Q;
            d_ = d;

            K = currentP / (currentP + R);
            d = d_ + K * (sample - currentsample - d_);
            nextX = currentX + K * (sample - currentX) + alpha * d;
            nextP = (1 - K) * currentP;

            currentsample = sample;

            if (nextX > 1000)
            {
                int j = 1;
            }

            return nextX;
        }
    }
}
