using System.Web;
using System.Web.Optimization;

namespace MWR
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/LoginJS").Include(
                        "~/Scripts/Log-In Screen/Cha_Img.js"));

            bundles.Add(new ScriptBundle("~/bundles/Text_LoginJS").Include(
                        "~/Scripts/Log-In Screen/Change_Color_W.js"));

            bundles.Add(new ScriptBundle("~/bundles/Clean_Modal").Include(
                        "~/Scripts/Report Screen/Modal/Clean_Modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/Add_Year").Include(
                        "~/Scripts/Report Screen/Modal/Add_Years.js"));

            bundles.Add(new ScriptBundle("~/bundles/Add_Month").Include(
                        "~/Scripts/Report Screen/Modal/Add_Month.js"));

            bundles.Add(new ScriptBundle("~/bundles/Enabled_Btn").Include(
                        "~/Scripts/Report Screen/Modal/Enabled_Btn_Find.js"));

            bundles.Add(new ScriptBundle("~/bundles/ListBox_Setup").Include(
                        "~/Scripts/Report Screen/Modal/ListBox_Setup.js"));

            bundles.Add(new ScriptBundle("~/bundles/Loading_Modal").Include(
                        "~/Scripts/Report Screen/Modal/Loading_Modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/download_PDF").Include(
                        "~/Scripts/Report Screen/Download_PDF.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"
                      ));

            /*Styles for Log In Screen*/
            bundles.Add(new StyleBundle("~/Content/LogIn/css").Include(
                "~/Content/Log-In Screen/Log-In.css"));

            /*Styles for Report Screen*/
            bundles.Add(new StyleBundle("~/Content/Report/css").Include(
                "~/Content/Report Screen/Modal-FR.css",
                "~/Content/Report Screen/Navbar.css",
                "~/Content/Report Screen/Table.css",
                "~/Content/Report Screen/Modal_Load.css",
                "~/Content/Report Screen/printPDF.css"));

            /*Styles for Error Screen*/
            bundles.Add(new StyleBundle("~/Content/Error/css").Include(
                "~/Content/Error_Screen.css"));
        }
    }
}
