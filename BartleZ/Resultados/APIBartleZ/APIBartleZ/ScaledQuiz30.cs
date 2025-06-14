namespace APIBartleZ
{
    public class ScaledQuiz30
    {
        public int PNumber { get; set; }
        public string Age { get; set; }
        public string OftenPlayVideoGames { get; set; }
        public List<TypesofGamesPlay> typesofGamesPlays { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }
        public string Q7 { get; set; }
        public string Q8 { get; set; }
        public string Q9 { get; set; }
        public string Q10 { get; set; }
        public string Q11 { get; set; }
        public string Q12 { get; set; }
        public string Q13 { get; set; }
        public string Q14 { get; set; }
        public string Q15 { get; set; }
        public string Q16 { get; set; }
        public string Q17 { get; set; }
        public string Q18 { get; set; }
        public string Q19 { get; set; }
        public string Q20 { get; set; }
        public string Q21 { get; set; }
        public string Q22 { get; set; }
        public string Q23 { get; set; }
        public string Q24 { get; set; }
        public string Q25 { get; set; }
        public string Q26 { get; set; }
        public string Q27 { get; set; }
        public string Q28 { get; set; }
        public string Q29 { get; set; }
        public string Q30 { get; set; }
        public int KCount { get; set; }
        public int ACount { get; set; }
        public int SCount { get; set; }
        public int ECount { get; set; }
        public float KPercentage { get; set; }
        public float APercentage { get; set; }
        public float SPercentage { get; set; }
        public float EPercentage { get; set; }
        public string DominantType { get; set; }

        public ScaledQuiz30(int pNumber, string age, string oftenPlayVideoGames, List<TypesofGamesPlay> typesofGamesPlays, string q1, string q2, string q3, string q4, string q5, string q6, string q7, string q8, string q9, string q10, string q11, string q12, string q13, string q14, string q15, string q16, string q17, string q18, string q19, string q20, string q21, string q22, string q23, string q24, string q25, string q26, string q27, string q28, string q29, string q30, int kCount, int aCount, int sCount, int eCount, float kPercentage, float aPercentage, float sPercentage, float ePercentage, string dominantType)
        {
            PNumber = pNumber;
            Age = age;
            OftenPlayVideoGames = oftenPlayVideoGames;
            this.typesofGamesPlays = typesofGamesPlays;
            Q1 = q1;
            Q2 = q2;
            Q3 = q3;
            Q4 = q4;
            Q5 = q5;
            Q6 = q6;
            Q7 = q7;
            Q8 = q8;
            Q9 = q9;
            Q10 = q10;
            Q11 = q11;
            Q12 = q12;
            Q13 = q13;
            Q14 = q14;
            Q15 = q15;
            Q16 = q16;
            Q17 = q17;
            Q18 = q18;
            Q19 = q19;
            Q20 = q20;
            Q21 = q21;
            Q22 = q22;
            Q23 = q23;
            Q24 = q24;
            Q25 = q25;
            Q26 = q26;
            Q27 = q27;
            Q28 = q28;
            Q29 = q29;
            Q30 = q30;
            KCount = kCount;
            ACount = aCount;
            SCount = sCount;
            ECount = eCount;
            KPercentage = kPercentage;
            APercentage = aPercentage;
            SPercentage = sPercentage;
            EPercentage = ePercentage;
            DominantType = dominantType;
        }
    }
}
