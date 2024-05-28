using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_3
{
    class RungeKuttaMethod
    {
        public delegate double Mathfunction(double x);
        public double RungeKutta(mathfunction x,double x)
        {
            int n = (int)((x - x0) / h);

            double k1, k2, k3, k4;

            double y = y0;

            for (int i = 1; i <= n; i++)
            {

                // Apply Runge Kutta Formulas
                // to find next value of y
                k1 = h * (dYdt(x0, y));

                k2 = h * (dYdt(x0 + 0.5 * h, y + 0.5 * k1));

                k3 = h * (dYdt(x0 + 0.5 * h, y + 0.5 * k2));

                k4 = h * (dYdt(x0 + h, y + k3));

                // Update next value of y
                y = y + (1.0 / 6.0) * (k1 + 2* k2 + 2 * k3 + k4);

                // Update next value of x
                x0 = x0 + h;
            }

            return y;
        }

    }
}
