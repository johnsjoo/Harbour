﻿using System;
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
           /* while (true)
            {
                
                //Thread.Sleep(5000);
            }*/


            List<Boat> arrivalboats = new List<Boat>();
            Boat[] harBour = new Boat[64];
            //Skapa fem båtar.
            DailyArrivalBoats(arrivalboats);
            //kollar om plats finns i hamnen
            SlotCheckInHarbour(harBour, arrivalboats);
            //Skriver  ut de 5 båtarna som kommer in varje dag.
            PrintingBoat(arrivalboats);
            //Lägger till en dag varje varv i loopen.
            

            

        }

        private static void SlotCheckInHarbour(Boat[] harBour, List<Boat> arrivalboats)
        {
            int nearBySpaces = 0;
            List<Boat> SortedBySize = arrivalboats.OrderBy(o => o.BoatSize).ToList();

            for (int i = 0; i <= harBour.Length; i++)
            {

                if (harBour[i] == null)
                {
                    freeSpaceCounter++;
                    nearBySpaces++;
                }
                else
                {
                    Boat currentBoat = SortedBySize.First();
                    if (nearBySpaces >= 4 && currentBoat.BoatSize >=4)
                    {
                        harBour[i] = currentBoat;
                        SortedBySize.RemoveAt(0);
                    }
                    else if (nearBySpaces >= 2 && currentBoat.BoatSize >= 2)
                    {
                        harBour[i] = currentBoat;
                        SortedBySize.RemoveAt(0);
                    }
                    else if (nearBySpaces >= 1 && currentBoat.BoatSize >= 1)
                    {
                        harBour[i] = currentBoat;
                        SortedBySize.RemoveAt(0);
                    }

                    nearBySpaces = 0;
                }
            }
        }

        private static void DailyArrivalBoats(List<Boat> arrivalboats)
        {
            for (int i = 0; i < 5; i++)
            {

                int randomNumber = rnd.Next(1, 4);
                if (randomNumber == 1)
                {
                    Rowboat r = new Rowboat();
                    
                    string boatID = GenerateBoatId("R-");
                    int topSpeeed = Boat.KnotsToKmPerHour(rnd.Next(1, 3));
                    r.BoatID = boatID;
                    r.Weight = rnd.Next(100, 300);
                    r.BoatType = "Roddbåt";
                    r.TopSpeed = topSpeeed;
                    r.MaxNumberOfPassangers = rnd.Next(1, 6);
                    r.RowBoatDay = 1;
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
                    m.MotorBoatDay = 3;
                    m.BoatSize = 1;

                    arrivalboats.Add(m);
                }
                else if (randomNumber == 3)
                {
                    SailBoat s = new SailBoat();
                    
                    string boatID = GenerateBoatId("S-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 12));
                    s.BoatID = boatID;
                    s.Weight = rnd.Next(800, 6000);
                    s.BoatType = "Segelbåt";
                    s.TopSpeed = topSpeed;
                    s.SailBoatsLength = rnd.Next(10, 60);
                    s.SailBoatDays = 4;
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
                    c.CargoDays = 6;
                    c.BoatSize = 4;

                    arrivalboats.Add(c);
                }

            }
        }

        private static void PrintingBoat(List<Boat> arrivalboats)
        {
            Console.WriteLine($"Plats \t Båttyp \t ID \t Vikt \t MaxHastighet \t Unika egenskaper");
            Console.WriteLine("-------------------------------------------------------------------------");

            foreach (Boat boat in arrivalboats)
            {
                if (boat is Rowboat)
                {
                    Console.WriteLine($"\t {boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((Rowboat)boat).MaxNumberOfPassangers)}\tpassagerare");
                }
                else if (boat is MotorBoat)
                {
                    Console.WriteLine($"\t {boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((MotorBoat)boat).HorsePower)}\thästkrafter");
                }
                else if (boat is SailBoat)
                {
                    Console.WriteLine($"\t{boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((SailBoat)boat).SailBoatsLength)} \t m");
                }
                else if (boat is CargoShip)
                {
                    Console.WriteLine($"\t{boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {(((CargoShip)boat).NumberOfContainers)}\tContainrar");
                }
            }

            //summering av data


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
