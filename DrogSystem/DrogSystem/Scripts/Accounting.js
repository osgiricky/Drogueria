$(document).ready(function () {
    loadData();
});


//Load Data function  
function loadData() {
    $.ajax({
        url: "/Accountings/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.FechaCierre + '</td>';
                html += '<td>' + item.Ingresos + '</td>';
                html += '<td>' + item.Egresos + '</td>';
                html += '<td>' + item.BaseCaja + '</td>';
                //html += '<td><center><a href="#" onclick="return getbyID(' + item.ContabilidadId + ')">Detalles</a></center></td>';
                html += '</tr>';
            });
            $('#maestro').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Add() {
    var entryObj = {
        FechaCierre: $('#FechaCierre').val(),
        Ingresos: currencyANumero($('#Ingresos').val()),
        Egresos: currencyANumero($('#Egresos').val()),
        BaseCaja: $('#BaseCaja').val(),
    };
    $.ajax({
        url: "/Accountings/Crear",
        data: JSON.stringify(entryObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Probar == true) {
                swal.fire({
                    title: "Estimado Usuario",
                    text: response.Mensaje,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    icon: "success",
                    closeOnConfirm: false
                }).then((result) => {
                    loadData();
                    $('#myModal').modal('hide');
                    $('.modal-backdrop').remove();
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    var resp = ValidarCierre();
    if (resp == false) {
        return false;
    }
    $.ajax({
        url: "/Accountings/DatosCierre",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#FechaCierre').val(result.FechaCierre);
            $('#Ingresos').val(currencyFormat(result.Ingresos.toString()));
            $('#Egresos').val(currencyFormat(result.Egresos.toString()));
            $('#BaseInicial').val(currencyFormat(result.BaseInicial.toString()));
            if (result.BaseInicial == 0)
                document.getElementById("BaseInicial").disabled = false;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $('#myModal').modal('show');
}


function currencyFormat(num) {
    return num.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

function currencyANumero(num) {
    num = num.replace("$", "");
    num = num.replace(",", "");
    return num;
}

  
function ValidarCierre() {
    var isValid = true;
    $.ajax({
        url: "/Accountings/ValidarCierre",
        type: "POST",
        async: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == true) {
                isValid = false;
                swal.fire({
                    title: "Estimado Usuario",
                    text: "Cierre diario ya fue realizado.",
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    icon: "error",
                    closeOnConfirm: false
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return isValid;
}

