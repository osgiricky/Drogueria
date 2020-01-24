$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/ProductDetails/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.CodBarras + '</td>';
                html += '<td>' + item.RegInvima + '</td>';
                html += '<td>' + item.Existencias + '</td>';
                html += '<td>' + item.NombreProducto + '</td>';
                html += '<td>' + item.NombreFabricante + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.ProductDetailId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.ProductDetailId + ')">Eliminar</a></center></td>';
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
    document.getElementById('myModalLabel').innerHTML = "Editar Detalle Producto";
    $('#CodBarras').css('border-color', 'lightgrey');
    $('#Existencias').css('border-color', 'lightgrey');
    $('#Fabricante').css('border-color', 'lightgrey');
    $.ajax({
        url: "/ProductDetails/getbyID/" + Id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ProductDetailId').val(result.ProductDetailId);
            $('#ProductoId').val(result.ProductoId);
            $('#NombreProducto').val(result.NombreProducto);
            $('#CodBarras').val(result.CodBarras);
            $('#RegInvima').val(result.RegInvima);
            $('#Existencias').val(result.Existencias);
            var html = '';
            $.each(result.ListaFabricantes, function (key, item) {
                if (result.MarkerId == item.MarkerId) {
                    html += '<option value="' + item.MarkerId + '" selected>' + item.NombreFabricante + '</option>';
                }
                else {
                    html += '<option value="' + item.MarkerId + '" >' + item.NombreFabricante + '</option>';
                }
            });
            $('#MarkerId').html(html);

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
                url: "/ProductDetails/Borrar/" + Id,
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
        ProductDetailId: $('#ProductDetailId').val(),
        CodBarras: $('#CodBarras').val(),
        RegInvima: $('#RegInvima').val(),
        Existencias: $('#Existencias').val(),
        ProductoId: $('#ProductoId').val(),
        MarkerId: $('#MarkerId').val(),
    };
    $.ajax({
        url: "/ProductDetails/Editar",
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
    if ($('#Existencias value:selected').val() == "") {
        $('#Existencias').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Existencias').css('border-color', 'lightgrey');
    }
    if ($('#MarkerId').val().trim() == "") {
        $('#MarkerId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#MarkerId').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var userObj = {
        CodBarras: $('#CodBarras').val(),
        RegInvima: $('#RegInvima').val(),
        Existencias: $('#Existencias').val(),
        ProductoId: $('#ProductoId').val(),
        MarkerId: $('#MarkerId').val(),
    };
    $.ajax({
        url: "/ProductDetails/Crear",
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
    $('#myModal1').modal('hide');
    $('.modal-backdrop').remove();
    document.getElementById('myModalLabel').innerHTML = "Crear Detalle de Producto";
    $('#PrecioProductoId').val("");
    $('#ProductoId').val("");
    $('#ProductDetailId').val("");
    $('#CodBarras').val("");
    $('#RegInvima').val("");
    $('#Existencias').val("");
    $('#Fabricante').html("");
    $('#CodBarras').css('border-color', 'lightgrey');
    $('#Existencias').css('border-color', 'lightgrey');
    $('#Fabricante').css('border-color', 'lightgrey');

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/ProductDetails/listaFabricantes/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#NombreProducto').val(result.EDProduct.NombreProducto);
            var html = html += '<option value="" selected> Selecione una Opción</option>';;
            $.each(result.ListaEDMarker, function (key, item) {
                html += '<option value="' + item.MarkerId + '" >' + item.NombreFabricante + '</option>';
            });
            $('#MarkerId').html(html);
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

function buscar() {
    var ProductName = {
        NombreProducto: $('#BuscarProducto').val(),
    };
    $.ajax({
        url: "/ProductDetails/BuscarXNombre/",
        data: JSON.stringify(ProductName),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result.ListaProductos, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombreProducto + '</td>';
                html += '<td>' + item.Descripcion + '</td>';
                html += '<td><center><a href="#" onclick="return clearTextBox(' + item.ProductoId + ')">Seleccionar</a></center></td>';
                html += '</tr>';
            });
            $('#detalle').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}