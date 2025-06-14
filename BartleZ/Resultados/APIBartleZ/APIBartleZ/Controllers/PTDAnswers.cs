namespace APIBartleZ.Controllers
{
    public class PTDAnswers
    {
        public int PlayerID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public float TimeOnDecision { get; set; }


        public PTDAnswers()
        {
            PlayerID = int.MinValue;
            QuestionID = int.MinValue;
            Answer = string.Empty;
            TimeOnDecision = long.MinValue;
        }

        public PTDAnswers(int playerID, int questionID, string answer, long tod)
        {
            PlayerID = playerID;
            QuestionID = questionID;
            Answer = answer;
            TimeOnDecision = tod;
        }
    }
}
