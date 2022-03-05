namespace Commercial_Controller
{
    public class Door
    { //giving the doors their properties
        public int ID;
        public string status;
        public Door(int _id)
        {
            this.ID = _id;
            this.status = "closed";
        }
    }
}