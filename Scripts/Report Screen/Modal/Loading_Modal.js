/*
 * This Script is in charge for extracting data for other report, using an Ajax function.
 * This function is going to be activated only when the user clicks on the button "btn-find",
 * which is enabled until the user entered a validate date for this form. That's the way
 * we can be sure that user is not going use this function badly.
 * 
 * First, it's necessary to avoid activating the POST method which is in the form 
 * element. This is because we don't want to send the same data twice. To get this, we
 * use the following method: e.preventDefault(), it doesn't need any params in this case.
 * Although we put this code line in the end, it's important knowing it from the beggining.
 *
 * Then, we extract values both year-select and month-select elements, and store them in
 * year and month variables respectively. 
 * 
 * The Ajax function needs following params:
 *      type: represents the type of method that we are going to use for this function 
 *            (it can be POST or GET).
 *      url: in this space, we type the url, but in our case, we are going to use the 
 *           same url as in the form action.
 *      data: here we put our params, which were extracted from select elements. But these
 *            params need to be sent as a string with their respective names and separated
 *            by an ampersand.
 *            
 * When an ajax function is executed we can use several internal functions, to handle 
 * each process (before, during, and after the execution). In our case, we selected four
 * moments/events to perform specific actions:
 *      beforeSend: event that ocurrs before sending the data to the respective URL (controller).
 *                  In our case, we use it to show static-loading-img modal and to hide
 *                  F-REPORT modal, with help of modal method, which need just a param.
 *                  
 *      success: this event takes place when the controller has finished to process the 
 *               request correctly. In this event, we can handle the response that server sends.
 *               In our case, server response sends new values to the report, which 
 *               will be used to replace the old values.
 *               
 *      error:  it occurs when the request has a problem to be done. It could be caused by
 *              a server problem/error. In this case, we just close the static-loading-img
 *              modal and show the error on console.
 *      
 *      complete: this event takes place when the controller has finished to process the
 *                request, regardless of whether if it was successful or there was an
 *                error in the process. We added a timeout function, due to the process to get information
 *                from the database can take a really short time span, causing that can't close
 *                loading-modal element
 *                
 *      
 */

$(document).ready(function () {

    $("#btn-find").click(function (e) {
        
        var year = $("#year-select").val();
        var month = $("#month-select").val();

        $.ajax({ 
            type: 'POST',
            url: '/Report/getAnotherReport/',
            data: 'year-select=' + year + '&month-select=' + month,
            //Close form modal and open loading modal
            beforeSend: function () {
                $("#F-REPORT").modal("toggle");

                $("#static-loading-img").modal("toggle");
            },
            //Set the values in respectives fields
            success: function (response) {
                const formatT = new Intl.NumberFormat("en-US");

                $("#store-1").text(formatT.format(response[0].Data.receiving) );
                $("#store-2").text(formatT.format(response[0].Data.pck) );
                $("#store-3").text(formatT.format(response[0].Data.chinaM));
                $("#store-4").text(formatT.format(response[0].Data.total));

                $("#mout-1").text(formatT.format(response[1].Data.freePSMT) );
                $("#mout-2").text(formatT.format(response[1].Data.smtP) );
                $("#mout-3").text(formatT.format(response[1].Data.assyP));
                $("#mout-4").text(formatT.format(response[1].Data.freePASSY));
                $("#mout-5").text(formatT.format(response[1].Data.totalPP));
                $("#mout-6").text(formatT.format(response[1].Data.TrayCons));
                $("#mout-7").text(formatT.format(response[1].Data.smtTOwh));
                $("#mout-8").text(formatT.format(response[1].Data.totalTrans));

                $("#FG-1").text(formatT.format(response[2].Data.owhIN) );
                $("#FG-2").text(formatT.format(response[2].Data.owhOUT) );

                $("#date-month").text(response[3].Data);
                
            },
            //Close loading modal
            complete: function () {
                setTimeout(function(){
                    $("#static-loading-img").modal("toggle");
                }, 500);                
                //Refresh values
                $("#year-select").val("Choose one...").trigger("change");
            },
            //Show modal error
            error: function (e) {
                console.log("Error:" + e.responseText);
                $("#Error-Modal").modal("toggle");
            }
        });

        e.preventDefault();
    });
});