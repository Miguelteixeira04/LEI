namespace APIBartleZ
{
    public class CheckboxAnswer
    {
        public int playerID {  get; set; }
        public string answer { get; set; }

        public CheckboxAnswer()
        {
            playerID = int.MinValue;
            answer = string.Empty;
        }

        public CheckboxAnswer(int playerID, string answer)
        {
            this.playerID = playerID;
            this.answer = answer;
        }
    }
}
