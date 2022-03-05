
using System;
using System.Collections;
using System.Collections.Generic;
namespace Commercial_Controller
{
    public class Column
    {
        public string ID;
        public string status;
        public List<int> servedFloors;
        public bool isBasement;
        public List<Elevator> elevatorsList;
        public List<CallButton> callButtons;

        public Column(string _ID, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            //giving the column its properties
            this.ID = _ID;
            this.status = "online";
            this.servedFloors = _servedFloors;
            this.isBasement = _isBasement;
            this.elevatorsList = new List<Elevator>();
            this.callButtons = new List<CallButton>();

            this.createCallButtons(servedFloors, isBasement);
            this.createElevators(servedFloors, _amountOfElevators);
        }

        public void createCallButtons(List<int> servedFloors, bool isBasement)
        {
            //populating the call buttons
            int buttonId = 1;
            if (isBasement) {
                int buttonFloor = -1;
                foreach (int item in servedFloors)
                {
                    CallButton callButton = new CallButton(buttonId, buttonFloor, "up");
                    this.callButtons.Add(callButton);
                    buttonFloor++;
                    buttonId++;
                }
            } else {
                int buttonFloor = 1;
                foreach (int item in servedFloors)
                {
                    CallButton callButton = new CallButton(buttonId, buttonFloor, "down");
                    this.callButtons.Add(callButton);
                    buttonFloor++;
                    buttonId++;
                }
            }
        }
        public void createElevators(List<int> servedFloors, int amountOfElevators)
        {
            //populating the elevators
            int elevatorID = 1;
            for (int i = 0; i < amountOfElevators; i++) {
                Elevator elevator = new Elevator(elevatorID.ToString());
                this.elevatorsList.Add(elevator);
                elevatorID++;
            }
        }
        public Elevator requestElevator(int requestedFloor, string direction)
        {
            //this is the function for the users to be able to request an elevator
            Elevator elevator = this.findElevator(requestedFloor, direction);
            elevator.addNewRequest(requestedFloor);
            elevator.move();

            //back to lobby
            elevator.addNewRequest(1);
            elevator.move();
            return elevator;
        }

        public Hashtable checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, int bestScore, int referenceGap, Elevator bestElevator, int floor)
                { //checking if the elevator chosen is best
                    if (scoreToCheck < bestScore) {
                        bestScore = scoreToCheck;
                        bestElevator = newElevator;
                        referenceGap = Math.Abs(newElevator.currentFloor - floor);
                    } else if (bestScore == scoreToCheck) {
                        int gap = Math.Abs(newElevator.currentFloor - floor);
                        if (referenceGap > gap) {
                            bestElevator = newElevator;
                            referenceGap = gap;
                        }
                    }
                    Hashtable info = new Hashtable();
                    info.Add("bestElevator", bestElevator);
                    info.Add("bestScore", bestScore);
                    info.Add("referenceGap", referenceGap);
                    
                    return info;
                }
       public Elevator findElevator(int requestedFloor, string direction)
        { //finding where the elevator is based on a scoring system
            Elevator bestElevator = null;
            int bestScore = 100;
            int referenceGap = 100000;
            Hashtable bestElevatorinformation;

            if (requestedFloor == 1) {
                foreach (Elevator elevator in this.elevatorsList)
                {
                    // Elevator is stopped at lobby and has requests
                    if (1 == elevator.currentFloor && elevator.status == "stopped") {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is idle at the lobby, has no requests
                    else if (1 == elevator.currentFloor && elevator.status == "idle") {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is lower than user and coming up
                    else if (1 > elevator.currentFloor && elevator.direction == "up") {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is above the user and coming down
                    else if (1 < elevator.currentFloor && elevator.direction == "down") {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is not at the lobby, but is idle and has no requests
                    else if (elevator.status == "idle") {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is not available, but could take the request if there's nothing better
                    else {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }

                    bestElevator = (Elevator)bestElevatorinformation["bestElevator"];
                    bestScore = (int)bestElevatorinformation["bestScore"];
                    referenceGap = (int)bestElevatorinformation["referenceGap"];
                }
            }
            // If requested floor is not the lobby
            else {
                foreach (Elevator elevator in this.elevatorsList)
                {
                    // Elevator is stopped at the same level as user, about to go to lobby
                    if (requestedFloor == elevator.currentFloor && elevator.status == "stopped" && elevator.direction == direction) {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is lower than user, and going up towards the lobby
                    else if (requestedFloor > elevator.currentFloor && elevator.direction == "up" && elevator.direction == direction) {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is above user and going down towards the lobby
                    else if (requestedFloor < elevator.currentFloor && elevator.direction == "down" && elevator.direction == direction) {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is idle
                    else if (elevator.status == "idle") {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    // Elevator is not available but can still take the request
                    else {
                        bestElevatorinformation = this.checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    bestElevator = (Elevator)bestElevatorinformation["bestElevator"];
                    bestScore = (int)bestElevatorinformation["bestScore"];
                    referenceGap = (int)bestElevatorinformation["referenceGap"];
                }
            }
            return bestElevator;
        }
        
    }
}