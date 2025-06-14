using Microsoft.Data.SqlClient;

namespace APIBartleZ
{
    public class Player
    {
        public int PNumber { get; set; }
        public string Age { get; set; }
        public string OftenPlayVideoGames { get; set; }
        public List<TypesofGamesPlay> typesofGamesPlays { get; set; }
        public string Motivation { get; set; }
        public string achievinggoals { get; set; }
        public string discovering {  get; set; }
        public string socializing { get; set; }
        public string competing { get; set; }
        public string HavePlayerTypeQuiz { get; set; }
        public string ResultsAccurate { get; set; }
        public string ThoughtsQUIZvsGAME { get; set; }
        public int HowEnjoyTeam { get; set; }
        public string IndependentvsColaboration { get; set; }
        public int Winning { get; set; }
        public string CompetingMotivation { get; set; }
        public string TasksInGamesPrio { get; set; }
        public int WorkUnderConstraints { get; set; }
        public List<CheckboxAnswer> motcrowd {  get; set; }
        public string preference { get; set; }
        public string comp {  get; set; }
        public string coop { get; set; }
        public List<CheckboxAnswer> gamemot {  get; set; }
        public int ssordergamemot { get; set; }
        public int lordergamemot { get; set; }
        public int rankordergamemot { get; set; }
        public int teamordergamemot { get; set; }
        public int timeordergamemot { get; set; }
        public int rewardordergamemot { get; set; }
        public int diffordergamemot { get; set; }
        public List<CheckboxAnswer> crowdenjoy { get; set; }
        public int ssordercrowdenjoy { get; set; }
        public int lordercrowdenjoy { get; set; }
        public int rankordercrowdenjoy { get; set; }
        public int teamordercrowdenjoy { get; set; }
        public int timeordercrowdenjoy { get; set; }
        public int rewardordercrowdenjoy { get; set; }
        public int diffordercrowdenjoy { get; set; }
        public string compcoopcrowd { get; set; }
        public string typecrowd { get; set; }

        public Player(int pNumber, string age, string oftenPlayVideoGames, List<TypesofGamesPlay> typesofGamesPlays, string motivation, string achievinggoals, string discovering, string socializing, string competing, string havePlayerTypeQuiz, string resultsAccurate, string thoughtsQUIZvsGAME, int howEnjoyTeam, string independentvsColaboration, int winning, string competingMotivation, string tasksInGamesPrio, int workUnderConstraints, List<CheckboxAnswer> motcrowd, string preference, string comp, string coop, List<CheckboxAnswer> gamemot, int ssordergamemot, int lordergamemot, int rankordergamemot, int teamordergamemot, int timeordergamemot, int rewardordergamemot, int diffordergamemot, List<CheckboxAnswer> crowdenjoy, int ssordercrowdenjoy, int lordercrowdenjoy, int rankordercrowdenjoy, int teamordercrowdenjoy, int timeordercrowdenjoy, int rewardordercrowdenjoy, int diffordercrowdenjoy, string compcoopcrowd, string typecrowd)
        {
            PNumber = pNumber;
            Age = age;
            OftenPlayVideoGames = oftenPlayVideoGames;
            this.typesofGamesPlays = typesofGamesPlays;
            Motivation = motivation;
            this.achievinggoals = achievinggoals;
            this.discovering = discovering;
            this.socializing = socializing;
            this.competing = competing;
            HavePlayerTypeQuiz = havePlayerTypeQuiz;
            ResultsAccurate = resultsAccurate;
            ThoughtsQUIZvsGAME = thoughtsQUIZvsGAME;
            HowEnjoyTeam = howEnjoyTeam;
            IndependentvsColaboration = independentvsColaboration;
            Winning = winning;
            CompetingMotivation = competingMotivation;
            TasksInGamesPrio = tasksInGamesPrio;
            WorkUnderConstraints = workUnderConstraints;
            this.motcrowd = motcrowd;
            this.preference = preference;
            this.comp = comp;
            this.coop = coop;
            this.gamemot = gamemot;
            this.ssordergamemot = ssordergamemot;
            this.lordergamemot = lordergamemot;
            this.rankordergamemot = rankordergamemot;
            this.teamordergamemot = teamordergamemot;
            this.timeordergamemot = timeordergamemot;
            this.rewardordergamemot = rewardordergamemot;
            this.diffordergamemot = diffordergamemot;
            this.crowdenjoy = crowdenjoy;
            this.ssordercrowdenjoy = ssordercrowdenjoy;
            this.lordercrowdenjoy = lordercrowdenjoy;
            this.rankordercrowdenjoy = rankordercrowdenjoy;
            this.teamordercrowdenjoy = teamordercrowdenjoy;
            this.timeordercrowdenjoy = timeordercrowdenjoy;
            this.rewardordercrowdenjoy = rewardordercrowdenjoy;
            this.diffordercrowdenjoy = diffordercrowdenjoy;
            this.compcoopcrowd = compcoopcrowd;
            this.typecrowd = typecrowd;
        }
    }
}
