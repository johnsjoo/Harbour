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
     



        public static int KnotsToKmPerHour(double number)
        {
                 
            double result = number * 1.85200;
            return (int)result;

            
        }
        
        

    }
    
}
