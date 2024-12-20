using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using MWR.Class;
using MWR.DAL;
using MWR.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Web.Mvc;

namespace MWR.Controllers
{
    public class ReportController : Controller
    {
        CUtils _cu = new CUtils();
        COracle oDB = new COracle();
        MWR_Context ldb = new MWR_Context();

        /// GET: Report
        /// <summary>
        /// This method sends the correct view. If the user is authenticated, is relocated to
        /// the report view, but if it's not authenticated, it is relocated to index in home 
        /// controller (login view).
        /// </summary>
        /// <returns>Returns a view.</returns>
        public ActionResult Index() {

            if (User.Identity.IsAuthenticated) {

                setViewData(getViewData(DateTime.Now.Year, DateTime.Now.Month - 1));
                ViewData["UserD"] = (validate_user_Result)Session["User"];
                setListBox();

                return View();
            } else {
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// This method is called when the user want to see another report that is not the current one.
        /// To get it, it is necessary send two int values, one for the year and another for the month.
        /// It's intended to be called via an Ajax function.
        /// </summary>
        /// <returns>All necessary data to make a report but in format JSON.</returns>
        [HttpPost]
        public JsonResult getAnotherReport() {
            int month, year;

            int.TryParse(HttpContext.Request.Params["year-select"], out year);
            int.TryParse(HttpContext.Request.Params["month-select"], out month);

            List<Object> datos = getViewData(year, month);
            setListBox();

            List<JsonResult> result = new List<JsonResult>();

            result.Add(Json((StoreIn)datos.ElementAt(0)));
            result.Add(Json((MaterialOut)datos.ElementAt(1)));
            result.Add(Json((FGInfo)datos.ElementAt(2)));
            result.Add(Json(datos.ElementAt(3)));

            return Json(result);
        }

        /// <summary>
        /// This method is responsible to generate a PDF document with information of the report (table). For achieving this, 
        /// it is necessary passing some strings in JSON format.
        /// </summary>
        /// <param name="storeIn">It's a JSON variable that contains all attributes for a StoreIn variable.</param>
        /// <param name="mout">It's a JSON variable that contains all attributes for a MaterialOut variable.</param>
        /// <param name="fgInfo">It's a JSON variable that contains all attributes for a FGInfo variable.</param>
        /// <param name="date">It's a string variable that contains information about the date of </param>
        /// <returns>This method returns a PDF document, which is downloaded automatically.</returns>
        public ActionResult downloadR(String storeIn, String mout, String fgInfo, String date)
        {
            StoreIn stIN = JsonSerializer.Deserialize<StoreIn>(storeIn);
            MaterialOut mOUT = JsonSerializer.Deserialize<MaterialOut>(mout);
            FGInfo fgINFO = JsonSerializer.Deserialize<FGInfo>(fgInfo);
            String html = _cu.getPDFFormat(stIN, mOUT, fgINFO, date);

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(html);
                Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 30f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                date = date.Replace(',', '_');

                return File(stream.ToArray(), "application/pdf", "[MWR] - " + date + ".pdf");
            }
        }


        /// <summary>
        /// This function is responsible to get all data from oracle to put it on the report.
        /// </summary>
        /// <param name="year">Int value that represents the year of the report consulted</param>
        /// <param name="month">Int value that represents the month of the report consulted</param>
        /// <returns>A list of objects that containing data to be displayed in the report</returns>
        private List<Object> getViewData(int year, int month) {

            List<Object> datos = new List<Object>();
            StoreIn st = new StoreIn();
            MaterialOut mout = new MaterialOut();
            FGInfo fg = new FGInfo();

            //STEP 1: GET ID's TO PERFORM THE TRANSACTION
            int nextMonth = _cu.correctMonth(month + 1);
            int nextYear = _cu.correctYear(year, month);

            Reportes cl1 = getID(month, year, 1);
            Reportes cl2 = getID(nextMonth, nextYear, 2);

            //STEP 1.1: Store the new gotten ID.
            saveKey(cl1);
            saveKey(cl2);

            //STEP 1.2: Determine if there is, at least, a data with the same ID
            bool ask = searchDataR(cl1.ID, ref st, ref mout, ref fg);

            if (!ask)  {
                int res;
                //STEP 2: GET ALL STORE IN DATA.
                //STEP 2.1 - Get Sub-Inventory In Data
                int d1, d2, d3, d4;
                d1 = d2 = d3 = d4 = 0;

                String date1 = _cu.getD_Format("01", month, year);
                String date2 = _cu.getD_Format("01", nextMonth, nextYear);
                res = oDB.getStInData(date1, date2, ref d1, ref d2, ref d3, ref d4);

                int error = 0 / res;

                //STEP 2.2 - Save data in the local DB
                st = new StoreIn(d3, d2, d1, d4, cl1.ID);
                saveStoreIn(st);

                //STEP 3: GET ALL MATERIAL OUT DATA.
                //STEP 3.1: Get picking both SMT and ASSY
                int d5, d6;
                d5 = d6 = 0;

                oDB.getPicking(date1, date2, "SMT1", ref d5);
                oDB.getPicking(date1, date2, "ASSY", ref d6);

                //STEP 3.2: Get free pick and Sub Inventory 
                int d7, d8;
                d7 = d8 = 0;

                oDB.FPandSI(cl1.TransID, cl2.TransID, "SMT1", ref d7);
                oDB.FPandSI(cl1.TransID, cl2.TransID, "ASSY", ref d8);

                //STEP 2.4: Get Total Picking for Production
                int d9 = d5 + d6 + d7 + d8;

                //STEP 3.3: Get Tray CONS Transactions
                int d10 = 0;
                oDB.TConsT(cl1.TransID, cl2.TransID, ref d10);

                //STEP 3.4: Get Return transaction SMT -> WH
                int d11 = 0;

                //STEP 3.5: Get the total of transactions of this section
                int d12 = d9 + d10 + d11;

                //STEP 4.6 - Save Material Out Information
                mout = new MaterialOut(d7, d8, d5, d6, d9, d10, d11, d12, cl1.ID);
                saveMOut(mout);

                //STEP 4: GET FG INFORMATION
                //STEP 4.1: Get OWH-In data
                int d13 = 0;
                oDB.Owh_In(date1, date2, ref d13);

                //STEP 4.2 - Get OWH-Out
                int d14 = 0;
                oDB.Owh_Out(date1, date2, ref d14);

                //STEP 4.3 - Save FG Information 
                fg = new FGInfo(d13, d14, cl1.ID);
                saveFG(fg);
            }

            datos.Add(st);
            datos.Add(mout);
            datos.Add(fg);
            datos.Add(_cu.translation(_cu.translation(month)) + ", " + year);

            return datos;
        }


        /// <summary>
        /// Set objects in the respective viewdata.
        /// </summary>
        /// <param name="data">
        /// List of objects that has to contains an instance 
        /// of StoreIn, another one of MaterialOut, one of FGInfo and one of string
        /// </param>
        private void setViewData(List<Object> data) {

            StoreIn storeIn = (StoreIn)data.ElementAt(0);
            ViewData["StoreIn"] = storeIn;

            MaterialOut mout = (MaterialOut)data.ElementAt(1);
            ViewData["Material_Out"] = mout;

            FGInfo fg = (FGInfo)data.ElementAt(2);
            ViewData["FG"] = fg;

            ViewData["Date"] = (String)data.ElementAt(3);
        }

        /// <summary>
        /// This function is in charge to set up select-elements into the view
        /// </summary>
        private void setListBox() {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectListItem op1 = new SelectListItem();
            op1.Text = op1.Value = "Choose one...";

            list.Add(op1);

            ViewData["year-select"] = list;
            ViewData["month-select"] = list;
        }

        /// <summary>
        /// This method is responsible to get an ID, which is going to be used
        /// in the data extraction process. First, checks, using a number of month
        /// and another for the year, if it is stored in our local database. Otherwise,
        /// extract it from oracle.
        /// </summary>
        /// <param name="month">It's an int value, which be a value between 1 and 12</param>
        /// <param name="year">It's a four digit number.</param>
        /// <returns>An instance of Claves class which represents a month</returns>
        private Reportes getID(int month, int year, int key) {
            Reportes clave = new Reportes();
            var id = ldb.Reportes.Where(cl => cl.month == month && cl.year == year);

            if (id.Count() > 0) {
                clave = (Reportes)id.First();
            } else {
                int auxT = 0;

                //Cubren bien el primer caso
                String date1 = _cu.getD_Format("01", month, year);
                String date2 = _cu.getD_Format("02", month, year);

                oDB.getID(date1, date2, ref auxT);

                clave = new Reportes(year, month, auxT);
            }

            return clave;
        }


        /// <summary>
        /// Store a key in the local database depending if it is, or not, into the DB.
        /// </summary>
        /// <param name="cl">It represents an instance of Claves class</param>
        private void saveKey(Reportes cl) {
            var lis = ldb.Reportes.Where(s => s.month == cl.month && s.year == cl.year && s.TransID == cl.TransID);
            if (lis.Count() < 1) {
                ldb.Reportes.Add(cl);
                ldb.SaveChanges();
            }
        }

        /// <summary>
        /// This method is responsible for determining and getting report information from the local database.
        /// </summary>
        /// <param name="cl1">An int variable which is used to identify the report.</param>
        /// <param name="st">It's param passed as reference to save StoreIn info.</param>
        /// <param name="mout">It's param passed as reference to save Material Out info.</param>
        /// <param name="fg">It's param passed as reference to save FG info.</param>
        /// <returns>A bool value which indicates if there is information in our local database.</returns>
        private bool searchDataR(int ID, ref StoreIn st, ref MaterialOut mout, ref FGInfo fg ) {
            bool state = false;

            var ast = from store in ldb.StoreIns
                      where store.reporteID == ID
                      select store;

            var amout = from mater in ldb.MaterialOuts
                        where mater.reporteID == ID
                        select mater;

            var afg = from fgs in ldb.FGInfos
                      where fgs.reporteID == ID
                      select fgs;

            if (ast.Count() > 0 && amout.Count() > 0 && afg.Count() > 0)  {
                st = ast.First();
                mout = amout.First();
                fg = afg.First();
                state = true;
            }

            return state;
        }


        /// <summary>
        /// This method is in charge for saving StoreIn information into the local database.
        /// </summary>
        /// <param name="data">A StoreIn variable will be stored into the local database.</param>
        private void saveStoreIn(StoreIn data)
        {
            ldb.StoreIns.Add(data);
            ldb.SaveChanges();
        }

        /// <summary>
        /// This method is in charge for saving MaterialOut information into the local database.
        /// </summary>
        /// <param name="data">A MaterialOut variable will be stored into the local database.</param>
        private void saveMOut(MaterialOut data){
            ldb.MaterialOuts.Add(data);
            ldb.SaveChanges();
        }

        /// <summary>
        /// This method is in charge for saving FG information into the local database.
        /// </summary>
        /// <param name="data">A FGInfo variable will be stored into the local database.</param>
        private void saveFG(FGInfo data)  {
            ldb.FGInfos.Add(data);
            ldb.SaveChanges();
        }

    }
}
