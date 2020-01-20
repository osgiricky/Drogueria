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
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombreTercero + '</td>';
                html += '<td>' + item.Codtercero + '</td>';
                html += '<td>' + item.TipoTercero + '</td>';
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
            var html = '';
            $.each(result.ListaTipoTercero, function (key, item) {
                if (result.ProviderTypeId == item.ProviderTypeId) {
                    html += '<option value="' + item.ProviderTypeId + '" selected>' + item.TipoTercero + '</option>';
                }
                else {
                    html += '<option value="' + item.ProviderTypeId + '" >' + item.TipoTercero + '</option>';
                }
            });
            $('#ProviderTypeId').html(html);

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
    var userObj = {
        TerceroId: $('#TerceroId').val(),
        NombreTercero: $('#NombreTercero').val(),
        Codtercero: $('#Codtercero').val(),
        ProviderTypeId: $('#ProviderTypeId').val(),
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
    if ($('#ProviderTypeId value:selected').val() == "") {
        $('#ProviderTypeId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ProviderTypeId').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var userObj = {
        TerceroId: $('#TerceroId').val(),
        NombreTercero: $('#NombreTercero').val(),
        Codtercero: $('#Codtercero').val(),
        ProviderTypeId: $('#ProviderTypeId').val(),
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
            $('#ProviderTypeId').html("");
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
    $('#ProviderTypeId').html("");
    $('#NombreTercero').css('border-color', 'lightgrey');
    $('#Codtercero').css('border-color', 'lightgrey');
    $('#ProviderTypeId').css('border-color', 'lightgrey');
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Providers/listatipos/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = html += '<option value="" selected> Selecione una Opción</option>';;
            $.each(result, function (key, item) {
                html += '<option value="' + item.ProviderTypeId + '" >' + item.TipoTercero + '</option>';
            });
            $('#ProviderTypeId').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $('#myModal').modal('show');

}