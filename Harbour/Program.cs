using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Transactions;
using System.Xml.Schema;



namespace Harbour
{
    class Program
    {
        static string path = @"C:\Users\John\Desktop\c# Uppgifter\Harbour\Harbour\bin\Debug\netcoreapp3.1\HarbourInformation.txt";
        static int rejectedBoats = 0;
        static int harbourDay = 0;
        static Random rnd = new Random();

        static void Main(string[] args)
        {

            List<Boat> arrivalboats = new List<Boat>();
            //Array med listor i. 
            List<Boat>[] harBour = new List <Boat> [64];
            ListsInArray(harBour);
            Console.WriteLine("Tryck [Enter] för att påbörja en ny dag i hamnen");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Enter:

                        //Kolla skapad fil
                        WriteToFile(harBour, path);
                        //om hittad fil skriv till filen

                        ReadFile(harBour, path, arrivalboats);
                        //annars skapa en fil.

                        harbourDay++;
                        Console.WriteLine("DAG:" + harbourDay);

                        //Funktion för att kolla hur många dagar båtarna har kvar i hamnen.
                        DaysLeftInHarbour(harBour);

                        //skapa fem båtatr
                        DailyArrivalBoats(arrivalboats);

                        //läggg till båtar
                        SlotCheckInHarbour(harBour, arrivalboats);

                        //printa båtar
                        PrintingBoat(harBour);
                        break;  
                }
            }
        }

        private static void ReadFile(List<Boat>[] harBour, string path,List<Boat> arrivalboats)
        {
            //0;Lastfartyg;L-FFD;6135;18;311;
            foreach (var boat in File.ReadLines(path, System.Text.Encoding.UTF8))
            {
                string[] boatLine =  boat.Split(';');
                if (boatLine[1] == "Lastfartyg")
                {
                    CargoShip c = new CargoShip();

                    string boatID = GenerateBoatId("L-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 20));
                    c.BoatID = boatLine[2];
                    c.Weight = int.Parse(boatLine[3]);
                    c.BoatType = "Lastfartyg";
                    c.TopSpeed = int.Parse(boatLine[4]);
                    c.uniqueProp = int.Parse(boatLine[5]);
                    c.DaysLeft = int.Parse(boatLine[6]);
                    c.BoatSize = int.Parse(boatLine[7]);

                    arrivalboats.Add(c);
                }
                else if (boatLine[1] == "Motorbåt")
                {
                    MotorBoat m = new MotorBoat();

                    string boatID = GenerateBoatId("M-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 60));
                    m.BoatID = boatLine[2];
                    m.Weight = int.Parse(boatLine[3]);
                    m.BoatType = "Motorbåt";
                    m.TopSpeed = int.Parse(boatLine[4]);
                    m.uniqueProp = int.Parse(boatLine[5]);
                    m.DaysLeft = int.Parse(boatLine[6]);
                    m.BoatSize = int.Parse(boatLine[7]);

                    arrivalboats.Add(m);

                }
                else if (boatLine[1] == "Roddbåt")
                {
                    Rowboat r = new Rowboat();

                    string boatID = GenerateBoatId("R-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 3));
                    r.BoatID = boatID;
                    r.Weight = rnd.Next(100, 300);
                    r.BoatType = "Roddbåt";
                    r.TopSpeed = topSpeed;
                    r.uniqueProp = rnd.Next(1, 6);
                    r.DaysLeft = 1;
                    r.BoatSize = 1;

                    arrivalboats.Add(r);
                }
                else if (boatLine[1] == "Segelbåt")
                {
                    SailBoat s = new SailBoat();

                    string boatID = GenerateBoatId("S-");
                    int topSpeed = Boat.KnotsToKmPerHour(rnd.Next(1, 12));
                    int sailBoatLengt = Boat.FotToMeter(rnd.Next(10, 60));
                    s.BoatID = boatID;
                    s.Weight = rnd.Next(800, 6000);
                    s.BoatType = "Segelbåt";
                    s.TopSpeed = topSpeed;
                    s.uniqueProp = sailBoatLengt;
                    s.DaysLeft = 4;
                    s.BoatSize = 2;

                    arrivalboats.Add(s);
                }
                
            }
        }

        private static void WriteToFile(List<Boat>[]harBour, string path)
        {
            path = @"C:\Users\John\Desktop\c# Uppgifter\Harbour\Harbour\bin\Debug\netcoreapp3.1\HarbourInformation.txt";
            
            //if (File.Exists(path))
            
                using StreamWriter sw = new StreamWriter(path);
                {

                    for (int i = 0; i < harBour.Length; i++)
                    {

                        if (harBour[i].Count != 0)
                        {
                            foreach (Boat boat in harBour[i])
                            {
                                sw.WriteLine($"{i};{boat.BoatType};{boat.BoatID};{boat.Weight};{boat.TopSpeed};{boat.uniqueProp};{boat.DaysLeft};{boat.BoatSize}");
                            }
                        }
                    }
                }
        }
        

        private static void ListsInArray(List<Boat>[] harBour)
        {
            for (int i = 0; i < harBour.Length; i++)
            {
                harBour[i] = new List<Boat>();
            }
        }

        public static void SlotCheckInHarbour(List<Boat>[] harBour, List<Boat> arrivalboats)
        {
            //sorterar listan med båtar med störst båt först
            List<Boat> SortedBoats = arrivalboats.OrderByDescending(b => b.BoatSize).ToList();

            //för varje båt i listan, försök att placera den i hamnen, börjar med största båten först
            foreach (Boat b in SortedBoats)
            {
                //metoden returnerar en bool om den fick lägga till eller inte
                if (PlaceBoatInHarbour(harBour, b))
                {
                    //boat is in harbour.
                }
                else
                {
                    //Avvisa båt
                    Console.WriteLine("Avvisad båt" + b.BoatID + "\t" + b.BoatType);
                    rejectedBoats++;
                    
                }
            }

        }

        //returnerar true om båten är fick lägga till i hamnen, false om den blev avvisad
        public static bool PlaceBoatInHarbour(List<Boat>[] harBour, Boat currentBoat)
        {

            //loopa igenom alla platser i hamnen
            for (int i = 0; i < harBour.Length; i++)
            {
                
                if (harBour[i].Count == 1 && currentBoat is Rowboat && harBour[i].First() is Rowboat)
                {
                    currentBoat.SlotNumber = i;
                    harBour[i].Add(currentBoat);
                    return true; 
                }
               

                //Om platsen är tom och edge-case om listan tar slut
                if (harBour[i].Count == 0 && currentBoat.BoatSize + i <= harBour.Length)
                {
                    int startIndex = i;
                    //Närliggande lediga platser
                    int numOfAdjacent = 0;
                    //Kollar om de nästkommande platserna också är tomma, t.o.m. båtens storlek
                    for (int j = startIndex; j < startIndex + currentBoat.BoatSize; j++)
                    {
                        //om dom är det adderar vi
                        if (harBour[j].Count == 0)
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
                            currentBoat.SlotNumber = j;
                            harBour[j].Add(currentBoat);
                            
                        }
                        //Vi kunde lägga till båten --> true
                        return true;
                    }
                }
            }
            //fall vi kör igenom hela for-loopen och inget händer får vi avvisa båten
            rejectedBoats++;
            return false;
            

        }
        private static void DailyArrivalBoats(List<Boat> arrivalboats)
        {
            arrivalboats.Clear();
            for (int i = 0; i < 5; i++)
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
                    r.uniqueProp = rnd.Next(1, 6);
                    r.DaysLeft = 1;
                    r.BoatSize = 1;

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
                    m.uniqueProp = rnd.Next(10, 1000);
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
                    s.uniqueProp = sailBoatLengt;
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
                    c.uniqueProp = rnd.Next(0, 500);
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
                    if (i == harBour.Length - 1 || harBour[i+1].Count == 0) 
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
        private static void PrintingBoat( List<Boat>[] harBour)
        {
            int freespaceCounter = 0;
            //Hashset kan inte innehålla multipla objekt.
            HashSet<Boat> allBoats = new HashSet<Boat>();

            Console.WriteLine($"Plats \t Båttyp \t ID \t Vikt \t MaxHastighet \t Unika egenskaper");
            Console.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < harBour.Length; i++)
            {
                if (harBour[i].Count == 0)
                {
                    Console.WriteLine($"{i}\t Tomt");
                    freespaceCounter++;
                }
                else
                {  
                    foreach (Boat boat in harBour[i])
                    {
                        allBoats.Add(boat);
                        if (boat is Rowboat)
                        {
                            Console.WriteLine($"{i}\t {boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {boat.uniqueProp}\tpassagerare");
                        }
                        else if (boat is MotorBoat)
                        {
                            Console.WriteLine($"{i}\t {boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {boat.uniqueProp}\thästkrafter");  
                        }
                        else if (boat is SailBoat)
                        {
                            Console.WriteLine($"{i}\t{boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {boat.uniqueProp} \t m");   
                        }
                        else if (boat is CargoShip)
                        {
                            Console.WriteLine($"{i}\t{boat.BoatType} \t {boat.BoatID} \t {boat.Weight} \t {boat.TopSpeed} km/h \t {boat.uniqueProp}\tContainrar");   
                        }
                    }  
                }
            }

            Console.WriteLine("----------Summering------------");

            //double totalRowboat = allBoats.Where(boat => boat.BoatType == "RowBoat").Count();

            var ofTypeCargo = allBoats.OfType<CargoShip>();
            Console.WriteLine($"Lastbåtar: {ofTypeCargo.Count()}");

            var ofTypeSailBoat = allBoats.OfType<SailBoat>();
            Console.WriteLine($"Sailbåtar: {ofTypeSailBoat.Count()}");

            var ofTypeMotorBoat = allBoats.OfType<MotorBoat>();
            Console.WriteLine($"Motorbåtar:{ofTypeMotorBoat.Count()}");

            var ofTypeRowBoat = allBoats.OfType<Rowboat>();
            Console.WriteLine($"Roddbåtar: {ofTypeRowBoat.Count()}");

            //Summerar vikten
            double totalWeight = allBoats.Sum(boat => boat.Weight);

            //Medelvärde av maxhastighet
            double averageSpeed = allBoats.Average(boat => boat.TopSpeed);


            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Total Vikt:      {totalWeight}");
            Console.WriteLine($"Medelhastighet:  {averageSpeed}");
            Console.WriteLine($"Avisade båtar:   {rejectedBoats}");
            Console.WriteLine($"Lediga platser:  {freespaceCounter}");



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
