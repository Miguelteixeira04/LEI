using NuGet.Versioning;

namespace APIBartleZ.Controllers
{
    public class GameInfo
    {
        public int PNumber { get; set; }
        public int TotalScore { get; set; }
        public float TotalTime { get; set; }
        public float TimeSpentRoom1 { get; set; }
        public float TimeSpentRoom2 { get; set; }
        public float TimeSpentRoom3 { get; set; }
        public float TimeSpentRoom4 { get; set; }
        public float TimeSpentRoom5 { get; set; }
        public float TimeSpentRoom6 { get; set; }
        public float TimeSpentRoom7 { get; set; }
        public float TimeSpentRoom8 { get; set; }
        public float TimeSpentRoom9 { get; set; }
        public int NumberOfReturns { get; set; }
        public int NumberOfDeaths { get; set; }
        public List<PTDAnswers> pTDAnswers { get; set; }
        public List<CvCAnswers> CvCAnswers { get; set; }
        public List<HITAnswer> hITAnswers { get; set; }
        public int PositionOnLeaderboard { get; set; }
        public string FinalRank { get; set; }
        public List<Rank> RankingHistory { get; set; }
        public int NumberOfBadges { get; set; }
        public List<Badge> badges { get; set; }
        public string dominanttype { get; set; }
        public float aperc { get; set; }
        public float eperc { get; set; }
        public float kperc { get; set; }
        public float sperc { get; set; }
        public float domainpercentage { get; set; }

        public GameInfo(int pNumber, int totalScore, float totalTime, float timeSpentRoom1, float timeSpentRoom2, float timeSpentRoom3, float timeSpentRoom4, float timeSpentRoom5, float timeSpentRoom6, float timeSpentRoom7, float timeSpentRoom8, float timeSpentRoom9, int numberOfReturns, int numberOfDeaths, List<PTDAnswers> pTDAnswers, List<CvCAnswers> cvCAnswers, List<HITAnswer> hITAnswers, int positionOnLeaderboard, string finalRank, List<Rank> rankingHistory, int numberOfBadges, List<Badge> badges, string dominanttype, float aperc, float eperc, float kperc, float sperc, float domainpercentage)
        {
            PNumber = pNumber;
            TotalScore = totalScore;
            TotalTime = totalTime;
            TimeSpentRoom1 = timeSpentRoom1;
            TimeSpentRoom2 = timeSpentRoom2;
            TimeSpentRoom3 = timeSpentRoom3;
            TimeSpentRoom4 = timeSpentRoom4;
            TimeSpentRoom5 = timeSpentRoom5;
            TimeSpentRoom6 = timeSpentRoom6;
            TimeSpentRoom7 = timeSpentRoom7;
            TimeSpentRoom8 = timeSpentRoom8;
            TimeSpentRoom9 = timeSpentRoom9;
            NumberOfReturns = numberOfReturns;
            NumberOfDeaths = numberOfDeaths;
            this.pTDAnswers = pTDAnswers;
            CvCAnswers = cvCAnswers;
            this.hITAnswers = hITAnswers;
            PositionOnLeaderboard = positionOnLeaderboard;
            FinalRank = finalRank;
            RankingHistory = rankingHistory;
            NumberOfBadges = numberOfBadges;
            this.badges = badges;
            this.dominanttype = dominanttype;
            this.aperc = aperc;
            this.eperc = eperc;
            this.kperc = kperc;
            this.sperc = sperc;
            this.domainpercentage = domainpercentage;
        }
    }
}
