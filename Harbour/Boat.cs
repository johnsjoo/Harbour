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
        public double TopSpeed { get; set; }
        public int SlotNumber { get; set; }
        public double BoatSize { get; set; }
        public int DaysLeft { get; set; }
        public int uniqueProp { get; set; }


        public static int KnotsToKmPerHour(int knots)
        {        
            double result = knots * 1.85200;
            return (int)result;
        }
        public static int FotToMeter(int fot)
        {
            double value = fot * 0.3840;
            return (int)value;
        }
        public static string GenerateBoatId(string boatID)
        {
            //Metod som returnerar 3 unika bokstäver till vårt Boat-id.
            for (int i = 0; i < 3; i++)
            {
                char randomChar = (char)rnd.Next('A', 'Z');
                boatID += randomChar;
            }
            return boatID;
        }
    }
}
