$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Products/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombreProducto + '</td>';
                html += '<td>' + item.Descripcion + '</td>';
                html += '<td>' + item.Componentes + '</td>';
                html += '<td>' + item.MinStock + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.ProductoId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.ProductoId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('#maestro').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(Id) {
    document.getElementById('myModalLabel').innerHTML = "Editar Producto";
    $('#NombreProducto').css('border-color', 'lightgrey');
    $('#Descripcion').css('border-color', 'lightgrey');
    $('#Componentes').css('border-color', 'lightgrey');
    $('#MinStock').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Products/getbyID/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ProductoId').val(result.ProductoId);
            $('#NombreProducto').val(result.NombreProducto);
            /*var html = '';
            $.each(result.ListaTerceros, function (key, item) {
                if (result.TerceroId == item.TerceroId) {
                    html += '<option value="' + item.TerceroId + '" selected>' + item.Descripcion + '</option>';
                }
                else {
                    html += '<option value="' + item.TerceroId + '" >' + item.Descripcion + '</option>';
                }
            });
            $('#TerceroId').html(html);*/
            $('#Descripcion').val(result.Descripcion);
            $('#Componentes').val(result.Componentes);
            $('#MinStock').val(result.MinStock);

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
                url: "/Products/Borrar/" + Id,
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
        ProductoId: $('#ProductoId').val(),
        NombreProducto: $('#NombreProducto').val(),
        MinStock: $('#MinStock').val(),
        Descripcion: $('#Descripcion').val(),
        Componentes: $('#Componentes').val(),
    };
    $.ajax({
        url: "/Products/Editar",
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
                $('#ProductoId').val("");
                $('#NombreProducto').val("");
                $('#Componentes').val("");
                $('#MinStock').val("");
                $('#Descripcion').val("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#NombreProducto').val().trim() == "") {
        $('#NombreProducto').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NombreProducto').css('border-color', 'lightgrey');
    }
    if ($('#TerceroId value:selected').val() == "") {
        $('#TerceroId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TerceroId').css('border-color', 'lightgrey');
    }
    if ($('#Componentes').val().trim() == "") {
        $('#Componentes').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Componentes').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var userObj = {
        ProductoId: $('#ProductoId').val(),
        NombreProducto: $('#NombreProducto').val(),
        MinStock: $('#MinStock').val(),
        Descripcion: $('#Descripcion').val(),
        Componentes: $('#Componentes').val(),
    };
    $.ajax({
        url: "/Products/Crear",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide')
            $('#ProductoId').val("");
            $('#NombreProducto').val("");
            $('#Componentes').val("");
            $('#MinStock').val("");
            $('#Descripcion').val("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    document.getElementById('myModalLabel').innerHTML = "Agregar Pago Tercero";
    $('#ProductoId').val("");
    $('#NombreProducto').val("");
    $('#Componentes').val("");
    $('#MinStock').val("");
    $('#Descripcion').val("");

    $('#NombreProducto').css('border-color', 'lightgrey');
    $('#Componentes').css('border-color', 'lightgrey');
    $('#MinStock').css('border-color', 'lightgrey');
    $('#Descripcion').css('border-color', 'lightgrey');

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    /*$.ajax({
        url: "/Products/listaTerceros/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = html += '<option value="" selected> Selecione una Opción</option>';;
            $.each(result, function (key, item) {
                html += '<option value="' + item.TerceroId + '" >' + item.Descripcion + '</option>';
            });
            $('#TerceroId').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });*/
    $('#myModal').modal('show');

}