namespace APIBartleZ
{
    public class Rank
    {
        public int playerID { get; set; }
        public string name { get; set; }

        public Rank(int playerID, string name)
        {
            this.playerID = playerID;
            this.name = name;
        }
    }
}
