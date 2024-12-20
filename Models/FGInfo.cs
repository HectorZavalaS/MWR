namespace MWR.Models
{
    public class FGInfo
    {
        public int ID { get; set; }
        public int owhIN { get; set; }
        public int owhOUT { get; set; }
        public int reporteID { get; set; }

        public FGInfo() {
            owhIN = 0;
            owhOUT = 0;
        }

        public FGInfo (int owhIn, int owhOut, int reporteID)
        {
            owhIN = owhIn;
            owhOUT = owhOut;
            this.reporteID = reporteID;
        }


        public FGInfo(int owhIn, int owhOut) {
            owhIN = owhIn;
            owhOUT = owhOut;
        }

    }
}