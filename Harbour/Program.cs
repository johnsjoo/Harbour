using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace Harbour
{
    class Program
    {
        
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            List<Boat> arrivalboats = new List<Boat>();
            Boat[] harBour = new Boat[64];
            //Skapa fem båtar.
            for (int i = 0; i < 5; i++)
            {

                int randomNumber = rnd.Next(1, 4);
                if (randomNumber == 1)
                {
                    Rowboat r = new Rowboat();
                    harBour[i] = r;
                    string boatID = GenerateBoatId("R-");
                    double topSpeeed = Boat.KnotsToKmPerHour(rnd.Next(0,3));
                    r.BoatID = boatID;
                    r.Weight = rnd.Next(100,300);
                    r.BoatType = "Roddbåt";
                    r.TopSpeed = topSpeeed;
                    r.MaxNumberOfPassangers = rnd.Next(1,6);

                    arrivalboats.Add(r);
                    
                }
                else if (randomNumber == 2)
                {
                    MotorBoat m = new MotorBoat();
                    harBour[i] = m;
                    string boatID = GenerateBoatId("M-");
                    double topSpeed = Boat.KnotsToKmPerHour(rnd.Next(0,60));
                    m.BoatID = boatID;
                    m.Weight = rnd.Next(200,3000);
                    m.BoatType = "Motorbåt";
                    m.TopSpeed = topSpeed;
                    m.HorsePower = rnd.Next(10,10000);

                    arrivalboats.Add(m);
                }
                else if (randomNumber == 3)
                {
                    SailBoat s = new SailBoat();
                    harBour[i] = s;
                    string boatID = GenerateBoatId("S-");
                    double topSpeed = Boat.KnotsToKmPerHour(rnd.Next(0,12));
                    s.BoatID = boatID;
                    s.Weight = rnd.Next(800,6000);
                    s.BoatType = "Segelbåt";
                    s.TopSpeed = topSpeed;
                    s.SailBoatsLength = rnd.Next(10,60);

                    arrivalboats.Add(s);
                }
                else if (randomNumber == 4)
                {
                    CargoShip c = new CargoShip();
                    harBour[i] = c;
                    string boatID = GenerateBoatId("L-");
                    double topSpeed = Boat.KnotsToKmPerHour(rnd.Next(0,20));
                    c.BoatID = boatID;
                    c.Weight = rnd.Next(3000,20000);
                    c.BoatType = "Lastfartyg";
                    c.TopSpeed = topSpeed;
                    c.NumberOfContainers = rnd.Next(0,500);

                    arrivalboats.Add(c);
                }

            }

            foreach (Boat boat in arrivalboats)
            {
                if (boat.BoatType == "Roddbåt")
                {
                    Console.WriteLine($"En {boat.BoatType} anlände");
                }
                else if (boat.BoatType == "Motorbåt")
                {
                    Console.WriteLine($"En {boat.BoatType} anlände");
                }
                else if (boat.BoatType == "Segelbåt")
                {
                    Console.WriteLine($"En {boat.BoatType} anlände");
                }
                else if (boat.BoatType == "Lastfartyg")
                {
                    Console.WriteLine($"En {boat.BoatType} anlände");
                }
            }
            


            

        }

        private static string GenerateBoatId(string boatID)
        {
            for (int i = 0; i < 3; i++)
            {

                char randomChar = (char)rnd.Next('a', 'z');
                boatID += rnd;
 
            }
            return boatID;
        }
    }
    
}
