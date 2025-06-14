using Microsoft.Data.SqlClient;

namespace APIBartleZ
{
    public class ScaledQuiz
    {
        public int PNumber { get; set; }
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
        public string Q31 { get; set; }
        public string Q32 { get; set; }
        public string Q33 { get; set; }
        public string Q34 { get; set; }
        public string Q35 { get; set; }
        public string Q36 { get; set; }
        public string Q37 { get; set; }
        public string Q38 { get; set; }
        public string Q39 { get; set; }
        public int KCount { get; set; }
        public int ACount { get; set; }
        public int SCount { get; set; }
        public int ECount { get; set; }
        public float KPercentage { get; set; }
        public float APercentage { get; set; }
        public float SPercentage { get; set; }
        public float EPercentage { get; set; }
        public string DominantType { get; set; }


        public ScaledQuiz()
        {
            PNumber = int.MinValue;
            Q1 = string.Empty;
            Q2 = string.Empty;
            Q3 = string.Empty;
            Q4 = string.Empty;
            Q5 = string.Empty;
            Q6 = string.Empty;
            Q7 = string.Empty;
            Q8 = string.Empty;
            Q9 = string.Empty;
            Q10 = string.Empty;
            Q11 = string.Empty;
            Q12 = string.Empty;
            Q13 = string.Empty;
            Q14 = string.Empty;
            Q15 = string.Empty;
            Q16 = string.Empty;
            Q17 = string.Empty;
            Q18 = string.Empty;
            Q19 = string.Empty;
            Q20 = string.Empty;
            Q21 = string.Empty;
            Q22 = string.Empty;
            Q23 = string.Empty;
            Q24 = string.Empty;
            Q25 = string.Empty;
            Q26 = string.Empty;
            Q27 = string.Empty;
            Q28 = string.Empty;
            Q29 = string.Empty;
            Q30 = string.Empty;
            Q31 = string.Empty;
            Q32 = string.Empty;
            Q33 = string.Empty;
            Q34 = string.Empty;
            Q35 = string.Empty;
            Q36 = string.Empty;
            Q37 = string.Empty;
            Q38 = string.Empty;
            Q39 = string.Empty;
            KCount = int.MinValue;
            ACount = int.MinValue;
            SCount = int.MinValue;
            ECount = int.MinValue;
            KPercentage = float.MinValue;
            APercentage = float.MinValue;
            SPercentage = float.MinValue;
            EPercentage = float.MinValue;
            DominantType = string.Empty;

        }

        public ScaledQuiz(int iD, int pNumber, string q1, string q2, string q3, string q4, string q5, string q6, string q7, string q8, string q9, string q10, string q11, string q12, string q13, string q14, string q15, string q16, string q17, string q18, string q19, string q20, string q21, string q22, string q23, string q24, string q25, string q26, string q27, string q28, string q29, string q30, string q31, string q32, string q33, string q34, string q35, string q36, string q37, string q38, string q39, int kCount, int aCount, int sCount, int eCount, float kPercentage, float aPercentage, float sPercentage, float ePercentage, string dominantType)
        {
            PNumber = pNumber;
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
            Q31 = q31;
            Q32 = q32;
            Q33 = q33;
            Q34 = q34;
            Q35 = q35;
            Q36 = q36;
            Q37 = q37;
            Q38 = q38;
            Q39 = q39;
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

        public ScaledQuiz ReadItem(SqlDataReader reader)
        {
            ScaledQuiz item = new ScaledQuiz();

            item.PNumber = Convert.ToInt32(reader["PNumber"]);
            item.Q1 = Convert.ToString(reader["Q1"]);
            item.Q2 = Convert.ToString(reader["Q2"]);
            item.Q3 = Convert.ToString(reader["Q3"]);
            item.Q4 = Convert.ToString(reader["Q4"]);
            item.Q5 = Convert.ToString(reader["Q5"]);
            item.Q6 = Convert.ToString(reader["Q6"]);
            item.Q7 = Convert.ToString(reader["Q7"]);
            item.Q8 = Convert.ToString(reader["Q8"]);
            item.Q9 = Convert.ToString(reader["Q9"]);
            item.Q10 = Convert.ToString(reader["Q10"]);
            item.Q11 = Convert.ToString(reader["Q11"]);
            item.Q12 = Convert.ToString(reader["Q12"]);
            item.Q13 = Convert.ToString(reader["Q13"]);
            item.Q14 = Convert.ToString(reader["Q14"]);
            item.Q15 = Convert.ToString(reader["Q15"]);
            item.Q16 = Convert.ToString(reader["Q16"]);
            item.Q17 = Convert.ToString(reader["Q17"]);
            item.Q18 = Convert.ToString(reader["Q18"]);
            item.Q19 = Convert.ToString(reader["Q19"]);
            item.Q20 = Convert.ToString(reader["Q20"]);
            item.Q21 = Convert.ToString(reader["Q21"]);
            item.Q22 = Convert.ToString(reader["Q22"]);
            item.Q23 = Convert.ToString(reader["Q23"]);
            item.Q24 = Convert.ToString(reader["Q24"]);
            item.Q25 = Convert.ToString(reader["Q25"]);
            item.Q26 = Convert.ToString(reader["Q26"]);
            item.Q27 = Convert.ToString(reader["Q27"]);
            item.Q28 = Convert.ToString(reader["Q28"]);
            item.Q29 = Convert.ToString(reader["Q29"]);
            item.Q30 = Convert.ToString(reader["Q30"]);
            item.Q31 = Convert.ToString(reader["Q31"]);
            item.Q32 = Convert.ToString(reader["Q32"]);
            item.Q33 = Convert.ToString(reader["Q33"]);
            item.Q34 = Convert.ToString(reader["Q34"]);
            item.Q35 = Convert.ToString(reader["Q35"]);
            item.Q36 = Convert.ToString(reader["Q36"]);
            item.Q37 = Convert.ToString(reader["Q37"]);
            item.Q38 = Convert.ToString(reader["Q38"]);
            item.Q39 = Convert.ToString(reader["Q39"]);
            item.KCount = Convert.ToInt32(reader["KCount"]);
            item.ACount = Convert.ToInt32(reader["ACount"]);
            item.SCount = Convert.ToInt32(reader["SCount"]);
            item.ECount = Convert.ToInt32(reader["ECount"]);
            item.KPercentage = (float)reader["KPercentage"];
            item.APercentage = (float)reader["APercentage"];
            item.SPercentage = (float)reader["SPercentage"];
            item.EPercentage = (float)reader["EPercentage"];
            item.DominantType = Convert.ToString(reader["DominantType"]);

            return item;
        }

        public void WriteItem(SqlCommand cmd)
        {
            cmd.Parameters.Add("@pnum", System.Data.SqlDbType.Int).Value = this.PNumber;
            cmd.Parameters.Add("@q1", System.Data.SqlDbType.VarChar).Value = this.Q1;
            cmd.Parameters.Add("@q2", System.Data.SqlDbType.VarChar).Value = this.Q2;
            cmd.Parameters.Add("@q3", System.Data.SqlDbType.VarChar).Value = this.Q3;
            cmd.Parameters.Add("@q4", System.Data.SqlDbType.VarChar).Value = this.Q4;
            cmd.Parameters.Add("@q5", System.Data.SqlDbType.VarChar).Value = this.Q5;
            cmd.Parameters.Add("@q6", System.Data.SqlDbType.VarChar).Value = this.Q6;
            cmd.Parameters.Add("@q7", System.Data.SqlDbType.VarChar).Value = this.Q7;
            cmd.Parameters.Add("@q8", System.Data.SqlDbType.VarChar).Value = this.Q8;
            cmd.Parameters.Add("@q9", System.Data.SqlDbType.VarChar).Value = this.Q9;
            cmd.Parameters.Add("@q10", System.Data.SqlDbType.VarChar).Value = this.Q10;
            cmd.Parameters.Add("@q11", System.Data.SqlDbType.VarChar).Value = this.Q11;
            cmd.Parameters.Add("@q12", System.Data.SqlDbType.VarChar).Value = this.Q12;
            cmd.Parameters.Add("@q13", System.Data.SqlDbType.VarChar).Value = this.Q13;
            cmd.Parameters.Add("@q14", System.Data.SqlDbType.VarChar).Value = this.Q14;
            cmd.Parameters.Add("@q15", System.Data.SqlDbType.VarChar).Value = this.Q15;
            cmd.Parameters.Add("@q16", System.Data.SqlDbType.VarChar).Value = this.Q16;
            cmd.Parameters.Add("@q17", System.Data.SqlDbType.VarChar).Value = this.Q17;
            cmd.Parameters.Add("@q18", System.Data.SqlDbType.VarChar).Value = this.Q18;
            cmd.Parameters.Add("@q19", System.Data.SqlDbType.VarChar).Value = this.Q19;
            cmd.Parameters.Add("@q20", System.Data.SqlDbType.VarChar).Value = this.Q20;
            cmd.Parameters.Add("@q21", System.Data.SqlDbType.VarChar).Value = this.Q21;
            cmd.Parameters.Add("@q22", System.Data.SqlDbType.VarChar).Value = this.Q22;
            cmd.Parameters.Add("@q23", System.Data.SqlDbType.VarChar).Value = this.Q23;
            cmd.Parameters.Add("@q24", System.Data.SqlDbType.VarChar).Value = this.Q24;
            cmd.Parameters.Add("@q25", System.Data.SqlDbType.VarChar).Value = this.Q25;
            cmd.Parameters.Add("@q26", System.Data.SqlDbType.VarChar).Value = this.Q26;
            cmd.Parameters.Add("@q27", System.Data.SqlDbType.VarChar).Value = this.Q27;
            cmd.Parameters.Add("@q28", System.Data.SqlDbType.VarChar).Value = this.Q28;
            cmd.Parameters.Add("@q29", System.Data.SqlDbType.VarChar).Value = this.Q29;
            cmd.Parameters.Add("@q30", System.Data.SqlDbType.VarChar).Value = this.Q30;
            cmd.Parameters.Add("@q31", System.Data.SqlDbType.VarChar).Value = this.Q31;
            cmd.Parameters.Add("@q32", System.Data.SqlDbType.VarChar).Value = this.Q32;
            cmd.Parameters.Add("@q33", System.Data.SqlDbType.VarChar).Value = this.Q33;
            cmd.Parameters.Add("@q34", System.Data.SqlDbType.VarChar).Value = this.Q34;
            cmd.Parameters.Add("@q35", System.Data.SqlDbType.VarChar).Value = this.Q35;
            cmd.Parameters.Add("@q36", System.Data.SqlDbType.VarChar).Value = this.Q36;
            cmd.Parameters.Add("@q37", System.Data.SqlDbType.VarChar).Value = this.Q37;
            cmd.Parameters.Add("@q38", System.Data.SqlDbType.VarChar).Value = this.Q38;
            cmd.Parameters.Add("@q39", System.Data.SqlDbType.VarChar).Value = this.Q39;
            cmd.Parameters.Add("@KC", System.Data.SqlDbType.Int).Value = this.KCount;
            cmd.Parameters.Add("@AC", System.Data.SqlDbType.Int).Value = this.ACount;
            cmd.Parameters.Add("@SC", System.Data.SqlDbType.Int).Value = this.SCount;
            cmd.Parameters.Add("@EC", System.Data.SqlDbType.Int).Value = this.ECount;
            cmd.Parameters.Add("@KP", System.Data.SqlDbType.Float).Value = this.KPercentage;
            cmd.Parameters.Add("@AP", System.Data.SqlDbType.Float).Value = this.APercentage;
            cmd.Parameters.Add("@SP", System.Data.SqlDbType.Float).Value = this.SPercentage;
            cmd.Parameters.Add("@EP", System.Data.SqlDbType.Float).Value = this.EPercentage;
            cmd.Parameters.Add("@DT", System.Data.SqlDbType.VarChar).Value = this.DominantType;
        }
    }
}
