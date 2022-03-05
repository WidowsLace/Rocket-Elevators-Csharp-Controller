using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    {
        public string ID;
        public string status;
        public int currentFloor;
        public string direction;
        public Door door;
        public List<int> floorRequestsList;
        public List<int> completedRequestsList;
        public Elevator(string _elevatorID)
        {
            this.ID = _elevatorID;
            this.status = "idle";
            this.currentFloor = 1;
            this.direction = null;
            this.door = new Door(Convert.ToInt32(ID));
            this.floorRequestsList = new List<int>();
            this.completedRequestsList = new List<int>();
        }
        public void move()
        {
            // elevators floor request list isnt empty
            while (this.floorRequestsList.Count != 0) {
                int[] tempArray = this.floorRequestsList.ToArray();
                int destination = tempArray[0];
                if (this.currentFloor < destination) {
                    this.direction = "up";
                    sortFloorList(tempArray);
                    if (this.door.status == "opened") {this.door.status = "closed";}
                    this.status = "moving";
                    
                    // Move elevator up
                    while (this.currentFloor != destination) {
                        this.currentFloor++;
                        Console.WriteLine("Floor: {0}", this.currentFloor);
                    }
                //elevator's current floor is higher than the destination
                } else if (this.currentFloor > destination) {
                    this.direction = "down";
                    sortFloorList(tempArray);
                    if (this.door.status == "opened") {this.door.status = "closed";}
                    this.status = "moving";

                    // Move the elevator down until it reaches the floor
                    while (this.currentFloor != destination) {
                        this.currentFloor--;
                        Console.WriteLine("Floor: {0}", this.currentFloor);
                    }
                }
                this.status = "stopped";
                this.door.status = "opened";
                this.completedRequestsList.Add(destination);
                this.floorRequestsList.RemoveAt(0);
            }
        }

        //sort floor list from top to bottom or bottom to top
        public void sortFloorList(int[] requestList) {
            if (this.direction == "up") {
                Array.Sort(requestList);
            } else {
                Array.Sort(requestList);
                Array.Reverse(requestList);
            }
        }
        
        //add new request and change direction
        public void addNewRequest(int requestedFloor) {
            if (!(this.floorRequestsList.Contains(requestedFloor)) ) {
                floorRequestsList.Add(requestedFloor);
            }

            if (this.currentFloor < requestedFloor) {
                this.direction = "up";
            } else if (this.currentFloor > requestedFloor) {
                this.direction = "down";
            }
        }
    }
}