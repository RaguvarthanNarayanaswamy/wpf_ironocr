using System;
using System.Windows;

namespace IronOCR.Service
{
    class PageHandler
    {
        public static Tuple<double, double> GetScreenSize()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeigth = SystemParameters.PrimaryScreenHeight;
            return Tuple.Create(screenWidth, screenHeigth);

        } 
    }
}
