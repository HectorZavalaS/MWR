namespace MWR.Models
{
    public class StoreIn
    {
        public int ID { get; set; }
        public int receiving { get; set; }
        public int pck { get; set; }
        public int chinaM { get; set; }
        public int total { get; set; }
        public int reporteID { get; set; }

        public StoreIn() {
            receiving = 0;
            pck = 0;
            chinaM = 0;
            total = 0;
            reporteID = 0;
        }

        public StoreIn(int receiving, int pck, int chinaM, int total)
        {
            this.receiving = receiving;
            this.pck = pck;
            this.chinaM = chinaM;
            this.total = total;
        }

        public StoreIn(int receiving, int pck, int chinaM, int total, int reporteID)
        {
            this.receiving = receiving;
            this.pck = pck;
            this.chinaM = chinaM;
            this.total = total;
            this.reporteID = reporteID;
        }

    }
}