using Microsoft.Data.SqlClient;

namespace APIBartleZ
{
    public class TypesofGamesPlay
    {
        public int PlayerID { get; set; }
        public string Answer { get; set; }

        public TypesofGamesPlay()
        {
            PlayerID = int.MinValue;
            Answer = string.Empty;
        }

        public TypesofGamesPlay(int playerID, string answer)
        {
            PlayerID = playerID;
            Answer = answer;
        }

        public TypesofGamesPlay ReadItem(SqlDataReader reader)
        {
            TypesofGamesPlay item = new TypesofGamesPlay();

            item.PlayerID = Convert.ToInt32(reader["PlayerID"]);
            item.Answer = Convert.ToString(reader["Answer"]);

            return item;
        }

        public void WriteItem(SqlCommand cmd)
        {
            cmd.Parameters.Add("@PID", System.Data.SqlDbType.Int).Value = this.PlayerID;
            cmd.Parameters.Add("@ans", System.Data.SqlDbType.VarChar).Value = this.Answer;
        }
    }
}
