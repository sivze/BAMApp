using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Management.createBuilding();                 //build a building
            Management.simulateElevator();             //simulate elevator system
        }
    }
    //============================================================
    class Management
    {
        public static Data data;                               //data to share infomation between floor and elevator
        public static ArrayList floors;                       //array to store many floor objects
        public static Elevator elevator;                    //only one elevator of a building

        private const int NUM_FLOORS = 5;            //define the number of floors in a building

        public static void createBuilding()               //initialize building having an elevator.
        {
            data = new Data();
            elevator = new Elevator(ref data);
            floors = new ArrayList();
            for (int i = 0; i < NUM_FLOORS; i++)
            {
                Floor floor = new Floor(i, ref data);
                floors.Add(floor);
            }
        }

        public static void simulateElevator()            //simulate elevator system by using thread.
        {
            for (int i = 0; i < floors.Count; i++)                                                         //every floor is activated.
                new Thread(new ThreadStart(((Floor)floors[i]).pressButton)).Start();

            new Thread(new ThreadStart(elevator.move)).Start();                            //elevator is activated.

            //------------------------------------- Monitoring Elevator ------------------------------------
            for (;;)
            {
                Thread.Sleep(1000);                           //print out status every second.
                Console.WriteLine("---------------------------------------------------------");
                Console.Write("Elevator Status :   ");
                for (int i = 0; i < elevator.FloorNum; i++)
                    Console.Write("        ");
                Console.Write((elevator.Status == 2 ? "<-" : "  "));
                if (elevator.Door)
                    Console.Write("O");
                else
                    Console.Write("C");
                Console.Write((elevator.Status == 1 ? "-> " : "    "));
                Console.Write(" \n");
                Console.Write("Floor Buttons   :     ");
                for (int i = 0; i < 5; i++)
                {
                    if (data.buttonFloor[i] == 1)
                        Console.Write("U");
                    else if (data.buttonFloor[i] == 2)
                        Console.Write("D");
                    else
                        Console.Write("-");
                    Console.Write("       ");
                }
                Console.Write(" \n");
                Console.Write("Elevator Buttons:     ");
                for (int i = 0; i < 5; i++)
                    Console.Write((elevator.button[i] ? "T" : " ") + "       ");
                Console.Write(" \n");
            }
        }
    }
}
