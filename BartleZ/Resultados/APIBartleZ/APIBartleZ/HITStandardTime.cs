namespace APIBartleZ
{
    public class HITStandardTime
    {
        public string hit {  get; set; }
        public float timeseconds { get; set; }

        public HITStandardTime(string hit, float timeseconds)
        {
            this.hit = hit;
            this.timeseconds = timeseconds;
        }
    }
}
