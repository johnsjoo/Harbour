using System;
using System.Collections.Generic;
using System.Text;

namespace Harbour
{
    class CargoShip : Boat
    {
       

        public int NumberOfContainers { get; set; }
        public int CargoDays { get; set; }



        public static int RandomContainers() 
        {
            Random rnd = new Random();
            int random = rnd.Next(0,500);
            return random;

        }
    }
}
