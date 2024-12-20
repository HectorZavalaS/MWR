namespace MWR.Models
{
    public class Reportes
    {
        public Reportes()
        {
        }

        public Reportes(int year, int month, int transID)
        {
            this.year = year;
            this.month = month;
            this.TransID = transID;
        }

        public int ID { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int TransID { get; set; }

    }
}