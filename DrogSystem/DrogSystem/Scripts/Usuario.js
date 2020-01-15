$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Users/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.Nombre + '</td>';
                html += '<td>' + item.CodUsuario + '</td>';
                html += '<td>' + item.Descripcion + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.UsuarioId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.UsuarioId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(UsuarioId) {
    $('#Nombre').css('border-color', 'lightgrey');
    $('#Clave').css('border-color', 'lightgrey');
    $('#CodUsuario').css('border-color', 'lightgrey');
    $('#TipoUsuarioId').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Users/getbyID/" + UsuarioId,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#UsuarioId').val(result.UsuarioId);
            $('#Nombre').val(result.Nombre);
            $('#Clave').val(result.Clave);
            $('#CodUsuario').val(result.CodUsuario);
            var html = '';
            $.each(result.ListaTipoUsuario, function (key, item) {
                if (result.TipoUsuarioId == item.TipoUsuarioId) {
                    html += '<option value="' + item.TipoUsuarioId + '" selected>' + item.Descripcion + '</option>';
                }
                else {
                    html += '<option value="' + item.TipoUsuarioId + '" >' + item.Descripcion + '</option>';
                }
            });
            $('#TipoUsuarioId').html(html);

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

function Delete(UsuarioId) {
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
                url: "/Users/Borrar/" + UsuarioId,
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
        UsuarioId: $('#UsuarioId').val(),
        CodUsuario: $('#CodUsuario').val(),
        Nombre: $('#Nombre').val(),
        Clave: $('#Clave').val(),
        TipoUsuarioId: $('#TipoUsuarioId').val(),
    };
    $.ajax({
        url: "/Users/Editar",
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
                $('#UsuarioId').val("");
                $('#Nombre').val("");
                $('#Clave').val("");
                $('#CodUsuario').val("");
                $('#TipoUsuarioId').html("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#CodUsuario').val().trim() == "") {
        $('#CodUsuario').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#CodUsuario').css('border-color', 'lightgrey');
    }
    if ($('#Nombre').val().trim() == "") {
        $('#Nombre').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Nombre').css('border-color', 'lightgrey');
    }
    if ($('#Clave').val().trim() == "") {
        $('#Clave').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Clave').css('border-color', 'lightgrey');
    }
    if ($('#TipoUsuarioId value:selected').val() == "") {
        $('#TipoUsuarioId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TipoUsuarioId').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var userObj = {
        UsuarioId: $('#UsuarioId').val(),
        CodUsuario: $('#CodUsuario').val(),
        Nombre: $('#Nombre').val(),
        Clave: $('#Clave').val(),
        TipoUsuarioId: $('#TipoUsuarioId').val(),
    };
    $.ajax({
        url: "/Users/Crear",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide')
            $('#UsuarioId').val("");
            $('#Nombre').val("");
            $('#Clave').val("");
            $('#CodUsuario').val("");
            $('#TipoUsuarioId').html("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    $('#UsuarioId').val("");
    $('#Nombre').val("");
    $('#Clave').val("");
    $('#CodUsuario').val("");
    $('#TipoUsuarioId').html("");
    $('#Nombre').css('border-color', 'lightgrey');
    $('#Clave').css('border-color', 'lightgrey');
    $('#CodUsuario').css('border-color', 'lightgrey');
    $('#TipoUsuarioId').css('border-color', 'lightgrey');
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Users/listatipos/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = html += '<option value="" selected> Selecione una Opción</option>';;
            $.each(result, function (key, item) {
                html += '<option value="' + item.TipoUsuarioId + '" >' + item.Descripcion + '</option>';
            });
            $('#TipoUsuarioId').html(html);            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
   
}