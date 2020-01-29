$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/ProductPresentationPrices/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombreProducto + '</td>';
                html += '<td>' + item.NombreFabricante + '</td>';
                html += '<td>' + item.NombrePresentacion + '</td>';
                html += '<td>' + item.CantPresentacion + '</td>';
                html += '<td>' + item.Precio + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.PrecioProductoId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.PrecioProductoId + ')">Eliminar</a></center></td>';
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
    document.getElementById('myModalLabel').innerHTML = "Editar Precio Producto";
    $('#CodBarras').css('border-color', 'lightgrey');
    $('#NombreProducto').css('border-color', 'lightgrey');
    $('#Precio').css('border-color', 'lightgrey');
    $.ajax({
        url: "/ProductPresentationPrices/getbyID/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ProductDetailId').val(result.ProductDetailId);
            $('#PrecioProductoId').val(result.PrecioProductoId);
            $('#NombreProducto').val(result.NombreProducto);
            $('#CodBarras').val(result.CodBarras);
            $('#NombreFabricante').val(result.NombreFabricante);
            $('#Precio').val(result.Precio);
            var html = '';
            $.each(result.ListaNombrePresentacion, function (key, item) {
                if (result.NombrePresentacion == item.NombrePresentacion) {
                    html += '<option value="' + item.NombrePresentacion + '" selected>' + item.NombrePresentacion + '</option>';
                }
                else {
                    html += '<option value="' + item.NombrePresentacion + '" >' + item.NombrePresentacion + '</option>';
                }
            });
            $('#NombrePresentacion').html(html);
            html = '';
            $.each(result.ListaPresentacion, function (key, item) {
                if (result.CantPresentacion == item.CantPresentacion) {
                    html += '<option value="' + item.PresentationId + '" selected>' + item.CantPresentacion + '</option>';
                }
                else {
                    html += '<option value="' + item.PresentationId + '" >' + item.CantPresentacion + '</option>';
                }
            });
            $('#PresentationId').html(html);
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
                url: "/ProductPresentationPrices/Borrar/" + Id,
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
        PrecioProductoId: $('#PrecioProductoId').val(),
        ProductDetailId: $('#ProductDetailId').val(),
        PresentationId: $('#PresentationId').val(),
        Precio: $('#Precio').val(),
    };
    $.ajax({
        url: "/ProductPresentationPrices/Editar",
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
                $('#ProductDetailId').val("");
                $('#CodBarras').val("");
                $('#RegInvima').val("");
                $('#Existencias').val("");
                $('#Fabricante').html("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#CodBarras').val().trim() == "") {
        $('#CodBarras').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#CodBarras').css('border-color', 'lightgrey');
    }
    if ($('#NombrePresentacion value:selected').val() == "") {
        $('#NombrePresentacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NombrePresentacion').css('border-color', 'lightgrey');
    }
    if ($('#PresentationId value:selected').val() == "") {
        $('#PresentationId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PresentationId').css('border-color', 'lightgrey');
    }
    if ($('#Precio').val().trim() == "") {
        $('#Precio').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Precio').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var userObj = {
        PrecioProductoId: $('#PrecioProductoId').val(),
        ProductDetailId: $('#ProductDetailId').val(),
        PresentationId: $('#PresentationId').val(),
        Precio: $('#Precio').val(),
    };
    $.ajax({
        url: "/ProductPresentationPrices/Crear",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide')
            $('#ProductDetailId').val("");
            $('#ProductoId').val("");
            $('#CodBarras').val("");
            $('#RegInvima').val("");
            $('#Existencias').val("");
            $('#Fabricante').html("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    document.getElementById('myModalLabel').innerHTML = "Crear Precio Producto";
    $('#ProductDetailId').val("");
    $('#ProductoId').val("");
    $('#NombreProducto').val("");
    $('#CodBarras').val("");
    $('#Precio').val("");
    $('#NombrePresentacion').html("");
    $('#PresentationId').html("");
    $('#CodBarras').css('border-color', 'lightgrey');
    $('#Precio').css('border-color', 'lightgrey');

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/ProductPresentationPrices/listaPresentacion",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<option value="" selected> Selecione una Opción</option>';
            $.each(result, function (key, item) {
                html += '<option value=' + item.NombrePresentacion + ' >' + item.NombrePresentacion + '</option>';
            });
            $('#NombrePresentacion').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $('#myModal').modal('show');
}
function clearBuscar() {
    $('#BuscarProducto').val("");
    $('#detalle').html("");
    $('#myModal1').modal('show');
}

function buscar(nombrePresentacion) {

    $.ajax({
        url: "/ProductPresentationPrices/BuscarXNombrePresentation",
        data: '{presentacion: "' + nombrePresentacion + '" }',
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<option value="" selected> Selecione una Opción</option>';
            $.each(result, function (key, item) {
                html += '<option value="' + item.PresentationId + '" >' + item.CantPresentacion + '</option>';
                });
            $('#PresentationId').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function buscarProduct() {
    var CodBarra = $('#CodBarras').val();
    if (CodBarra.length == 13) {
        $.ajax({
            url: "/ProductPresentationPrices/BuscarProducto",
            data: '{CodBarras: "' + CodBarra + '" }',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#ProductDetailId').val(result.ProductDetailId);
                $('#NombreProducto').val(result.NombreProducto);
                $('#NombreFabricante').val(result.NombreFabricante);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    else {
        swal.fire({
            title: "Estimado Usuario",
            text: "El codigo de barras debe tener una longitud de 13 caracteres.",
            icon: 'info',
            showCancelButton: false,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Aceptar",
        });
        return false;

    }
}