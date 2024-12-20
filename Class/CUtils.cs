using MWR.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MWR.Class
{
    public class CUtils {

        private enum months {
            Ene = 1, Feb = 2, Mar = 3, Abr = 4, May = 5, Jun = 6, 
            Jul = 7, Ago = 8, Sep = 9, Oct = 10, Nov = 11, Dec = 12
        }

        public CUtils() { }
        

        /// <summary>
        /// This method is used to turn a string into a another different string
        /// (an abbreviated month-name turns into a full month-name).
        /// </summary>
        /// <param name="month">It needs to be an abbreviated spanish name of a month.</param>
        /// <example>"Ene" turns into "January"</example>
        /// <returns>It returns a full english name of a month</returns>
        public String translation(String month)  {
            month = month.Replace("Ene", "January");
            month = month.Replace("Feb", "February");
            month = month.Replace("Mar", "March");
            month = month.Replace("Abr", "April");
            month = month.Replace("May", "May");
            month = month.Replace("Jun", "June");
            month = month.Replace("Jul", "July");
            month = month.Replace("Ago", "August");
            month = month.Replace("Sep", "September");
            month = month.Replace("Oct", "October");
            month = month.Replace("Nov", "November");
            month = month.Replace("Dec", "December");

            return month ;
        }

        /// <summary>
        /// This function turns a number into a name of a month.
        /// </summary>
        /// <param name="mnth">A value between 1 and 12</param>
        /// <returns>A string whose value is the name of a month</returns>
        public String translation(int mnth) {
            return ((months)mnth).ToString();
        }

        /// <summary>
        /// This fucntion receives an int value that represents a four digit year, which is
        /// going to modify to be a two digit number (just last two digits). 
        /// </summary>
        /// <param name="year">Int value, it need higher than 1000 </param>
        /// <returns>A string that represents two last digits in the number entered.</returns>
        public String yearFormat(int year)  {
            String newYear = year.ToString();
            char[] yearAux = newYear.ToArray();

            newYear = yearAux[yearAux.Length - 2].ToString() + yearAux[yearAux.Length - 1];

            return newYear;
        }


        /// <summary>
        /// This function write a date in the correct format, for example: "01-Jun-23".
        /// </summary>
        /// <param name="day">It's a string that could be "01" or "02"</param>
        /// <param name="month">An int value between 1 and 12 that represents a month in our calendar</param>
        /// <param name="year">A four digit number that represents a year</param>
        /// <returns>Returns a string that represents a date with the correct format</returns>
        /// <example>
        /// Correct format:  "01-Jun-23"
        /// <paramref name="day"/> "01"
        /// <paramref name="month"/> 6
        /// <paramref name="year"/>2023
        /// </example>
        public String getD_Format(String day, int month, int year){

            String final_date = day + "-" + translation(month) + "-" + yearFormat(year);

            return final_date;
        }

        /// <summary>
        /// This function determines if the number that represents the month is out of the range,
        /// in which case it is necessary getting it back into the range.
        /// In brief, it allows us have control on the data that represents a month.
        /// </summary>
        /// <param name="month">Int value it could be between 1 and 13</param>
        /// <returns>
        /// If the number is equal to 13 return a 1, but if the number is not, returns 
        /// the same number that received.
        /// </returns>
        public int correctMonth(int month) {
            if(month == 13) {
                month = 1;
            }

            return month;
        }

        /// <summary>
        /// This function is responible to add a year if the current month is 12 (december)
        /// </summary>
        /// <param name="year">A four digit number</param>
        /// <param name="month">A number between 1 and 12</param>
        /// <returns>
        /// If the month is 12, the year variable is increased by one. Otherwise, returns the same value
        /// </returns>
        public int correctYear(int year, int month) {
            if(month == 12) {
                year++;
            }

            return year;
        }

        public void interChangeStrings(ref String date1, ref String date2)
        {
            String aux;

            aux = date1;
            date1 = date2;
            date2 = aux;
        }


        public String getPDFFormat(StoreIn st, MaterialOut mout, FGInfo fg, String date)
        {
            String html = "<body>" +
                "<h1 style='text-align: center; margin-bottom: 50px;'>Monthly Warehouse Report</h1>" +
                "<table style='font-family: Arial;border: 5px solid white; width: 100%;'>" +

                "<tr style='color:white;border: 5px solid white; height: 60px;' >" +
                "   <th colspan='3' style='background: rgb(130, 142, 246);width: 100%;text-align: center; font-size: 20pt;padding: 5px;'>" + date+"</th>" +
                "</tr>" +

                "<tr style='text-align: center; background: rgb(193, 215, 249); font-size: 16pt; color: white; font-weight: 600; height: 40px;'>" +
                "   <td  >Process </td> " +
                "   <td >Transactions</td> " +
                "   <td >Batch Number</td></tr>" +

                "<tr style='background: rgb(223, 227, 255); color: black;font-size: 13pt; height: 30px;'>" +
                "   <td  >Store In </td>" +
                "   <td  >Receiving </td>" +
                "   <td  > "+st.receiving+" </td> " +
                "</tr>" +

                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td  > </td>" +
                "   <td  >PCK (Tray)</td>" +
                "   <td  > "+st.pck+" </td> " +
                "</tr>" +

                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td > China Material </td>" +
                "   <td > "+st.chinaM+" </td> " +
                "</tr>" +

                "<tr style='background: rgb(193, 215, 249); height: 30px;' >" +
                "   <td ></td>" +
                "   <td></td>" +
                "   <td style='color: black; font-weight: bold; font-size: 14pt;' >" + st.total+"</td>" +
                "</tr>" +

                //MATERIAL OUT
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt;height: 30px;'>" +
                "   <td > Material Out</td>" +
                "   <td >Free Pick & Sub Inventory SMT</td>" +
                "   <td > "+mout.freePSMT+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td >SMT Picking </td>" +
                "   <td > "+mout.smtP+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td >ASSY Picking </td>" +
                "   <td > "+mout.assyP+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td >Free Pick & Sub Inventory ASSY</td>" +
                "   <td > "+mout.freePASSY+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td >Total Picking for Production</td>" +
                "   <td > "+mout.totalPP+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt;height: 30px;'>" +
                "   <td > </td>" +
                "   <td >Tray CONS Transactions</td>" +
                "   <td > "+mout.TrayCons+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td >Return Transaction SMT -> WH</td>" +
                "   <td > "+mout.smtTOwh+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(193, 215, 249); height: 30px;' >" +
                "   <td ></td>" +
                "   <td></td>" +
                "   <td style='color: black; font-weight: bold; font-size: 14pt;' >"+mout.totalTrans+"</td>" +
                "</tr>" +

                //FG ZONE
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > FG </td>" +
                "   <td >OWH - IN</td>" +
                "   <td > "+fg.owhIN+" </td> " +
                "</tr>" +
                "<tr style='background: rgb(223, 227, 255);color: black;font-size: 13pt; height: 30px;'>" +
                "   <td > </td>" +
                "   <td > OWH - OUT </td>" +
                "   <td > "+fg.owhOUT+" </td> " +
                "</tr>" +

                "</table></body>";

            return html;
        }


    }
}