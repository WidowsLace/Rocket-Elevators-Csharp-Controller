namespace Commercial_Controller
{
    public class CallButton
    { //giving the call buttons their properties
        public int ID;
        public int floor;
        public string direction;
        public CallButton(int _ID, int _floor, string _direction)
        {
            this.ID = _ID;
            this.floor = _floor;
            this.direction = _direction;
        }
    }
}