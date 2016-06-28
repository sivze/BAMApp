using System;
using System.Threading;

namespace ConsoleApplication1
{
    //============================================================
    class Data                                                //storage class to share data between floor and elevator
    {
        public int[] buttonFloor;                         //button info of each floor.
        public Random rand;                             //to generate random value for dalay of simulation
        private const int NUM_FLOORS = 5;        //define the number of floors in a building
        public Data()
        {
            buttonFloor = new int[NUM_FLOORS];
            rand = new Random();
        }
    }

    //============================================================
    interface IMachine                                     //interface for a maching having a door
    {
        void move();
        void openDoor();
    }

    //============================================================
    interface IButton                                        //interface for classes having buttons
    {
        void pressButton();
    }

    //============================================================
    class Floor : IButton                                  //there are elevator button on each floor
    {
        private const int RANDOM_SLEEP_MAX = 10000;
        private const int NUM_FLOORS = 5;        //define the number of floors in a building

        private int floorNum;                   //n-th floor
        private Data data;                       //data to share information between an elevator and floor

        public Floor(int aFloorNum, ref Data aData)
        {
            floorNum = aFloorNum;
            data = aData;
        }

        public void pressButton()             //press floor button
        {
            for (;;)
            {
                Thread.Sleep(data.rand.Next(RANDOM_SLEEP_MAX));   //delay for simulation

                int button = data.rand.Next(5);            //Probability to press button is 40%. i.e. {1,2} will be taken in {0,1,2,3,4}
                if (button == 1)                                  //Up button is pressed.
                {
                    if (floorNum != (NUM_FLOORS - 1))
                        data.buttonFloor[floorNum] = 1;
                }
                else if (button == 2)                           //Down button is pressed.
                {
                    if (floorNum != 0)
                        data.buttonFloor[floorNum] = 2;
                }
            }
        }
    }

    //============================================================
    class Elevator : IMachine, IButton      //an elevator is able to move and has a door and buttons.
    {
        private int floorNum, status;          //floorNum is the current location, and 'status' info is (0:stop, 1:up 2:down)
        public bool[] button;                     //buttons inside the elevator
        private bool door;                        //true: door is opened, false: door is closed
        private Data data;                        //data to share information between an elevator and floor

        private const int NUM_FLOORS = 5;          //the number of buttons inside elevator

        public int FloorNum { get { return floorNum; } }       //Properties are made for monitoring.
        public int Status { get { return status; } }
        public bool Door { get { return door; } }

        public Elevator(ref Data aData)
        {
            floorNum = 0;
            status = 0;
            door = false;
            data = aData;
            button = new bool[NUM_FLOORS];

            for (int i = 0; i < NUM_FLOORS; i++)
                button[i] = false;
        }

        //-----------------------------------------------------------------------------------------------
        public void move()
        {
            for (;;)
            {
                if (status == 0)                                      //if elevator is stoped,
                {
                    status = hasToMove();
                    if (data.buttonFloor[floorNum] != 0)     //if the current floor button is pressed, then open door
                    {
                        openDoor();
                        data.buttonFloor[floorNum] = 0;
                    }
                }
                else if (status == 1)                            //if the elevator moves up
                {
                    bool flag = moveUp();
                }
                else if (status == 2)                            //if the elevator moves down
                {
                    bool flag = moveDown();
                }
            }
        }

        //-----------------------------------------------------------------------------------------------
        private bool moveUp()
        {
            if (floorNum == (NUM_FLOORS - 1))          //if floor is the top, then stop elevator
            {
                status = 0;
                return false;
            }

            Thread.Sleep(1000);                               //gap while elevator is moving
            floorNum += 1;                                       //move elevator up

            if (button[floorNum] || (data.buttonFloor[floorNum] == 1))
            {
                openDoor();
                button[floorNum] = false;
                if (data.buttonFloor[floorNum] == 1)
                    data.buttonFloor[floorNum] = 0;
            }
            if (floorNum == (NUM_FLOORS - 1)) //if elevator is on the top floor and floor button has been pressed, then open door
            {
                if (data.buttonFloor[floorNum] != 0)
                {
                    openDoor();
                    data.buttonFloor[floorNum] = 0;
                }
            }
            if (hasToMoveUp())               //if elevator has to move up, then status is set as 1.
                status = 1;
            else
                status = 0;
            return true;
        }

        //-----------------------------------------------------------------------------------------------
        private bool moveDown()
        {
            if (floorNum == 0)
            {
                status = 0;
                return false;
            }

            Thread.Sleep(1000);                                //gap while elevator is moving
            floorNum -= 1;                                        //move elevator down

            if (button[floorNum] || (data.buttonFloor[floorNum] == 2))
            {
                openDoor();
                button[floorNum] = false;
                if (data.buttonFloor[floorNum] == 2)
                    data.buttonFloor[floorNum] = 0;
            }
            if (floorNum == 0)         //if elevator is on the bottom floor and floor button has been pressed, then open door
            {
                if (data.buttonFloor[floorNum] != 0)
                {
                    openDoor();
                    data.buttonFloor[floorNum] = 0;
                }
            }
            if (hasToMoveDown())
                status = 2;
            else
                status = 0;
            return true;
        }

        //-----------------------------------------------------------------------------------------------
        private int hasToMove()                               //return value : 1(Up), 2(Down), otherwise 0(stop)
        {
            if (hasToMoveUp())
                return 1;
            if (hasToMoveDown())
                return 2;
            return 0;
        }

        //-----------------------------------------------------------------------------------------------
        private bool hasToMoveUp()
        {
            for (int i = (floorNum + 1); i < NUM_FLOORS; i++)      //after scanning buttons, decide the next action.
            {
                if (button[i] || (data.buttonFloor[i] != 0))
                {
                    return true;
                }
            }
            return false;
        }

        //-----------------------------------------------------------------------------------------------
        private bool hasToMoveDown()
        {
            for (int i = (floorNum - 1); i >= 0; i--)                      //after scanning buttons, decide the next action.
            {
                if (button[i] || (data.buttonFloor[i] != 0))
                {
                    return true;
                }
            }
            return false;
        }

        //-----------------------------------------------------------------------------------------------
        public void openDoor()
        {
            door = true;
            this.pressButton();
            Thread.Sleep(1000);                             //delay for opened door
            if (((status == 1) && (data.buttonFloor[floorNum] == 1)) || ((status == 2) && (data.buttonFloor[floorNum] == 2)))
            {                                                         //ex: ignore if elevator moves up and floor down button is pressed
                data.buttonFloor[floorNum] = 0;
            }
            button[floorNum] = false;
            door = false;
        }

        //-----------------------------------------------------------------------------------------------
        public void pressButton()                                     //a button inside elevator is pressed.
        {
            int buttonNum = data.rand.Next(NUM_FLOORS);
            if (data.buttonFloor[floorNum] == 1)        //if floor button is up, then only upper button in elevator has to be pressed.
            {
                buttonNum = (buttonNum % (NUM_FLOORS - floorNum + 1)) + floorNum;
                this.button[buttonNum] = true;
            }
            else if (data.buttonFloor[floorNum] == 2) //if floor button is down, then only lower button in elevator has to be pressed.
            {
                buttonNum = buttonNum % floorNum;
                this.button[buttonNum] = true;
            }
            else
                this.button[buttonNum] = true;
        }
    }//Elevator Class
}//namespace