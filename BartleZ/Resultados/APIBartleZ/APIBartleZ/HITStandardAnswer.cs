namespace APIBartleZ
{
    public class HITStandardAnswer
    {
        public string hit {  get; set; }
        public string answer { get; set; }

        public HITStandardAnswer(string hit, string answer)
        {
            this.hit = hit;
            this.answer = answer;
        }
    }
}
