using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;


namespace Harbour
{

    


    class Boat
    {
        static Random rnd = new Random();

        public string BoatID { get; set; }
        public string BoatType { get; set; }
        public int Weight { get; set; }
        public int TopSpeed { get; set; }
        public int BoatNumber { get; set; }



        public static double KnotsToKmPerHour(double number)
        {
            double result = number * 1.85200;
            return result;
        }
        //public static void GetWheight(int firstNumber, int secondNumber)
        //{
        //    Console.WriteLine("vikt: " + rnd.Next(firstNumber,secondNumber));
            
        //}
        

    }
    
}
