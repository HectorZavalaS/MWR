/*
 * This function is responsible to extract data from each field in the report table, 
 * to set them in their respective inputs.
 * This information is going to be used to make the report in PDF format.
 */

$(document).ready(function () {
    $("#btn-dwld").click(function (e) {
        console.log("Vine");

        var datos = document.getElementsByClassName("batch-table-data");

        var storeIn = {
            "receiving": parseInt(datos[0].textContent.replace(',', "")),
            "pck": parseInt(datos[1].textContent.replace(',', "")),
            "chinaM": parseInt(datos[2].textContent.replace(',', "")),
            "total": parseInt(datos[3].textContent.replace(',', ""))
        };

        var materialOut = {
            "freePSMT": parseInt(datos[4].textContent.replace(',', "")),
            "smtP": parseInt(datos[5].textContent.replace(',', "")),
            "assyP": parseInt(datos[6].textContent.replace(',', "")),
            "freePASSY": parseInt(datos[7].textContent.replace(',', "")),
            "totalPP": parseInt(datos[8].textContent.replace(',', "")),
            "TrayCons": parseInt(datos[9].textContent.replace(',', "")),
            "smtTOwh": parseInt(datos[10].textContent.replace(',', "")),
            "totalTrans": parseInt(datos[11].textContent.replace(',', ""))
        };

        var FGInfo = {
            "owhIN": parseInt(datos[12].textContent.replace(',', "")),
            "owhOUT": parseInt(datos[13].textContent.replace(',', "")),
        };

        $("input[name='date']").val($("#date-month").text());
        $("input[name='storeIn']").val(JSON.stringify(storeIn));
        $("input[name='mout']").val(JSON.stringify(materialOut));
        $("input[name='fgInfo']").val(JSON.stringify(FGInfo));
    });
});


/*
 * This is anoter way to make a PDF, but this statements print the whole active screen/window, so some CSS Rules are lost in the process,
 * causing some tittles hide.
 

 * <script>
        $(document).ready(function () {
            $("#btn-dwld").on("click", function (e) {

                $("#message-report").hide();
                $("#btn-dwld").hide();
                $("#img-logo-nav").hide();

                window.print();

                $("#message-report").show();
                $("#btn-dwld").show();
                $("#img-logo-nav").show();
            });
        });
    </script>
 */