$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/PaymentProviders/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Fecha_Pago + '</td>';
                html += '<td>' + item.NombreTercero + '</td>';
                html += '<td>' + item.Valor_Pago + '</td>';
                html += '<td>' + item.Observacion + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.Id_Pago + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.Id_Pago + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(Id) {
    document.getElementById('myModalLabel').innerHTML = "Editar Pago Tercero";
    $('#Id_Pago').css('border-color', 'lightgrey');
    $('#NombreTercero').css('border-color', 'lightgrey');
    $('#Fecha_Pago').css('border-color', 'lightgrey');
    $('#Observacion').css('border-color', 'lightgrey');
    $.ajax({
        url: "/PaymentProviders/getbyID/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id_Pago').val(result.Id_Pago);
            $('#Fecha_Pago').val(result.Fecha_Pago);
            var html = '';
            $.each(result.ListaTerceros, function (key, item) {
                if (result.TerceroId == item.TerceroId) {
                    html += '<option value="' + item.TerceroId + '" selected>' + item.NombreTercero + '</option>';
                }
                else {
                    html += '<option value="' + item.TerceroId + '" >' + item.NombreTercero + '</option>';
                }
            });
            $('#TerceroId').html(html);
            $('#Valor_Pago').val(result.Valor_Pago);
            $('#Observacion').val(result.Observacion);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Delete(Id) {
    Swal.fire({
        title: "Estimado Usuario",
        text: "Esta seguro(a) que desea eliminar registro?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: '#d33',
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.value == true) {
            $.ajax({
                url: "/PaymentProviders/Borrar/" + Id,
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Probar == false) {
                        swal.fire({
                            title: "Estimado Usuario",
                            text: response.Mensaje,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",
                            icon: "error",
                            closeOnConfirm: false
                        });
                    }
                    else {
                        Swal.fire({
                            title: "Estimado Usuario",
                            text: response.Mensaje,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",
                            icon: "success",
                            closeOnConfirm: false
                        });
                    }
                    loadData();
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        }
    })
}
function Update() {
    var res = validate();
    if (res == false) {
        swal.fire({
            title: "Estimado Usuario",
            text: "Los campos marcados en rojo son obligatorios.",
            icon: 'warning',
            showCancelButton: false,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Aceptar",
        });
        return false;
    }
    var userObj = {
        Id_Pago: $('#Id_Pago').val(),
        Valor_Pago: $('#Valor_Pago').val(),
        Fecha_Pago: $('#Fecha_Pago').val(),
        Observacion: $('#Observacion').val(),
        TerceroId: $('#TerceroId').val(),
    };
    $.ajax({
        url: "/PaymentProviders/Editar",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Probar == false) {
                swal.fire({
                    title: "Estimado Usuario",
                    text: response.Mensaje,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    icon: "error",
                    closeOnConfirm: false
                });
            }
            else {
                loadData();
                $('#myModal').modal('hide');
                $('#Id_Pago').val("");
                $('#Valor_Pago').val("");
                $('#Fecha_Pago').val("");
                $('#Observacion').html("");
                $('#TerceroId').html("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#Fecha_Pago').val().trim() == "") {
        $('#Fecha_Pago').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Fecha_Pago').css('border-color', 'lightgrey');
    }
    if ($('#TerceroId value:selected').val() == "") {
        $('#TerceroId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TerceroId').css('border-color', 'lightgrey');
    }
    if ($('#Valor_Pago').val().trim() == "") {
        $('#Valor_Pago').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Valor_Pago').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var userObj = {
        Id_Pago: $('#Id_Pago').val(),
        Valor_Pago: $('#Valor_Pago').val(),
        Fecha_Pago: $('#Fecha_Pago').val(),
        Observacion: $('#Observacion').val(),
        TerceroId: $('#TerceroId').val(),
    };
    $.ajax({
        url: "/PaymentProviders/Crear",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide')
            $('#Id_Pago').val("");
            $('#Valor_Pago').val("");
            $('#Fecha_Pago').val("");
            $('#Observacion').val("");
            $('#TerceroId').html("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    document.getElementById('myModalLabel').innerHTML = "Agregar Pago Tercero";
    $('#Id_Pago').val("");
    $('#Valor_Pago').val("");
    $('#Fecha_Pago').val("");
    $('#Observacion').val("");
    $('#TerceroId').html("");
    $('#Valor_Pago').css('border-color', 'lightgrey');
    $('#Fecha_Pago').css('border-color', 'lightgrey');
    $('#TerceroId').css('border-color', 'lightgrey');
    $('#btnUpdate').hide();
    $('#Id_Pago').hide("");
    $('#IdPagoText').hide("");
    $('#btnAdd').show();
    $.ajax({
        url: "/PaymentProviders/listaTerceros/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = html += '<option value="" selected> Selecione una Opción</option>';;
            $.each(result, function (key, item) {
                html += '<option value="' + item.TerceroId + '" >' + item.NombreTercero + '</option>';
            });
            $('#TerceroId').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $('#myModal').modal('show');

}