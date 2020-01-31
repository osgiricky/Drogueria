$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Providers/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            var tercero;
            $.each(result, function (key, item) {
                tercero = '';
                if (item.TipoTercero == "P") {
                    tercero = 'Proveedor';
                }
                else if (item.TipoTercero == "O") {
                    tercero = 'Otro';;
                }
                html += '<tr>';
                html += '<td>' + item.NombreTercero + '</td>';
                html += '<td>' + item.Codtercero + '</td>';
                html += '<td>' + tercero + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.TerceroId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.TerceroId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(TerceroId) {
    document.getElementById('myModalLabel').innerHTML = "Editar Tercero";
    $('#NombreTercero').css('border-color', 'lightgrey');
    $('#Codtercero').css('border-color', 'lightgrey');
    $('#ProviderTypeId').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Providers/getbyID/" + TerceroId,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#TerceroId').val(result.TerceroId);
            $('#NombreTercero').val(result.NombreTercero);
            $('#Codtercero').val(result.Codtercero);
            if (result.TipoTercero == "P") {
                $('#O').attr('checked', false);
                $('#P').attr('checked', true);
            }
            else if (result.TipoTercero == "O") {
                $('#P').attr('checked', false);
                $('#O').attr('checked', true);
            }

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

function Delete(TerceroId) {
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
                url: "/Providers/Borrar/" + TerceroId,
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                //data: ID,
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
    var tipotercero = '';
    if (document.getElementsByName('TipoTercero')[0].checked) {
        tipotercero = 'P';
    }
    else if (document.getElementsByName('TipoTercero')[1].checked) {
        tipotercero = 'O';
    }
    var userObj = {
        TerceroId: $('#TerceroId').val(),
        NombreTercero: $('#NombreTercero').val(),
        Codtercero: $('#Codtercero').val(),
        TipoTercero: tipotercero,
    };
    $.ajax({
        url: "/Providers/Editar",
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
                $('#TerceroId').val("");
                $('#NombreTercero').val("");
                $('#Codtercero').val("");
                $('#ProviderTypeId').html("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#NombreTercero').val().trim() == "") {
        $('#NombreTercero').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NombreTercero').css('border-color', 'lightgrey');
    }
    if ($('#Codtercero').val().trim() == "") {
        $('#Codtercero').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Codtercero').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var tipotercero = '';
    if (document.getElementsByName('TipoTercero')[0].checked) {
        tipotercero = 'P';
    }
    else if (document.getElementsByName('TipoTercero')[1].checked) {
        tipotercero = 'O';
    }
    var userObj = {
        TerceroId: $('#TerceroId').val(),
        NombreTercero: $('#NombreTercero').val(),
        Codtercero: $('#Codtercero').val(),
        TipoTercero: tipotercero,
    };
    $.ajax({
        url: "/Providers/Crear",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide')
            $('#TerceroId').val("");
            $('#NombreTercero').val("");
            $('#Codtercero').val("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    document.getElementById('myModalLabel').innerHTML = "Agregar Tercero";
    $('#TerceroId').val("");
    $('#NombreTercero').val("");
    $('#Codtercero').val("");
    $('#NombreTercero').css('border-color', 'lightgrey');
    $('#Codtercero').css('border-color', 'lightgrey');
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#P').attr('checked', false);
    $('#O').attr('checked', true);
    $('#myModal').modal('show');
}