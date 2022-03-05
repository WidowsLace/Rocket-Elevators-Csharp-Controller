using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Battery
    {
        public int ID = 1;
        public int columnID = 1;
        public string status;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorRequestButtonList;
        
        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        { //giving the battery its properties
            this.ID = _ID;
            this.status = "online";
            this.columnsList = new List<Column>();
            this.floorRequestButtonList = new List<FloorRequestButton>();

            if (_amountOfBasements > 0) {
                this.createBasementFloorRequestButtons(_amountOfBasements);
                this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                _amountOfColumns--;
            }
            
            this.createFloorRequestButtons(_amountOfFloors);
            this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfBasements, _amountOfElevatorPerColumn);
        }

        public Column findBestColumn(int _requestedFloor)
        { //determining which column is best 
            foreach (Column column in this.columnsList) {
                // Loop through each floor in columns servedFloors list
                for (int i = 0; i < column.servedFloors.Count; i+= 1) {
                    if (column.servedFloors[i] == _requestedFloor) {
                        return column;
                    }
                }
            }
            return null;
        }
        //user presses a button at lobby
        public (Column, Elevator) assignElevator(int requestedFloor, string direction)
        { //assigning the elevator to the column using the requested floor and the direction
            Column chosenColumn = this.findBestColumn(requestedFloor);
            Elevator chosenElevator = chosenColumn.findElevator(1, direction);
            // Add the Lobby to request list
            chosenElevator.addNewRequest(1);
            //elevator to the lobby
            chosenElevator.move();
            // Add the requested floor to request list
            chosenElevator.addNewRequest(requestedFloor);
            chosenElevator.move();

            return (chosenColumn, chosenElevator);
        }
        
        public void createColumns(int amountOfColumns, int amountOfFloors, int amountOfBasements, int amountOfElevatorPerColumn)
        { //populating columns
            decimal temp = amountOfFloors / amountOfColumns;
            int amountOfFloorsPerColumn = Convert.ToInt32(Math.Ceiling(temp));
            int floor = 1;

            // For each floor within the column, add a floor to the servedFloors list
            for (int i = 0; i < amountOfColumns; i++) {
                List<int> servedFloors = new List<int>();
                for (int j = 0; j < amountOfFloorsPerColumn; j++) {
                    if (floor <= amountOfFloors) {
                        servedFloors.Add(floor);
                        floor++;
                    }
                }
                // Create a column and add to list
                Column column = new Column(columnID.ToString(), amountOfElevatorPerColumn, servedFloors, false);
                this.columnsList.Add(column);
            }
        }
        public void createBasementColumn(int amountOfBasements, int elevatorsPerColumn)
                {
                    List<int> servedFloors = new List<int>();
                    int floor = -1;

                    //floor levels column will serve 
                    for (int i = 0; i < amountOfBasements; i++) {
                        servedFloors.Add(floor);
                        floor--;
                    }

                    Column column = new Column(columnID.ToString(), elevatorsPerColumn, servedFloors, true);
                    this.columnsList.Add(column);
                    columnID++;
                }
                public void createBasementFloorRequestButtons(int amountOfBasements)
                { //populating the basement column
                    int buttonFloor = -1, floorRequestButtonID = 1;
                    // For each basement, create a floor request button
                    for (int i = 0; i < amountOfBasements; i++) {
                        FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, buttonFloor, "down");
                        this.floorRequestButtonList.Add(floorRequestButton);
                        buttonFloor--;
                        floorRequestButtonID++;
                    }
                }
        public void createFloorRequestButtons(int amountOfFloors)
        { //populating the floor request buttons for both basements and floors above lobby
            int buttonFloor = 1, floorRequestButtonID = 1;
            // For each above ground floor, create a floor request button
            for (int i = 0; i < amountOfFloors; i++) {
                FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, buttonFloor, "up");
                this.floorRequestButtonList.Add(floorRequestButton);
                buttonFloor++;
                floorRequestButtonID++;
            }
        }
    }
}