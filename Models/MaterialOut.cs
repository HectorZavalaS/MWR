namespace MWR.Models
{
    public class MaterialOut
    {
        public int ID { get; set; }
        public int freePSMT { get; set; }
        public int freePASSY { get; set; }
        public int smtP { get; set; }
        public int assyP { get; set; }
        public int totalPP { get; set; }
        public int TrayCons { get; set; }
        public int smtTOwh { get; set; }
        public int totalTrans { get; set; }
        public int reporteID { get; set; }

        public MaterialOut() {
            freePSMT = 0;
            freePASSY = 0;
            smtP = 0;
            assyP = 0;
            totalPP = 0;
            TrayCons = 0;
            smtTOwh = 0;
            totalTrans = 0;
            reporteID = 0;
        }

        public MaterialOut(int fpSMT, int fpASSY, int smtP, int assyP, int totalPP, int trayC, int smTOwh, int totalTrans)
        {
            freePSMT = fpSMT;
            freePASSY = fpASSY;
            this.smtP = smtP;
            this.assyP = assyP;
            this.totalPP = totalPP;
            TrayCons = trayC;
            smtTOwh = smTOwh;
            this.totalTrans = totalTrans;
        }

        public MaterialOut(int fpSMT, int fpASSY, int smtP,int assyP, int totalPP, int trayC, int smTOwh, int totalTrans, int reporteID)
        {
            freePSMT = fpSMT;
            freePASSY = fpASSY;
            this.smtP = smtP;
            this.assyP = assyP;
            this.totalPP = totalPP;
            TrayCons = trayC;
            smtTOwh = smTOwh;
            this.totalTrans = totalTrans;
            this.reporteID = reporteID;
        }

    }
}