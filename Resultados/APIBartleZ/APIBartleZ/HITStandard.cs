using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APIBartleZ
{
    public class HITStandard
    {
        public int playerNumber { get; set; }
        public List<HITStandardAnswer> answers { get; set; }
        public List<HITStandardTime> times { get; set; }

        public HITStandard(int playerNumber, List<HITStandardAnswer> answers, List<HITStandardTime> times)
        {
            this.playerNumber = playerNumber;
            this.answers = answers;
            this.times = times;
        }
    }
}
