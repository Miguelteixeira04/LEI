namespace APIBartleZ
{
    public class CvCAnswers
    {
        public int PlayerID { get; set; }
        public int QuestionID { get; set; }
        public string Nature { get; set; }
        public string Answer { get; set; }
        public float TimeOnDecision { get; set; }

        public CvCAnswers()
        {
            PlayerID = int.MinValue;
            QuestionID = int.MinValue;
            Nature = string.Empty;
            Answer = string.Empty;
            TimeOnDecision = long.MinValue;
        }

        public CvCAnswers(int iD, int playerID, int questionID, string nature, string answer, long tod)
        {
            PlayerID = playerID;
            QuestionID = questionID;
            Nature = nature;
            Answer = answer;
            TimeOnDecision = tod;
        }
    }
}
