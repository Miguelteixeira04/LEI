namespace APIBartleZ
{
    public class Badge
    {
        public int playerID { get; set; }
        public string name { get; set; }

        public Badge(int playerID, string name)
        {
            this.playerID = playerID;
            this.name = name;
        }
    }
}
