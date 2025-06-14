namespace APIBartleZ
{
    public class HITAnswer
    {
        public int PlayerID { get; set; }
        public int QuestionID { get; set; }
        public string DifficultyChosen { get; set; }
        public bool Correct {  get; set; }
        public string Answer { get; set; }
        public int PointsGathered { get; set; }
        public float TimeOnDecision { get; set; }

        public HITAnswer()
        {
            PlayerID = int.MinValue;
            QuestionID = int.MinValue;
            DifficultyChosen = string.Empty;
            Correct = false;
            Answer = string.Empty;
            PointsGathered = int.MinValue;
            TimeOnDecision = long.MinValue;
        }

        public HITAnswer(int iD, int playerID, int questionID, string dchosen, bool correct, string answer, int pgath, long tod)
        {
            PlayerID = playerID;
            QuestionID = questionID;
            DifficultyChosen = dchosen;
            Correct = correct;
            Answer = answer;
            PointsGathered= pgath;
            TimeOnDecision = tod;
        }
    }
}
