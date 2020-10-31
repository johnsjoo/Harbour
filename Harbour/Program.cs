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
        static int freeSpaceCounter = 0;
        static int harbourDay = 0;
        static Random rnd = new Random();

        static void Main(string[] args)
        {

            List<Boat> arrivalboats = new List<Boat>();
            //Array med listor i. 
            List<Boat>[] harBour = new List <Boat> [64];
            ListsInArray(harBour);
            while (true)
            {
                harbourDay++;
                Console.WriteLine("DAG:" + harbourDay);
                //Funktion för att kolla hur många dagar båtarna har kvar i hamnen.
                DaysLeftInHarbour(harBour);

                //skapa fem båtatr
                DailyArrivalBoats(arrivalboats);

                //läggg till båtar
                SlotCheckInHarbour(harBour, arrivalboats);

                //printa båtar
                PrintingBoat(arrivalboats, harBour);

                Thread.Sleep(5000);
            }
  


        }

        private static void ListsInArray(List<Boat>[] harBour)
        {
            for (int i = 0; i < harBour.Length; i++)
            {
                harBour[i] = new List<Boat>();
            }
        }

        public static void SlotCheckInHarbour(List<Boat>[] harbour, List<Boat> arrivalboats)
        {
            //sorterar listan med båtar med störst båt först
            List<Boat> SortedBoats = arrivalboats.OrderByDescending(b => b.BoatSize).ToList();

            //för varje båt i listan, försök att placera den i hamnen, börjar med största båten först
            foreach (Boat b in SortedBoats)
            {

                //metoden returnerar en bool om den fick lägga till eller inte
                if (PlaceBoatInHarbour(harbour, b))
                {
                    //boat is in harbour
                }
                else
                {
                    //Avvisa båt
                    Console.WriteLine("Avvisad båt" + b.BoatID + "\t" + b.BoatType);
                    
                }
            }

        }

        //returnerar true om båten är fick lägga till i hamnen, false om den blev avvisad
        public static bool PlaceBoatInHarbour(List<Boat>[] harbour, Boat currentBoat)
        {
            //loopa igenom alla platser i hamnen
            for (int i = 0; i < harbour.Length; i++)
            {
                if (harbour[i].Count == 1)
                {
                    if (harbour[i].First() is Rowboat && currentBoat is Rowboat)
                    {
                        harbour[i].Add(currentBoat);
                    }

                }

                //Om platsen är tom och edge-case om listan tar slut
                if (harbour[i].Count == 0 && currentBoat.BoatSize + i < harbour.Length)
                {

                    int startIndex = i;
                    //Närliggande lediga platser
                    int numOfAdjacent = 0;
                    //Kollar om de nästkommande platserna också är tomma, t.o.m. båtens storlek
                    for (int j = startIndex; j < startIndex + currentBoat.BoatSize; j++)
                    {
                        //om dom är det adderar vi
                        if (harbour[j].Count == 0 )
                        {
                            numOfAdjacent++;
                        }
                    }
                    //Om alla nästkommande platser tom båtens storlek är tomma kan får båten plats, så vi lägger till.
                    if (numOfAdjacent == currentBoat.BoatSize)
                    {

                        //samma loop som förut, men nu vet vi att alla platser är tomma så då lägger vi till båten på dessa platser/index. 
                        for (int j = startIndex; j < startIndex + currentBoat.BoatSize; j++)
                        {
                            harbour[j].Add(currentBoat);
                        }
                        //Vi kunde lägga till båten --> true
                        return true;
                    }


                }

            }
            //fall vi kör igenom hela for-loopen och inget händer får vi avvisa båten
            return false;


        }

        private static void DailyArrivalBoats(List<Boat> arrivalboats)
        {
            arrivalboats.Clear();
            for (int i = 0; i < 15; i++)
            {
                
                int randomNumber = rnd.Next(1, 5);
                if (randomNumber == 1)
                {
                    Rowboat r = new Rowboat();

                    string boatID = GenerateBoatId("R-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 3));
                    r.BoatID = boatID;
                    r.Weight = rnd.Next(100, 300);
                    r.BoatType = "Roddbåt";
                    r.TopSpeed = topSpeed;
                    r.MaxNumberOfPassangers = rnd.Next(1, 6);
                    r.DaysLeft = 1;
                    r.BoatSize = 0.5;

                    arrivalboats.Add(r);


                }
                else if (randomNumber == 2)
                {
                    MotorBoat m = new MotorBoat();
                    
                    string boatID = GenerateBoatId("M-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 60));
                    m.BoatID = boatID;
                    m.Weight = rnd.Next(200, 3000);
                    m.BoatType = "Motorbåt";
                    m.TopSpeed = topSpeed;
                    m.HorsePower = rnd.Next(10, 1000);
                    m.DaysLeft = 3;
                    m.BoatSize = 1;
                    
                    arrivalboats.Add(m);
                }
                else if (randomNumber == 3)
                {
                    SailBoat s = new SailBoat();
                    
                    string boatID = GenerateBoatId("S-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 12));
                    int sailBoatLengt = Boat.FotToMeter(rnd.Next(10, 60));
                    s.BoatID = boatID;
                    s.Weight = rnd.Next(800, 6000);
                    s.BoatType = "Segelbåt";
                    s.TopSpeed = topSpeed;
                    s.SailBoatsLength = sailBoatLengt;
                    s.DaysLeft = 4;
                    s.BoatSize = 2;
                    
                    arrivalboats.Add(s);
                }
                else if (randomNumber == 4)
                {
                    CargoShip c = new CargoShip();
                    
                    string boatID = GenerateBoatId("L-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 20));
                    c.BoatID = boatID;
                    c.Weight = rnd.Next(3000, 20000);
                    c.BoatType = "Lastfartyg";
                    c.TopSpeed = topSpeed;
                    c.NumberOfContainers = rnd.Next(0, 500);
                    c.DaysLeft = 6;
                    c.BoatSize = 4;
                   
                    arrivalboats.Add(c);
                }

            }
        }
        private static void DaysLeftInHarbour(List<Boat>[] harBour)
        {


            for (int i = 0; i < harBour.Length; i++)
            {


                if (harBour[i].Count == 0)
                {
                    continue;
                }
                else
                {
                    if (harBour[i+1].Count == 0 || i == harBour.Length - 1)
                    {
                        harBour[i].First().DaysLeft--;

                    }

                    else if (harBour[i].First().BoatID != harBour[i+1].First().BoatID)
                    {

                        harBour[i].First().DaysLeft--;

                    }


                }
            }
            for (int i = 0; i < harBour.Length; i++)
            {
                if (harBour[i].Count == 0)
                {
                    continue;
                }
                else
                {
                    if (harBour[i].First().DaysLeft == 0)
                    {
                        harBour[i].Clear();
                    }
                }
            }
        }
            
           

        

        private static void PrintingBoat(List<Boat> arrivalboats, List<Boat>[] harBour)
        {



            Console.WriteLine($"Plats \t Båttyp \t ID \t Vikt \t MaxHastighet \t Unika egenskaper");
            Console.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < harBour.Length; i++)
            {
                if (harBour[i].Count == 0)
                {
                    Console.WriteLine($"{i}\t Tomt");
                }
                else
                {
                    foreach (Boat boat in harBour[i])
                    {
                        if (boat is Rowboat)
                        {
                            Console.WriteLine($"{i}\t {boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((Rowboat)boat).MaxNumberOfPassangers)}\tpassagerare");
                        }
                        else if (boat is MotorBoat)
                        {
                            Console.WriteLine($"{i}\t {boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((MotorBoat)boat).HorsePower)}\thästkrafter");
                        }
                        else if (boat is SailBoat)
                        {
                            Console.WriteLine($"{i}\t{boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((SailBoat)boat).SailBoatsLength)} \t m");
                        }
                        else if (boat is CargoShip)
                        {
                            Console.WriteLine($"{i}\t{boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((CargoShip)boat).NumberOfContainers)}\tContainrar");
                        }

                    }
                    
                        
                    
                }




            }
        }
        private static string GenerateBoatId(string boatID)
        {
            for (int i = 0; i < 3; i++)
            {

                char randomChar = (char)rnd.Next('A', 'Z');
                boatID += randomChar;
 
            }
            return boatID;
        }
    }
    
}
